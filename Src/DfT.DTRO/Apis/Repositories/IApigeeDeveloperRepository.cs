namespace DfT.DTRO.Apis.Repositories;

public interface IApigeeDeveloperRepository
{
    
    /// <summary>
    /// Delete developer
    /// </summary>
    /// <param name="developerEmail">Developer email parameter passed</param>
    /// <returns>App</returns>
    Task<ApigeeDeveloper> DeleteDeveloper(string developerEmail);
}