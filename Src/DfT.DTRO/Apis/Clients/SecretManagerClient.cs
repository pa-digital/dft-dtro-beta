using Google.Cloud.SecretManager.V1;

namespace DfT.DTRO.Apis.Clients;

public class SecretManagerClient : ISecretManagerClient
{
    private readonly IConfiguration _configuration;
    
    public SecretManagerClient(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public string GetSecret(string secretName)
    {
        var apiProject = _configuration.GetValue<string>("ApiSettings:ApiProject");
        var secretManagerServiceClient = SecretManagerServiceClient.Create();
        var secretVersion = secretManagerServiceClient.AccessSecretVersion($"projects/{apiProject}/secrets/{secretName}/versions/latest");
        return secretVersion.Payload.Data.ToStringUtf8();
    }
}