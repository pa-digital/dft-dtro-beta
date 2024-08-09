namespace DfT.DTRO.Models;

public class SecuritySchemeSettings
{
    private readonly IConfiguration _configuration;

    public SecuritySchemeSettings(IConfiguration configuration) =>
        _configuration = configuration;

    public string In =>
        Environment.GetEnvironmentVariable(nameof(In)) ??
        _configuration.GetProperty<string>(nameof(SecuritySchemeSettings), nameof(In));

    public string Description =>
        Environment.GetEnvironmentVariable(nameof(Description)) ??
        _configuration.GetProperty<string>(nameof(SecuritySchemeSettings), nameof(Description));

    public string Name =>
        Environment.GetEnvironmentVariable(nameof(Name)) ??
        _configuration.GetProperty<string>(nameof(SecuritySchemeSettings), nameof(Name));

    public string Scheme =>
        Environment.GetEnvironmentVariable(nameof(Scheme)) ??
        _configuration.GetProperty<string>(nameof(SecuritySchemeSettings), nameof(Scheme));

    public string Type =>
        Environment.GetEnvironmentVariable(nameof(Type)) ??
        _configuration.GetProperty<string>(nameof(SecuritySchemeSettings), nameof(Type));
}