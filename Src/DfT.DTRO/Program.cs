using DfT.DTRO.Extensions.Configuration;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace DfT.DTRO;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureSerilog()
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
                if (context.HostingEnvironment.IsDevelopment())
                {
                    config.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: false);
                }

                config.AddEnvironmentVariables();
                config.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.Local.example.json", optional: true, reloadOnChange: false);
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
}