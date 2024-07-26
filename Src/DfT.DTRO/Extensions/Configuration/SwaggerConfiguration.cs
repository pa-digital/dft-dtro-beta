namespace DfT.DTRO.Extensions.Configuration;

[ExcludeFromCodeCoverage]
public static class SwaggerConfiguration
{
    public static void AddSwagger(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        var apiSettings = new ApiSettings(configuration);
        services.AddSwaggerGen(options =>
        {
            var openApiInfo = new OpenApiInfo
            {
                Version = apiSettings.Version,
                Title = apiSettings.Title,
                Description = apiSettings.Description,
                TermsOfService = new Uri(apiSettings.TermsOfService),
                Contact = new OpenApiContact
                {
                    Name = apiSettings.ContactName,
                    Url = new Uri(apiSettings.ContactUrl),
                },
                License = new OpenApiLicense
                {
                    Name = apiSettings.LicenseName,
                    Url = new Uri(apiSettings.LicenseUrl)
                }
            };

            options.SwaggerDoc(apiSettings.Version, openApiInfo);
            options.CustomSchemaIds(type => type.FullName);
            options.IncludeXmlComments($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{environment.ApplicationName}.xml");
            options.OperationFilter<GeneratePathParamsValidationFilter>();
            options.OperationFilter<CorrelationIdHeaderParameterFilter>();
            options.EnableAnnotations();
            options.DocumentFilter<FeatureGateFilter>();
            options.SchemaFilter<BoundingBoxSchemaFilter>();
        }).AddSwaggerGenNewtonsoftSupport();
    }
}