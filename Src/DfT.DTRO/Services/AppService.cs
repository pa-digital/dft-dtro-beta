using DfT.DTRO.Apis.Repositories;
using DfT.DTRO.Models.App;

namespace DfT.DTRO.Services;

public class AppService : IAppService
{
    
    private readonly IAppRepository _appRepository;

    public AppService(IAppRepository appRepository) =>
        _appRepository = appRepository;
    
    public async Task<App> CreateApp(AppInput appInput, string accessToken)
    {
       return await _appRepository.CreateApp(appInput, accessToken);
    }
}