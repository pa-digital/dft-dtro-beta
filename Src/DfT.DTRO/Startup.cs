using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace DfT.DTRO;

/// <summary>
/// Startup class
/// </summary>
[ExcludeFromCodeCoverage]
public class Startup
{
    private IWebHostEnvironment Environment { get; }

    private IConfiguration Configuration { get; }

    /// <summary>
    /// Application startup
    /// </summary>
    /// <param name="env">Environment passed in</param>
    /// <param name="configuration">Configuration passed in</param>
    public Startup(IWebHostEnvironment env, IConfiguration configuration)
    {
        Environment = env;
        Configuration = configuration;
    }

    /// <summary>
    /// Service configuration
    /// </summary>
    /// <param name="services">The service collection</param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services
            .AddMvc(options =>
            {
                options.InputFormatters.RemoveType<SystemTextJsonInputFormatter>();
                options.OutputFormatters.RemoveType<SystemTextJsonOutputFormatter>();
            })
            .AddNewtonsoftJson(opts =>
            {
                opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                opts.SerializerSettings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));

                opts.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            })
            .AddXmlSerializerFormatters();

        services.AddRepositories();

        services.AddSwagger(Configuration, Environment);
        services.AddHealthChecks();

        services.AddFeatureManagement();

        services.AddScoped<IAppIdMapperService, AppIdMapperService>();
        services.AddScoped<IJsonSchemaValidationService, JsonSchemaValidationService>();
        services.AddScoped<ISemanticValidationService, SemanticValidationService>();
        services.AddScoped<IGeometryValidation, GeometryValidation>();
        services.AddSingleton<IBoundingBoxService, BoundingBoxService>();
        services.AddScoped<IOldConditionValidationService, OldConditionValidationService>();
        services.AddSingleton<ISpatialProjectionService, Proj4SpatialProjectionService>();
        services.AddScoped<IDtroGroupValidatorService, DtroGroupValidatorService>();
        services.AddSingleton<IDtroMappingService, DtroMappingService>();
        services.AddSingleton<ISchemaTemplateMappingService, SchemaTemplateMappingService>();
        services.AddSingleton<IRuleTemplateMappingService, RuleTemplateMappingService>();
        services.AddScoped<IEventSearchService, EventSearchService>();
        services.AddScoped<ISchemaTemplateService, SchemaTemplateService>();
        services.AddScoped<IRuleTemplateService, RuleTemplateService>();
        services.AddScoped<IDtroService, DtroService>();
        services.AddScoped<ISearchService, SearchService>();
        services.AddScoped<IMetricsService, MetricsService>();
        services.AddScoped<IDtroUserService, DtroUserService>();
        services.AddScoped<ISystemConfigService, SystemConfigService>();
        services.AddSingleton<LoggingExtension>();

        services.TryAddSingleton<ISystemClock, SystemClock>();

        services.AddStorage(Configuration);
        services.AddValidationServices();
        services.AddRequestCorrelation();
        services.AddCache(Configuration);
    }

    /// <summary>
    /// Configure application
    /// </summary>
    /// <param name="app">Application to configure</param>
    /// <param name="env">Environment in which application is configured</param>
    /// <param name="loggerFactory">Factory logger</param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        app.UseRouting();

        app.UseRequestCorrelation();

        // 

        app.UseAuthorization();

        if (env.IsDevelopment())
        {
            app.UseCustomSwagger();
        }

        // Add the middleware before UseEndpoints
        app.UserGroupMiddleware();

        app.UseMiddleware<SecurityHeadersMiddleware>();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.UseHealthChecks("/health");

        //app.EmptyDtroUsersTable();
        //app.SeedAppData();

        app.Seed(SeedData.Users);
        app.Seed(SeedData.UserStatuses);
        app.Seed(SeedData.TrafficRegulationAuthorities);
        app.Seed(SeedData.DigitalServiceProviders);
        app.Seed(SeedData.TrafficRegulationAuthorityDigitalServiceProviders);
        app.Seed(SeedData.TrafficRegulationAuthorityDigitalServiceProviderStatuses);
        app.Seed(SeedData.Applications);
        app.Seed(SeedData.ApplicationTypes);
        app.Seed(SeedData.ApplicationPurposes);
        app.Seed(SeedData.SchemaTemplates);
        app.Seed(SeedData.RuleTemplates);
        app.Seed(SeedData.DigitalTrafficRegulationOrders);
        app.Seed(SeedData.DigitalTrafficRegulationOrderHistories);

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }
    }

}