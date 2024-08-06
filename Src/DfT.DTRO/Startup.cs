using DfT.DTRO.DAL;
using DfT.DTRO.Extensions.Configuration;
using DfT.DTRO.Extensions.DependencyInjection;
using DfT.DTRO.Filters;
using DfT.DTRO.Services.Conversion;
using DfT.DTRO.Services.Mapping;
using DfT.DTRO.Services.Validation;
using DfT.DTRO.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Internal;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
namespace DfT.DTRO;

public class Startup
{
    private readonly IWebHostEnvironment _hostingEnv;

    private IConfiguration Configuration { get; }

    public Startup(IWebHostEnvironment env, IConfiguration configuration)
    {
        _hostingEnv = env;
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
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

        services
            .AddSwaggerGen(c =>
            {
                c.SwaggerDoc("0.0.1", new OpenApiInfo
                {
                    Version = "0.0.1",
                    Title = "DTRO - OpenAPI 3.0",
                    Description = "DTRO - OpenAPI 3.0",
                    TermsOfService = new Uri("https://cloud.google.com/terms")
                });
                c.CustomSchemaIds(type => type.FullName);

                c.OperationFilter<GeneratePathParamsValidationFilter>();

                c.OperationFilter<CorrelationIdHeaderParameterFilter>();

                c.EnableAnnotations();

                c.DocumentFilter<FeatureGateFilter>();

                c.SchemaFilter<BoundingBoxSchemaFilter>();
            })
            .AddSwaggerGenNewtonsoftSupport();

        services.AddHealthChecks();

        services.AddFeatureManagement();

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

        services.AddStorage(Configuration);
        services.AddJsonLogic();
        services.AddRequestCorrelation();
        services.AddCache(Configuration);
        services.TryAddSingleton<ISystemClock, SystemClock>();
        services.AddMvc();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        app.UseRouting();

        app.UseRequestCorrelation();
        app.UseMiddleware<SecurityHeadersMiddleware>();

        app.UseAuthorization();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/0.0.1/swagger.json", "DTRO - OpenAPI 3.0");
            c.RoutePrefix = string.Empty;
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.UseHealthChecks("/health");

        DbInitialize.SeedAppData(app);
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            //TODO: The line below will be removed once access
            //TODO: to query the deployed database is granted
            DbInitialize.RunSqlStatement(app);
        }
        else
        {
            app.UseExceptionHandler("/Error");

            app.UseHsts();
        }
    }
}