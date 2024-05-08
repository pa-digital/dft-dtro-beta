using DfT.DTRO.RequestCorrelation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DfT.DTRO.Extensions.Configuration;

/// <summary>
/// Provides extensions that help in using the request correlation.
/// </summary>
public static class RequestCorrelationConfigurationExtensions
{
    /// <summary>
    /// Adds services required to make use of request correlation.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="sectionName">The configuration section to retrieve the options from.</param>
    public static IServiceCollection AddRequestCorrelation(this IServiceCollection services, string sectionName = "RequestCorrelation")
    {
        // this internally calls TryAddSingleton, so no worries if it gets called multiple times.
        return services
            .AddHttpContextAccessor()
            .AddScoped<RequestCorrelationMiddleware>()
            .AddScoped<RequestCorrelationEnricherMiddleware>()
            .AddScoped<IRequestCorrelationProvider, RequestCorrelationProvider>()
            .AddSingleton(s =>
            {
                RequestCorrelationOptions options = new ();
                s.GetService<IConfiguration>()
                    .GetSection(sectionName)
                    .Bind(options);
                return Options.Create(options);
            });
    }

    /// <summary>
    /// Adds request correlation to the application's request pipeline.
    /// </summary>
    /// <param name="app">The <see cref="IApplicationBuilder"/> instance.</param>
    /// <returns>The <see cref="IApplicationBuilder"/> instance with applied middleware.</returns>
    public static IApplicationBuilder UseRequestCorrelation(this IApplicationBuilder app)
        => app.UseMiddleware<RequestCorrelationMiddleware>()
              .UseMiddleware<RequestCorrelationEnricherMiddleware>();
}
