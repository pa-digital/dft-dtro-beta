using System.Net.Http;
using DfT.DTRO.Models.Apigee;

namespace DfT.DTRO.Apis.Clients;

public interface IApigeeClient
{
    /// <summary>
    /// Create app
    /// </summary>
    /// <param name="developerEmail">Developer email parameter passed</param>
    /// <param name="developerAppInput">Developer application input parameter passed</param>
    /// <returns>App</returns>
    Task<HttpResponseMessage> CreateApp(string developerEmail, ApigeeDeveloperAppInput developerAppInput);

    /// <summary>
    /// Get app
    /// </summary>
    /// <param name="developerEmail">Developer email parameter passed</param>
    /// <param name="name">App name</param>
    /// <returns>App</returns>
    Task<HttpResponseMessage> GetApp(string developerEmail, string name);
    
    /// <summary>
    /// Update app status
    /// </summary>
    /// <param name="developerEmail">Developer email parameter passed</param>
    /// <param name="name">App name</param>
    /// <returns>App</returns>
    Task<HttpResponseMessage> UpdateAppStatus(string developerEmail, string name, string action);
    
    /// <summary>
    /// Delete developer
    /// </summary>
    /// <param name="developerEmail">Developer email parameter passed</param>
    /// <returns>App</returns>
    Task<HttpResponseMessage> DeleteDeveloper(string developerEmail);
}