namespace DfT.DTRO.Models;

public class OpenApiInfoSettings
{
    private readonly IConfiguration _configuration;

    public OpenApiInfoSettings(IConfiguration configuration) =>
        _configuration = configuration;

    public string Version =>
        Environment.GetEnvironmentVariable(nameof(Version)) ??
        _configuration.GetProperty<string>(nameof(OpenApiInfoSettings), nameof(Version));

    public string Title =>
        Environment.GetEnvironmentVariable(nameof(Title)) ??
        _configuration.GetProperty<string>(nameof(OpenApiInfoSettings), nameof(Title));

    public string Description =>
        Environment.GetEnvironmentVariable(nameof(Description)) ??
        _configuration.GetProperty<string>(nameof(OpenApiInfoSettings), nameof(Description));

    public string TermsOfService =>
        Environment.GetEnvironmentVariable(nameof(TermsOfService)) ??
        _configuration.GetProperty<string>(nameof(OpenApiInfoSettings), nameof(TermsOfService));

    public string ContactName =>
        Environment.GetEnvironmentVariable(nameof(ContactName)) ??
        _configuration.GetProperty<string>(nameof(OpenApiInfoSettings), nameof(ContactName));

    public string ContactUrl =>
        Environment.GetEnvironmentVariable(nameof(ContactUrl)) ??
        _configuration.GetProperty<string>(nameof(OpenApiInfoSettings), nameof(ContactUrl));

    public string LicenseName =>
        Environment.GetEnvironmentVariable(nameof(LicenseName)) ??
        _configuration.GetProperty<string>(nameof(OpenApiInfoSettings), nameof(LicenseName));

    public string LicenseUrl =>
        Environment.GetEnvironmentVariable(nameof(LicenseUrl)) ??
        _configuration.GetProperty<string>(nameof(OpenApiInfoSettings), nameof(LicenseUrl));


}