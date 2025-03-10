using System.Net.Http;
using DfT.DTRO.Models.Auth;

namespace DfT.DTRO.Apis.Clients;

public interface IAuthClient
{
    /// <summary>
    /// Get auth token
    /// </summary>
    /// <param name="authTokenInput"></param>
    /// <param name="parameters">Parameters passed</param>
    /// <returns>Auth token</returns>
    Task<HttpResponseMessage> GetToken(AuthTokenInput authTokenInput);

}