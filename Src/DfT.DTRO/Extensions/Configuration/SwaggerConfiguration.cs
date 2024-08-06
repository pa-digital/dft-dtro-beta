namespace DfT.DTRO.Extensions.Configuration;

[ExcludeFromCodeCoverage]
public static class SwaggerConfiguration
{
    private static InfoSettings _infoSettings;

    public static void AddSwagger(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        //var securitySchemeSettings = new SecuritySchemeSettings(configuration);
        //var securityRequirementSettings = new SecurityReqiurementSettings(configuration);
        _infoSettings = new InfoSettings(configuration);
        services.AddSwaggerGen(options =>
        {
            var openApiSecurityScheme = new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "",
                Name = "Authorization",
                Scheme = "Bearer",
                Type = SecuritySchemeType.ApiKey
            };
            options.AddSecurityDefinition("Bearer", openApiSecurityScheme);

            var openApiSecurityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Name = "Bearer",
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        },
                        Scheme = "oauth2"
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