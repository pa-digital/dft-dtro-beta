using DfT.DTRO.Models.Auth;

namespace DfT.DTRO.Services;

public interface IAuthService
{

    /// <summary>
    /// Get auth token
    /// </summary>
    /// <param name="parameters">Parameters passed</param>
    /// <returns>Auth token</returns>
    Task<AuthToken> GetToken(AuthTokenInput authTokenInput);

}