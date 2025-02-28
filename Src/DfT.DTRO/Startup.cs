using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Scrutor;

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
        services.AddScoped<IPortalUserDal, PortalUserDal>();
        services.AddScoped<IPortalUserService, PortalUserService>();

        services.Scan(scan => scan
                .FromAssemblies(Assembly.GetExecutingAssembly())
                .AddClasses(classes => classes.InNamespaces("DfT.DTRO.Services")) 
                .AsImplementedInterfaces()
                .WithScopedLifetime() 
                .AddClasses(classes => classes.InNamespaces("DfT.DTRO.DAL")) 
                .AsImplementedInterfaces()
                .WithScopedLifetime() 
                .AddClasses(classes => classes.InNamespaces("DfT.DTRO.Services.Mapping"))
                .AsImplementedInterfaces()
                .WithSingletonLifetime()
        );
        services.AddScoped<AuthClient, AuthClient>();
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