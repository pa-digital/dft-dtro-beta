namespace DfT.DTRO.Models;

public class OpenApiSecuritySchemeSettings
{
    private readonly IConfiguration _configuration;

    public OpenApiSecuritySchemeSettings(IConfiguration configuration) =>
        _configuration = configuration;

    public string In =>
        Environment.GetEnvironmentVariable(nameof(In)) ??
        _configuration.GetProperty<string>(nameof(OpenApiSecuritySchemeSettings), nameof(In));

    public string Description =>
        Environment.GetEnvironmentVariable(nameof(Description)) ??
        _configuration.GetProperty<string>(nameof(OpenApiSecuritySchemeSettings), nameof(Description));

    public string Name =>
        Environment.GetEnvironmentVariable(nameof(Name)) ??
        _configuration.GetProperty<string>(nameof(OpenApiSecuritySchemeSettings), nameof(Name));

    public string Scheme =>
        Environment.GetEnvironmentVariable(nameof(Scheme)) ??
        _configuration.GetProperty<string>(nameof(OpenApiSecuritySchemeSettings), nameof(Scheme));

    public string Type =>
        Environment.GetEnvironmentVariable(nameof(Type)) ??
        _configuration.GetProperty<string>(nameof(OpenApiSecuritySchemeSettings), nameof(Type));
}