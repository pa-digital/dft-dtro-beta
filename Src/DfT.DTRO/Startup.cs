using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
        var jwtSettings = Configuration.GetSection("Jwt");
        var key = System.Environment.GetEnvironmentVariable("Jwt__Key");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var cookie = context.Request.Cookies["jwtToken"];
                    if (!string.IsNullOrEmpty(cookie))
                    {
                        context.Token = cookie;
                    }
                    return Task.CompletedTask;
                }
            };

            options.RequireHttpsMetadata = true;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
            };
        });

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

        services.Scan(scan => scan
            .FromAssemblies(Assembly.GetExecutingAssembly())
            .AddClasses(classes => classes.InNamespaces(
                "DfT.DTRO.Services",
                "DfT.DTRO.DAL",
                "DfT.DTRO.Apis.Clients",
                "DfT.DTRO.Apis.Repositories",
                "DfT.DTRO.Utilities"))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(classes => classes.InNamespaces("DfT.DTRO.Services.Mapping"))
            .AsImplementedInterfaces()
            .WithSingletonLifetime()
        );
        services.AddScoped<SecretManagerClient>();
        services.AddSingleton<LoggingExtension>();
        services.TryAddSingleton<ISystemClock, SystemClock>();
        services.AddHttpClient();
        services.AddStorage(Configuration);
        services.AddValidationServices();
        services.AddCache(Configuration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        app.UseRouting();

        app.UseAuthentication();

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