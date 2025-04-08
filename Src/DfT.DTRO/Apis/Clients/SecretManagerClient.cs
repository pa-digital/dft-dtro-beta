namespace DfT.DTRO.Apis.Clients;

/// <inheritdoc cref="ISecretManagerClient"/>
public class SecretManagerClient : ISecretManagerClient
{
    private readonly IConfiguration _configuration;

    /// <inheritdoc cref="ISecretManagerClient"/>
    public SecretManagerClient(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <inheritdoc cref="ISecretManagerClient"/>
    public string GetSecret(string secretName)
    {
        var apiProject = _configuration.GetValue<string>("ApiSettings:ApiProject");
        var secretManagerServiceClient = SecretManagerServiceClient.Create();
        var secretVersion = secretManagerServiceClient.AccessSecretVersion($"projects/{apiProject}/secrets/{secretName}/versions/latest");
        return secretVersion.Payload.Data.ToStringUtf8();
    }
}