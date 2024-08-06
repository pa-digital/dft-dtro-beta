namespace DfT.DTRO.Models;

///<inheritdoc cref="SecuritySchemeSettings"/>
public class SecuritySchemeSettings
{
    private readonly IConfiguration _configuration;

    ///<inheritdoc cref="SecuritySchemeSettings"/>
    public SecuritySchemeSettings(IConfiguration configuration) =>
        _configuration = configuration;

    ///<inheritdoc cref="SecuritySchemeSettings"/>
    public string In =>
        Environment.GetEnvironmentVariable(nameof(In)) ??
        _configuration.GetProperty<string>(nameof(SecuritySchemeSettings), nameof(In));

    ///<inheritdoc cref="SecuritySchemeSettings"/>
    public string Description =>
        Environment.GetEnvironmentVariable(nameof(Description)) ??
        _configuration.GetProperty<string>(nameof(SecuritySchemeSettings), nameof(Description));

    ///<inheritdoc cref="SecuritySchemeSettings"/>
    public string Name =>
        Environment.GetEnvironmentVariable(nameof(Name)) ??
        _configuration.GetProperty<string>(nameof(SecuritySchemeSettings), nameof(Name));

    ///<inheritdoc cref="SecuritySchemeSettings"/>
    public string Scheme =>
        Environment.GetEnvironmentVariable(nameof(Scheme)) ??
        _configuration.GetProperty<string>(nameof(SecuritySchemeSettings), nameof(Scheme));

    ///<inheritdoc cref="SecuritySchemeSettings"/>
    public string Type =>
        Environment.GetEnvironmentVariable(nameof(Type)) ??
        _configuration.GetProperty<string>(nameof(SecuritySchemeSettings), nameof(Type));
}