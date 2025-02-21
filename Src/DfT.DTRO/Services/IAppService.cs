using DfT.DTRO.Models.App;

namespace DfT.DTRO.Services;

public interface IAppService
{
    
    /// <summary>
    /// Create app
    /// </summary>
    /// <param name="parameters">Parameters passed</param>
    /// <returns>App</returns>
    Task<App> CreateApp(AppInput appInput, string accessToken);
    
}