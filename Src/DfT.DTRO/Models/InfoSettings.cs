namespace DfT.DTRO.Models;

///<inheritdoc cref="InfoSettings"/>
public class InfoSettings
{
    private readonly IConfiguration _configuration;

    ///<inheritdoc cref="InfoSettings"/>
    public InfoSettings(IConfiguration configuration) =>
        _configuration = configuration;

    ///<inheritdoc cref="InfoSettings"/>
    public string Version =>
        Environment.GetEnvironmentVariable(nameof(Version)) ??
        _configuration.GetProperty<string>(nameof(InfoSettings), nameof(Version));

    ///<inheritdoc cref="InfoSettings"/>
    public string Title =>
        Environment.GetEnvironmentVariable(nameof(Title)) ??
        _configuration.GetProperty<string>(nameof(InfoSettings), nameof(Title));

    ///<inheritdoc cref="InfoSettings"/>
    public string Description =>
        Environment.GetEnvironmentVariable(nameof(Description)) ??
        _configuration.GetProperty<string>(nameof(InfoSettings), nameof(Description));

    ///<inheritdoc cref="InfoSettings"/>
    public string TermsOfService =>
        Environment.GetEnvironmentVariable(nameof(TermsOfService)) ??
        _configuration.GetProperty<string>(nameof(InfoSettings), nameof(TermsOfService));

    ///<inheritdoc cref="InfoSettings"/>
    public string ContactName =>
        Environment.GetEnvironmentVariable(nameof(ContactName)) ??
        _configuration.GetProperty<string>(nameof(InfoSettings), nameof(ContactName));

    ///<inheritdoc cref="InfoSettings"/>
    public string ContactEmail =>
        Environment.GetEnvironmentVariable(nameof(ContactEmail)) ??
        _configuration.GetProperty<string>(nameof(InfoSettings), nameof(ContactEmail));

    ///<inheritdoc cref="InfoSettings"/>
    public string ContactUrl =>
        Environment.GetEnvironmentVariable(nameof(ContactUrl)) ??
        _configuration.GetProperty<string>(nameof(InfoSettings), nameof(ContactUrl));

    ///<inheritdoc cref="InfoSettings"/>
    public string LicenseName =>
        Environment.GetEnvironmentVariable(nameof(LicenseName)) ??
        _configuration.GetProperty<string>(nameof(InfoSettings), nameof(LicenseName));

    ///<inheritdoc cref="InfoSettings"/>
    public string LicenseUrl =>
        Environment.GetEnvironmentVariable(nameof(LicenseUrl)) ??
        _configuration.GetProperty<string>(nameof(InfoSettings), nameof(LicenseUrl));
}