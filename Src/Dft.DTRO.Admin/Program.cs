using Dft.DTRO.Admin;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

ConfigHelper.ApiBaseUrl =
    Environment.GetEnvironmentVariable("BASE_URL") ??
    builder.Configuration.GetValue<string>("ExternalApi:BaseUrl");

ConfigHelper.Version =
    Environment.GetEnvironmentVariable("VERSION_URL") ??
    builder.Configuration.GetValue<string>("ExternalApi:VersionUrl");

ConfigHelper.ClientId =
    Environment.GetEnvironmentVariable("CLIENT_ID") ??
    builder.Configuration.GetValue<string>("CLIENT_ID");

ConfigHelper.ClientSecret =
    Environment.GetEnvironmentVariable("CLIENT_SECRET") ??
    builder.Configuration.GetValue<string>("CLIENT_SECRET");

ConfigHelper.XAppIdOverride =
    Environment.GetEnvironmentVariable("X_APP_ID_OVERRIDE") ??
    builder.Configuration.GetValue<string>("X_APP_ID_OVERRIDE");

ConfigHelper.TokenEndpoint =
    Environment.GetEnvironmentVariable("TOKEN_ENDPOINT") ??
    builder.Configuration.GetValue<string>("TOKEN_ENDPOINT");

builder.Services.AddHttpClient("ExternalApi", client =>
{
    client.BaseAddress = new Uri(ConfigHelper.ApiBaseUrl);
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    return new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
        {
            #if DEBUG
            return true;

            #else
            return sslPolicyErrors == System.Net.Security.SslPolicyErrors.None;

            #endif
        }
    };
});

builder.Services.AddScoped<ISchemaService, SchemaService>();
builder.Services.AddScoped<IRuleService, RuleService>();
builder.Services.AddScoped<IDtroService, DtroService>();
builder.Services.AddScoped<IMetricsService, MetricsService>();
builder.Services.AddScoped<IDtroUserService, DtroUserService>();
builder.Services.AddScoped<ISystemConfigService, SystemConfigService>();

builder.Services.AddScoped<IErrHandlingService, ErrHandlingService>();

builder.Services.AddScoped<IXappIdService, XappIdService>();

builder.Services.AddSingleton<IXappIdService>(provider => new XappIdService(builder.Configuration));
builder.Services.AddHealthChecks();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();
builder.Services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();


builder.Services.AddHttpContextAccessor();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set the session timeout
    options.Cookie.HttpOnly = true; // Make the session cookie HTTP only
    options.Cookie.IsEssential = true; // Make the session cookie essential
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // All subsequent pages requiring the authenticated identity are HTTPS
    options.Cookie.SameSite = SameSiteMode.Strict; // Prevent the cookie from being sent by the browser to the target site in all cross-site browsing contexts
});

var app = builder.Build();

app.UseMiddleware<SecurityHeaders>();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHealthChecks("/health");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapRazorPages();

app.Run();
