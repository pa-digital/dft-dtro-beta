using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using Google.Cloud.Diagnostics.Common;
using Microsoft.AspNetCore.Connections;

namespace DfT.DTRO;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                using (var stream = new FileStream("", FileMode.Open, FileAccess.Read))
                {
                    var userCredential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.FromStream(stream).Secrets,
                        new[] { "dtro-dev-dft-dtro-beta" },
                        "gabriel.popescu@dft.gov.uk",
                        CancellationToken.None,
                        new FileDataStore("dft-dtro-dev-01")).Result;
                }

                services
                    .AddGoogleDiagnostics("dft-dtro-dev-01",
                            "dtro-dev-dft-dtro-beta",
                            traceOptions: TraceOptions.Create(),
                            loggingOptions: LoggingOptions.Create(),
                            errorReportingOptions: ErrorReportingOptions.Create());
            })
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
                if (context.HostingEnvironment.IsDevelopment())
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
                }

                config.AddEnvironmentVariables();
                config.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: false);
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureKestrel(serverOptions =>
                    {
                        serverOptions.Limits.MaxRequestBodySize = 100_000_000;
                    })
                    .UseStartup<Startup>();
            });
}