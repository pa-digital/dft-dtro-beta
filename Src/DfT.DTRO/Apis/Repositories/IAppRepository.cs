using DfT.DTRO.Models.App;

namespace DfT.DTRO.Apis.Repositories;

public interface IAppRepository
{
    
    /// <summary>
    /// CreateApp
    /// </summary>
    /// <param name="parameters">Parameters passed</param>
    /// <returns>App</returns>
    Task<App> CreateApp(AppInput appInput);
    
}