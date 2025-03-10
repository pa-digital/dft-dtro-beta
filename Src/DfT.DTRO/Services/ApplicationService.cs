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

    public async Task<bool> ValidateAppBelongsToUser(string userId, string appId)
    {
        Guid appGuid = Guid.Parse(appId);
        string user = await _applicationDal.GetApplicationUser(appGuid);
        return user == userId;
    }

    public async Task<bool> ValidateApplicationName(string appName)
    {
        return await _applicationDal.CheckApplicationNameDoesNotExist(appName);
    }

    public async Task<ApplicationDetailsDto> GetApplicationDetails(string appId)
    {
        return await _applicationDal.GetApplicationDetails(appId);
    }

    public async Task<List<ApplicationListDto>> GetApplicationList(string userId)
    {
        return await _applicationDal.GetApplicationList(userId);
    }

    public async Task<bool> ActivateApplicationById(string applicationId)
    {
        Guid appGuid = Guid.Parse(applicationId);
        // TODO: approve application on Apigee
        return await _applicationDal.ActivateApplicationById(appGuid);
    }

    public async Task<App> CreateApplication(string email, AppInput appInput)
    {
        var developerAppInput = JsonHelper.ConvertObject<AppInput, ApigeeDeveloperAppInput>(appInput);
        var developerApp = await _apigeeAppRepository.CreateApp(email, developerAppInput);
        return JsonHelper.ConvertObject<ApigeeDeveloperApp, App>(developerApp);
    }
}