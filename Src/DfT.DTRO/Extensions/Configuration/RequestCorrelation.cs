namespace DfT.DTRO.Extensions.Configuration;

///<inheritdoc cref="RequestCorrelationConfiguration"/>
[ExcludeFromCodeCoverage]
public static class RequestCorrelationConfiguration
{
    ///<inheritdoc cref="RequestCorrelationConfiguration"/>
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

    ///<inheritdoc cref="RequestCorrelationConfiguration"/>
    public static IApplicationBuilder UseRequestCorrelation(this IApplicationBuilder app)
        => app.UseMiddleware<RequestCorrelationMiddleware>()
              .UseMiddleware<RequestCorrelationEnricherMiddleware>();
}
