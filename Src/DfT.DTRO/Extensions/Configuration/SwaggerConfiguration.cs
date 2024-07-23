namespace DfT.DTRO.Extensions.Configuration;

[ExcludeFromCodeCoverage]
public static class SwaggerConfiguration
{
    public static void AddSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        var apiSettings = new ApiSettings(configuration);
        services.AddSwaggerGen(options =>
        {
            var openApiInfo = new OpenApiInfo
            {
                Version = apiSettings.Version,
                Title = apiSettings.Title,
                Description = apiSettings.Description,
                TermsOfService = new Uri(apiSettings.TermsOfService)
            };

            options.SwaggerDoc(apiSettings.Version, openApiInfo);
            options.CustomSchemaIds(type => type.FullName);
            options.OperationFilter<GeneratePathParamsValidationFilter>();
            options.OperationFilter<CorrelationIdHeaderParameterFilter>();
            options.EnableAnnotations();
            options.DocumentFilter<FeatureGateFilter>();
            options.SchemaFilter<BoundingBoxSchemaFilter>();
        }).AddSwaggerGenNewtonsoftSupport();
    }
}