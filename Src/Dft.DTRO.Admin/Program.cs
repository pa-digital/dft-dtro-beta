using Dft.DTRO.Admin.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

var apiBaseUrl = builder.Configuration.GetValue<string>("ExternalApi:BaseUrl");
builder.Services.AddHttpClient("ExternalApi", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

builder.Services.AddScoped<ISchemaService, SchemaService>();
builder.Services.AddScoped<IRuleService, RuleService>();
builder.Services.AddScoped<IDtroService, DtroService>();
builder.Services.AddScoped<IMetricsService, MetricsService>();
builder.Services.AddScoped<ITraService, TraService>();
builder.Services.AddScoped<ISystemConfigService, SystemConfigService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
