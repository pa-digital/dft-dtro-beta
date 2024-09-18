
using Dft.DTRO.Admin;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

var apiBaseUrl =
    Environment.GetEnvironmentVariable("BASE_URL") ??
    builder.Configuration.GetValue<string>("ExternalApi:BaseUrl");

ConfigHelper.Version =
    Environment.GetEnvironmentVariable("VERSION_URL") ??
    builder.Configuration.GetValue<string>("ExternalApi:VersionUrl");


builder.Services.AddHttpClient("ExternalApi", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
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

var app = builder.Build();

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

app.MapRazorPages();

app.Run();
