using Serilog;

namespace DfT.DTRO.Extensions.Configuration;

public static class SerilogConfigurationExtensions
{
    public static IHostBuilder ConfigureSerilog(this IHostBuilder hostBuilder) =>
        hostBuilder.UseSerilog((context, configuration) =>
        {
            configuration.Enrich.FromLogContext();

            IConfigurationRoot configurationRoot = new ConfigurationBuilder()
                .AddJsonFile(GetAppSettings(hostBuilder), optional: false, reloadOnChange: true)
                .Build();

            configuration.WriteTo.Logger(log => log.ReadFrom.Configuration(configurationRoot));
        });

    private static string GetAppSettings(IHostBuilder hostBuilder) => "appsettings.json";
}