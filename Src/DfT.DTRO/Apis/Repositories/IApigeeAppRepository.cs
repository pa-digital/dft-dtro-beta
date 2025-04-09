namespace DfT.DTRO.Apis.Repositories;

public interface IApigeeAppRepository
{

    /// <summary>
    /// Create app
    /// </summary>
    /// <param name="developerEmail">Developer email parameter passed</param>
    /// <param name="developerAppInput">Developer application input parameter passed</param>
    /// <returns>App</returns>
    Task<ApigeeDeveloperApp> CreateApp(string developerEmail, ApigeeDeveloperAppInput developerAppInput);

    /// <summary>
    /// Get app
    /// </summary>
    /// <param name="developerEmail">Developer email parameter passed</param>
    /// <param name="name">app name</param>
    /// <returns>App</returns>
    Task<ApigeeDeveloperApp> GetApp(string developerEmail, string name);
    
    /// <summary>
    /// Delete app
    /// </summary>
    /// <param name="developerEmail">Developer email parameter passed</param>
    /// <param name="name">app name</param>
    /// <returns>App</returns>
    Task<ApigeeDeveloperApp> DeleteApp(string developerEmail, string name);
    
    /// <summary>
    /// Update app status
    /// </summary>
    /// <param name="developerEmail">Developer email parameter passed</param>
    /// <param name="name">app name</param>
    /// <returns>App</returns>
    Task<ApigeeDeveloperApp> UpdateAppStatus(string developerEmail, string name, string action);
}