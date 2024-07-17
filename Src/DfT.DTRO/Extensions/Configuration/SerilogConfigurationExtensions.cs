using Serilog;

namespace DfT.DTRO.Extensions.Configuration;

public static class SerilogConfigurationExtensions
{
    public static IHostBuilder ConfigureSerilog(this IHostBuilder hostBuilder) =>
        hostBuilder.UseSerilog((context, configuration) =>
        {
            string path = null;

            configuration.Enrich.FromLogContext();

            if (context.HostingEnvironment.IsDevelopment())
            {
                path = GetAppSettings(hostBuilder, "Development");
            }
            else if (context.HostingEnvironment.IsStaging())
            {
                path = GetAppSettings(hostBuilder, "Staging");
            }
            else if (context.HostingEnvironment.IsProduction())
            {
                path = GetAppSettings(hostBuilder, "Production");
            }

            IConfigurationRoot configurationRoot = new ConfigurationBuilder()
                .AddJsonFile(path, optional: false, reloadOnChange: true)
                .Build();

            configuration.WriteTo.Logger(log => log.ReadFrom.Configuration(configurationRoot));
        });

    private static string GetAppSettings(IHostBuilder hostBuilder, string environment) =>
        $"appsettings.{environment}.json";
}