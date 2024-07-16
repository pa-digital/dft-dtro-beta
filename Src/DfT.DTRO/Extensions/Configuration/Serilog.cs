using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Filters;

namespace DfT.DTRO.Extensions.Configuration;

public static class SerilogConfigurationExtensions
{
    public static IHostBuilder ConfigureSerilog(this IHostBuilder builder)
    {
        return builder.UseSerilog((context, config) =>
        {
            config.Enrich.FromLogContext();

            if (context.HostingEnvironment.IsDevelopment())
            {
                var devConsoleConfiguration = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json", true, true).Build();
                config.WriteTo.Logger(log => log.ReadFrom.Configuration(devConsoleConfiguration));

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

            var prodGclConfig = new ConfigurationBuilder().AddJsonFile("appsettings.Production.json").Build();

            config.WriteTo.Logger(log => log.ReadFrom.Configuration(prodGclConfig));
        });
    }

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
