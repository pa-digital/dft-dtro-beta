using Microsoft.OpenApi.Extensions;

namespace DfT.DTRO.Extensions.Configuration;

[ExcludeFromCodeCoverage]
public static class SwaggerConfiguration
{
    public static void AddSwagger(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        var openApiSecuritySchemeSettings = new OpenApiSecuritySchemeSettings(configuration);
        var openApiSecurityRequirementSettings = new OpenApiSecurityReqiurementSettings(configuration);
        var openApiInfoSettings = new OpenApiInfoSettings(configuration);
        services.AddSwaggerGen(options =>
        {
            var openApiSecurityScheme = new OpenApiSecurityScheme
            {
                In = openApiSecuritySchemeSettings.In.GetEnumFromDisplayName<ParameterLocation>(),
                Description = openApiSecuritySchemeSettings.Description,
                Name = openApiSecuritySchemeSettings.Name,
                Scheme = openApiSecuritySchemeSettings.Scheme,
                Type = openApiSecuritySchemeSettings.Type.GetEnumFromDisplayName<SecuritySchemeType>()
            };
            options.AddSecurityDefinition("Bearer", openApiSecurityScheme);

            var openApiSecurityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        In = openApiSecurityRequirementSettings.In.GetEnumFromDisplayName<ParameterLocation>(),
                        Name = openApiSecurityRequirementSettings.Name,
                        Reference = new OpenApiReference
                        {
                            Id = openApiSecurityRequirementSettings.ReferenceId,
                            Type = openApiSecurityRequirementSettings.ReferenceType.GetEnumFromDisplayName<ReferenceType>(),
                        },
                        Scheme = openApiSecurityRequirementSettings.Scheme
                    },
                    new List<string>()
                }
            };
            options.AddSecurityRequirement(openApiSecurityRequirement);

            var openApiInfo = new OpenApiInfo
            {
                Version = openApiInfoSettings.Version,
                Title = openApiInfoSettings.Title,
                Description = openApiInfoSettings.Description,
                TermsOfService = new Uri(openApiInfoSettings.TermsOfService),
                Contact = new OpenApiContact
                {
                    Name = openApiInfoSettings.ContactName,
                    Url = new Uri(openApiInfoSettings.ContactUrl),
                },
                License = new OpenApiLicense
                {
                    Name = openApiInfoSettings.LicenseName,
                    Url = new Uri(openApiInfoSettings.LicenseUrl)
                }
            };

            options.SwaggerDoc(openApiInfoSettings.Version, openApiInfo);
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