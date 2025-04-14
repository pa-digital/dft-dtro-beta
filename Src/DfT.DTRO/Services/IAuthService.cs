namespace DfT.DTRO.Services;

/// <summary>
/// Authentication service
/// </summary>
public interface IAuthService
{

    /// <summary>
    /// Get auth token
    /// </summary>
    /// <param name="authTokenInput">Parameter passed</param>
    /// <returns>Auth token</returns>
    Task<AuthToken> GetToken(AuthTokenInput authTokenInput);
}