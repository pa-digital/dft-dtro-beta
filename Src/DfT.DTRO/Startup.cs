using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace DfT.DTRO;

[ExcludeFromCodeCoverage]
public class Startup
{
    private IWebHostEnvironment Environment { get; }

    private IConfiguration Configuration { get; }

    public Startup(IWebHostEnvironment env, IConfiguration configuration)
    {
        Environment = env;
        Configuration = configuration;
    }

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
        services.AddScoped<IDtroDal, DtroDal>();
        services.AddScoped<IDtroHistoryDal, DtroHistoryDal>();
        services.AddScoped<ISchemaTemplateDal, SchemaTemplateDal>();
        services.AddScoped<IRuleTemplateDal, RuleTemplateDal>();
        services.AddScoped<IMetricDal, MetricDal>();
        services.AddScoped<IDtroUserDal, DtroUserDal>();
        services.AddScoped<IDtroUserService, DtroUserService>();
        services.AddScoped<ISystemConfigDal, SystemConfigDal>();
        services.AddScoped<ISystemConfigService, SystemConfigService>();
        services.AddSingleton<LoggingExtension>();

        services.TryAddSingleton<ISystemClock, SystemClock>();

        services.AddStorage(Configuration);
        services.AddValidationServices();
        services.AddRequestCorrelation();
        services.AddCache(Configuration);
    }

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
        //DbInitialize.EmptyDtroUsersTable(app);
        DbInitialize.SeedAppData(app);
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