using System.Net.Http;
using DfT.DTRO.Models.App;

namespace DfT.DTRO.Apis.Clients;

public interface IApigeeClient
{
    /// <summary>
    /// Create app
    /// </summary>
    /// <param name="appInput"></param>
    /// <param name="parameters">Parameters passed</param>
    /// <returns>App</returns>
    Task<HttpResponseMessage> CreateApp(AppInput appInput, string accessToken);
    
}