namespace DfT.DTRO.Apis.Clients;

public interface ISecretManagerClient
{
    /// <summary>
    /// Get secret
    /// </summary>
    /// <param name="secretName">secret name</param>
    /// <returns>App</returns>
    string GetSecret(string secretName);

}