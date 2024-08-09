namespace DfT.DTRO.Models;

public class SecurityRequirementSettings
{
    private readonly IConfiguration _configuration;

    public SecurityRequirementSettings(IConfiguration configuration) =>
        _configuration = configuration;

    public string Id =>
        Environment.GetEnvironmentVariable(nameof(Id)) ??
        _configuration.GetProperty<string>(nameof(SecurityRequirementSettings), nameof(Id));

    public string In =>
        Environment.GetEnvironmentVariable(nameof(In)) ??
        _configuration.GetProperty<string>(nameof(SecurityRequirementSettings), nameof(In));

    public string Name =>
        Environment.GetEnvironmentVariable(nameof(Name)) ??
        _configuration.GetProperty<string>(nameof(SecurityRequirementSettings), nameof(Name));

    public string Scheme =>
        Environment.GetEnvironmentVariable(nameof(Scheme)) ??
        _configuration.GetProperty<string>(nameof(SecurityRequirementSettings), nameof(Scheme));

    public string Type =>
        Environment.GetEnvironmentVariable(nameof(Type)) ??
        _configuration.GetProperty<string>(nameof(SecurityRequirementSettings), nameof(Type));
}