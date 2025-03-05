namespace DfT.DTRO.Apis.Repositories;

public interface IApigeeAppRepository
{

    /// <summary>
    /// CreateApp
    /// </summary>
    /// <param name="developerEmail">Developer email parameter passed</param>
    /// <param name="developerAppInput">Developer application input parameter passed</param>
    /// <returns>App</returns>
    Task<ApigeeDeveloperApp> CreateApp(string developerEmail, ApigeeDeveloperAppInput developerAppInput);

}