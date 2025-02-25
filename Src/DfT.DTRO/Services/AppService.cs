using DfT.DTRO.Apis.Repositories;
using DfT.DTRO.Models.Apigee;
using DfT.DTRO.Models.App;

namespace DfT.DTRO.Services;

public class AppService : IAppService
{
    
    private readonly IApigeeAppRepository _apigeeAppRepository;

    public AppService(IApigeeAppRepository apigeeAppRepository) =>
        _apigeeAppRepository = apigeeAppRepository;
    
    public async Task<App> CreateApp(AppInput appInput)
    {
        var username = appInput.Username;
        var developerAppInput = JsonHelper.ConvertObject<AppInput, ApigeeDeveloperAppInput>(appInput);
        var developerApp = await _apigeeAppRepository.CreateApp(username, developerAppInput);
        return JsonHelper.ConvertObject<ApigeeDeveloperApp, App>(developerApp);
    }
}