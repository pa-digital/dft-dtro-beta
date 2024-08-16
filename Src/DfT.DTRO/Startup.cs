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

        services.AddScoped<IJsonSchemaValidationService, JsonSchemaValidationService>();
        services.AddScoped<ISemanticValidationService, SemanticValidationService>();
        services.AddScoped<IConditionValidationService, ConditionValidationService>();
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
        services.AddScoped<ISwaCodeDal, SwaCodeDal>();
        services.AddScoped<ITraService, TraService>();
        services.AddScoped<ISystemConfigDal, SystemConfigDal>();
        services.AddScoped<ISystemConfigService, SystemConfigService>();
        services.TryAddSingleton<ISystemClock, SystemClock>();

        StorageService.AddStorage(services, Configuration);
        services.AddJsonLogic();
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

        app.UseMiddleware<SecurityHeadersMiddleware>();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.UseHealthChecks("/health");

        DbInitialize.GrantPermission(app);
        DbInitialize.EmptySwaCodesTable(app);
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