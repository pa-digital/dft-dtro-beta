namespace DfT.DTRO.Models;

public class InfoSettings
{
    private readonly IConfiguration _configuration;

    public InfoSettings(IConfiguration configuration) =>
        _configuration = configuration;

    public string Version =>
        Environment.GetEnvironmentVariable(nameof(Version)) ??
        _configuration.GetProperty<string>(nameof(InfoSettings), nameof(Version));

    public string Title =>
        Environment.GetEnvironmentVariable(nameof(Title)) ??
        _configuration.GetProperty<string>(nameof(InfoSettings), nameof(Title));

    public string Description =>
        Environment.GetEnvironmentVariable(nameof(Description)) ??
        _configuration.GetProperty<string>(nameof(InfoSettings), nameof(Description));

    public string TermsOfService =>
        Environment.GetEnvironmentVariable(nameof(TermsOfService)) ??
        _configuration.GetProperty<string>(nameof(InfoSettings), nameof(TermsOfService));

    public string ContactName =>
        Environment.GetEnvironmentVariable(nameof(ContactName)) ??
        _configuration.GetProperty<string>(nameof(InfoSettings), nameof(ContactName));

    public string ContactEmail =>
        Environment.GetEnvironmentVariable(nameof(ContactEmail)) ??
        _configuration.GetProperty<string>(nameof(InfoSettings), nameof(ContactEmail));

    public string ContactUrl =>
        Environment.GetEnvironmentVariable(nameof(ContactUrl)) ??
        _configuration.GetProperty<string>(nameof(InfoSettings), nameof(ContactUrl));

    public string LicenseName =>
        Environment.GetEnvironmentVariable(nameof(LicenseName)) ??
        _configuration.GetProperty<string>(nameof(InfoSettings), nameof(LicenseName));

    public string LicenseUrl =>
        Environment.GetEnvironmentVariable(nameof(LicenseUrl)) ??
        _configuration.GetProperty<string>(nameof(InfoSettings), nameof(LicenseUrl));


}