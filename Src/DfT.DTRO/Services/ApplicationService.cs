namespace DfT.DTRO.Services;

public class ApplicationService : IApplicationService
{
    private readonly IApplicationDal _applicationDal;
    private readonly IApigeeAppRepository _apigeeAppRepository;

    public ApplicationService(IApplicationDal applicationDal, IApigeeAppRepository apigeeAppRepository)
    {
        _applicationDal = applicationDal;
        _apigeeAppRepository = apigeeAppRepository;
    }

    public bool ValidateAppBelongsToUser(string userId, string appId)
    {
        Guid appGuid = Guid.Parse(appId);
        string user = _applicationDal.GetApplicationUser(appGuid);
        return user == userId;
    }

    public bool ValidateApplicationName(string appName)
    {
        return _applicationDal.CheckApplicationNameDoesNotExist(appName);
    }

    public ApplicationDetailsDto GetApplicationDetails(string appId)
    {
        return _applicationDal.GetApplicationDetails(appId);
    }

    public List<ApplicationListDto> GetApplicationList(string userId)
    {
        return _applicationDal.GetApplicationList(userId);
    }

    public async Task<App> CreateApplication(AppInput appInput)
    {
        var username = appInput.Username;
        var developerAppInput = JsonHelper.ConvertObject<AppInput, ApigeeDeveloperAppInput>(appInput);
        var developerApp = await _apigeeAppRepository.CreateApp(username, developerAppInput);
        return JsonHelper.ConvertObject<ApigeeDeveloperApp, App>(developerApp);
    }
}