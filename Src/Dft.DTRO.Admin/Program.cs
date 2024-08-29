var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

var apiBaseUrl =
    Environment.GetEnvironmentVariable("BASE_URL") ??
    builder
    .Configuration
    .GetValue<string>("ExternalApi:BaseUrl");

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

builder.Services.AddHealthChecks();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.MapHealthChecks("/health");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
