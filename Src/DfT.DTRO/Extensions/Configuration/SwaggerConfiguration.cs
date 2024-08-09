using Microsoft.OpenApi.Extensions;

namespace DfT.DTRO.Extensions.Configuration;

///<inheritdoc cref="SwaggerConfiguration"/>
[ExcludeFromCodeCoverage]
public static class SwaggerConfiguration
{
    private static InfoSettings _infoSettings;

    ///<inheritdoc cref="SwaggerConfiguration"/>
    public static void AddSwagger(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        var securitySchemeSettings = new SecuritySchemeSettings(configuration);
        var securityRequirementSettings = new SecurityRequirementSettings(configuration);
        _infoSettings = new InfoSettings(configuration);
        services.AddSwaggerGen(options =>
        {
            var openApiSecurityScheme = new OpenApiSecurityScheme
            {
                In = securitySchemeSettings.In.GetEnumFromDisplayName<ParameterLocation>(),
                Description = securitySchemeSettings.Description,
                Name = securitySchemeSettings.Name,
                Scheme = securitySchemeSettings.Scheme,
                Type = securitySchemeSettings.Type.GetEnumFromDisplayName<SecuritySchemeType>()
            };
            options.AddSecurityDefinition(securitySchemeSettings.Scheme, openApiSecurityScheme);

            var openApiSecurityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        In = securityRequirementSettings
                            .In
                            .GetEnumFromDisplayName<ParameterLocation>(),
                        Name = securityRequirementSettings.Name,
                        Reference = new OpenApiReference
                        {
                            Id = securityRequirementSettings.Id,
                            Type = securityRequirementSettings
                                .Type
                                .GetEnumFromDisplayName<ReferenceType>()
                        },
                        Scheme = securityRequirementSettings.Scheme
                    },
                    new string[]
                    {

                    }
                }
            };
            options.AddSecurityRequirement(openApiSecurityRequirement);

            var openApiInfo = new OpenApiInfo
            {
                Version = _infoSettings.Version,
                Title = _infoSettings.Title,
                Description = _infoSettings.Description,
                TermsOfService = new Uri(_infoSettings.TermsOfService),
                Contact = new OpenApiContact
                {
                    Name = _infoSettings.ContactName,
                    Email = _infoSettings.ContactEmail,
                    Url = new Uri(_infoSettings.ContactUrl)
                },
                License = new OpenApiLicense
                {
                    Name = _infoSettings.LicenseName,
                    Url = new Uri(_infoSettings.LicenseUrl)
                }
            };
            options.SwaggerDoc(_infoSettings.Version, openApiInfo);

            options.CustomSchemaIds(type => type.FullName);
            options.IncludeXmlComments($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{environment.ApplicationName}.xml");
            options.OperationFilter<GeneratePathParamsValidationFilter>();
            options.OperationFilter<CorrelationIdHeaderParameterFilter>();
            options.EnableAnnotations();
            options.DocumentFilter<FeatureGateFilter>();
            options.SchemaFilter<BoundingBoxSchemaFilter>();
        }).AddSwaggerGenNewtonsoftSupport();
    }

    ///<inheritdoc cref="SwaggerConfiguration"/>
    public static void UseCustomSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint($"/swagger/{_infoSettings.Version}/swagger.json", _infoSettings.Title);
            options.RoutePrefix = string.Empty;
        });
    }
}