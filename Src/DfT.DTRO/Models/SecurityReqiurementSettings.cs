namespace DfT.DTRO.Models;

public class SecurityReqiurementSettings
{
    private readonly IConfiguration _configuration;

    public SecurityReqiurementSettings(IConfiguration configuration) =>
        _configuration = configuration;

    public string Id =>
        Environment.GetEnvironmentVariable(nameof(Id)) ??
        _configuration.GetProperty<string>(nameof(SecurityReqiurementSettings), nameof(Id));

    public string In =>
        Environment.GetEnvironmentVariable(nameof(In)) ??
        _configuration.GetProperty<string>(nameof(SecurityReqiurementSettings), nameof(In));

    public string Name =>
        Environment.GetEnvironmentVariable(nameof(Name)) ??
        _configuration.GetProperty<string>(nameof(SecurityReqiurementSettings), nameof(Name));

    public string Scheme =>
        Environment.GetEnvironmentVariable(nameof(Scheme)) ??
        _configuration.GetProperty<string>(nameof(SecurityReqiurementSettings), nameof(Scheme));

    public string Type =>
        Environment.GetEnvironmentVariable(nameof(Type)) ??
        _configuration.GetProperty<string>(nameof(SecurityReqiurementSettings), nameof(Type));
}