using DfT.DTRO.RequestCorrelation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DfT.DTRO.Extensions.Configuration;

public static class RequestCorrelationConfigurationExtensions
{
    public static IServiceCollection AddRequestCorrelation(this IServiceCollection services, string sectionName = "RequestCorrelation")
    {
        return services
            .AddHttpContextAccessor()
            .AddScoped<RequestCorrelationMiddleware>()
            .AddScoped<RequestCorrelationEnricherMiddleware>()
            .AddScoped<IRequestCorrelationProvider, RequestCorrelationProvider>()
            .AddSingleton(s =>
            {
                RequestCorrelationOptions options = new();
                s.GetService<IConfiguration>()
                    .GetSection(sectionName)
                    .Bind(options);
                return Options.Create(options);
            });
    }

    public static IApplicationBuilder UseRequestCorrelation(this IApplicationBuilder app)
        => app.UseMiddleware<RequestCorrelationMiddleware>()
              .UseMiddleware<RequestCorrelationEnricherMiddleware>();
}
