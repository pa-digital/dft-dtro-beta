namespace DfT.DTRO.Apis.Clients;

/// <summary>
/// Secret manager client
/// </summary>
public interface ISecretManagerClient
{
    /// <summary>
    /// Get secret
    /// </summary>
    /// <param name="secretName">secret name</param>
    /// <returns>secret value</returns>
    string GetSecret(string secretName);

}