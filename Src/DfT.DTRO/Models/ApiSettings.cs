﻿namespace DfT.DTRO.Models;

[ExcludeFromCodeCoverage]
public class ApiSettings
{
    private readonly IConfiguration _configuration;

    public ApiSettings(IConfiguration configuration) =>
        _configuration = configuration;

    public string Version =>
        Environment.GetEnvironmentVariable(nameof(Version)) ??
        Get<string>(_configuration, nameof(Version));

    public string Title =>
        Environment.GetEnvironmentVariable(nameof(Title)) ??
        Get<string>(_configuration, nameof(Title));

    public string Description =>
        Environment.GetEnvironmentVariable(nameof(Description)) ??
        Get<string>(_configuration, nameof(Description));

    public string TermsOfService =>
        Environment.GetEnvironmentVariable(nameof(TermsOfService)) ??
        Get<string>(_configuration, nameof(TermsOfService));

    private static T Get<T>(IConfiguration configuration, string property) =>
        configuration.GetSection(nameof(ApiSettings)).GetValue<T>(property);
}