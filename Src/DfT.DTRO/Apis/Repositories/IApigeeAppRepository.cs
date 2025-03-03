using DfT.DTRO.Models.Apigee;

namespace DfT.DTRO.Apis.Repositories;

public interface IApigeeAppRepository
{
    
    /// <summary>
    /// CreateApp
    /// </summary>
    /// <param name="parameters">Parameters passed</param>
    /// <returns>App</returns>
    Task<ApigeeDeveloperApp> CreateApp(string developerEmail, ApigeeDeveloperAppInput developerAppInput);
    
}