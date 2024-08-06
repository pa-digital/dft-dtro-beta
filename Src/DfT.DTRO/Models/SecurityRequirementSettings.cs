namespace DfT.DTRO.Models;

///<inheritdoc cref="SecurityRequirementSettings"/>
public class SecurityRequirementSettings
{
    private readonly IConfiguration _configuration;

    ///<inheritdoc cref="SecurityRequirementSettings"/>
    public SecurityRequirementSettings(IConfiguration configuration) =>
        _configuration = configuration;

    ///<inheritdoc cref="SecurityRequirementSettings"/>
    public string Id =>
        Environment.GetEnvironmentVariable(nameof(Id)) ??
        _configuration.GetProperty<string>(nameof(SecurityRequirementSettings), nameof(Id));

    ///<inheritdoc cref="SecurityRequirementSettings"/>
    public string In =>
        Environment.GetEnvironmentVariable(nameof(In)) ??
        _configuration.GetProperty<string>(nameof(SecurityRequirementSettings), nameof(In));

    ///<inheritdoc cref="SecurityRequirementSettings"/>
    public string Name =>
        Environment.GetEnvironmentVariable(nameof(Name)) ??
        _configuration.GetProperty<string>(nameof(SecurityRequirementSettings), nameof(Name));

    ///<inheritdoc cref="SecurityRequirementSettings"/>
    public string Scheme =>
        Environment.GetEnvironmentVariable(nameof(Scheme)) ??
        _configuration.GetProperty<string>(nameof(SecurityRequirementSettings), nameof(Scheme));

    ///<inheritdoc cref="SecurityRequirementSettings"/>
    public string Type =>
        Environment.GetEnvironmentVariable(nameof(Type)) ??
        _configuration.GetProperty<string>(nameof(SecurityRequirementSettings), nameof(Type));
}