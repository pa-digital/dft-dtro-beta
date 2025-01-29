using Dft.DTRO.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHttpClient("DtroApiClient", client =>
        {
            client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("BASE_URL")
                                         ?? "https://localhost:5001"); // TODO get URL secondary location
        });
        services.AddHostedService<Worker>();
    })
    .RunConsoleAsync();