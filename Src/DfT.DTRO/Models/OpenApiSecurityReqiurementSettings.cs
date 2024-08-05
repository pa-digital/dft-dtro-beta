namespace DfT.DTRO.Models;

public class OpenApiSecurityReqiurementSettings
{
    private readonly IConfiguration _configuration;

    public OpenApiSecurityReqiurementSettings(IConfiguration configuration) =>
        _configuration = configuration;

    public string ReferenceId =>
        Environment.GetEnvironmentVariable(nameof(ReferenceId)) ??
        _configuration.GetProperty<string>(nameof(OpenApiSecurityReqiurementSettings), nameof(ReferenceId));

    public string In =>
        Environment.GetEnvironmentVariable(nameof(In)) ??
        _configuration.GetProperty<string>(nameof(OpenApiSecurityReqiurementSettings), nameof(In));

    public string Name =>
        Environment.GetEnvironmentVariable(nameof(Name)) ??
        _configuration.GetProperty<string>(nameof(OpenApiSecurityReqiurementSettings), nameof(Name));

    public string Scheme =>
        Environment.GetEnvironmentVariable(nameof(Scheme)) ??
        _configuration.GetProperty<string>(nameof(OpenApiSecurityReqiurementSettings), nameof(Scheme));

    public string ReferenceType =>
        Environment.GetEnvironmentVariable(nameof(ReferenceType)) ??
        _configuration.GetProperty<string>(nameof(OpenApiSecurityReqiurementSettings), nameof(ReferenceType));
}