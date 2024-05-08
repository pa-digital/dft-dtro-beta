using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using System;
using System.IO;

namespace DfT.DTRO.Extensions.Configuration;

/// <summary>
/// Provides extension methods for configuring Serilog.
/// </summary>
public static class SerilogConfigurationExtensions
{
    /// <summary>
    /// Configures an <see cref="IHostBuilder" /> to use Serilog for logging with the default settings for the DTRO Api.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The default settings are to use Google Cloud Logging (GCL) in production and console in development.
    /// </para>
    /// <para>
    /// Additionally if the <c>GOOGLE_APPLICATION_CREDENTIAL</c> key is present in the <see cref="IConfiguration"/>,
    /// GCL will also be used to log development logs, with the <c>ServiceName</c> reflecting the dev environment, user and machine.
    /// </para>
    /// <para>
    /// By default logs from namespaces <see cref="Microsoft"/> and <see cref="System"/>
    /// below <see cref="LogEventLevel.Warning"/> are not included when logging to GCL.
    /// </para>
    /// </remarks>
    /// <param name="builder">The <see cref="IHostBuilder" /> instance to configure.</param>
    /// <returns>A reference to the <paramref name="builder"/> after the operation has completed.</returns>
    public static IHostBuilder ConfigureSerilog(this IHostBuilder builder)
    {
        return builder.UseSerilog((context, config) =>
        {
            config.Enrich.FromLogContext();

            if (context.HostingEnvironment.IsDevelopment())
            {
                // Use console when in development.
                var devConsoleConfiguration = new ConfigurationBuilder().AddJsonFile("appsettings.Development.Serilog.json", true, true).Build();
                config.WriteTo.Logger(log => log.ReadFrom.Configuration(devConsoleConfiguration));

                // If credentials are defined, log to Google Cloud Logging as well
                if (context.Configuration["GOOGLE_APPLICATION_CREDENTIALS"] is string credentials
                    && context.Configuration["DevGclConfig"] is string devGclConfig)
                {
                    if (!File.Exists(devGclConfig))
                    {
                        GenerateDefaultGclConfig(devGclConfig, context.Configuration);
                    }

                    var devGclConfiguration = new ConfigurationBuilder().AddJsonFile(devGclConfig, true, true).Build();

                    config.WriteTo.Logger(log => log.ReadFrom.Configuration(devGclConfiguration));
                }

                return;
            }

            // In production log only to GCL with default credentials
            var prodGclConfig = new ConfigurationBuilder().AddJsonFile("appsettings.Serilog.json").Build();

            config.WriteTo.Logger(log => log.ReadFrom.Configuration(prodGclConfig));
        });
    }

    /// <summary>
    /// A filter that matches logs to be excluded from Google Cloud Logging.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> for logs that come from namespaces <see cref="Microsoft"/> or <see cref="System"/> and are below <see cref="LogEventLevel.Warning"/>
    /// and <see langword="false"/> for all other logs.
    /// </returns>
    private static bool GclExclusionFilter(LogEvent log)
    {
        return
            (Matching.FromSource("Microsoft")(log) || Matching.FromSource("System")(log))
            && log.Level < LogEventLevel.Warning;
    }

    private static void GenerateDefaultGclConfig(string devGclConfig, IConfiguration config)
    {
        string json =
            @"{
    ""Serilog"": {
        ""Using"": [
            ""Serilog.Sinks.GoogleCloudLogging"",
            ""Serilog.Expressions"",
            ""Serilog.Settings.Configuration""
        ],
        ""Filter"": [
            {
                ""Name"": ""ByExcluding"",
                ""Args"": {
                    ""expression"": ""(StartsWith(SourceContext, 'Microsoft') or StartsWith(SourceContext, 'System')) and (@l = 'Information' or @l = 'Debug' or @l = 'Verbose')""
                }
            }
        ],
        ""WriteTo"": [
            {
                ""Name"": ""GoogleCloudLogging"",
                ""Args"": {
                    ""projectID"": """ + (config["ProjectId"] ?? "[Add your project Id]") + @""",
                    ""serviceName"": """ + $"DTRO_Api/Dev/{Environment.UserName}@{Environment.MachineName}" + @""", 
                    ""serviceVersion"": ""alpha""
                }
            }
        ]
    }
}";

        File.WriteAllText(devGclConfig, json);
    }
}
