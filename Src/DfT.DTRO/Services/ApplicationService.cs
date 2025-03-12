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

    public async Task<bool> ValidateAppBelongsToUser(string email, Guid appId)
    {
        string userEmail = await _applicationDal.GetApplicationUser(appId);
        return userEmail == email;
    }

    public async Task<bool> ValidateApplicationName(string appName)
    {
        return await _applicationDal.CheckApplicationNameDoesNotExist(appName);
    }

    public async Task<ApplicationDetailsDto> GetApplication(Guid appId)
    {
        return await _applicationDal.GetApplicationDetails(appId);
    }

    public async Task<List<ApplicationListDto>> GetApplications(string email)
    {
        return await _applicationDal.GetApplicationList(email);
    }

    public async Task<PaginatedResponse<ApplicationPendingListDto>> GetPendingApplications(PaginatedRequest paginatedRequest)
    {
        PaginatedResult<ApplicationPendingListDto> paginatedResult =  await _applicationDal.GetPendingApplications(paginatedRequest);
        return new(paginatedResult.Results.ToList().AsReadOnly(), paginatedRequest.Page, paginatedResult.TotalCount);
    }

    public async Task<bool> ActivateApplicationById(Guid appId)
    {
        // TODO: approve application on Apigee
        return await _applicationDal.ActivateApplicationById(appId);
    }

    public async Task<App> CreateApplication(string email, AppInput appInput)
    {
        var developerAppInput = JsonHelper.ConvertObject<AppInput, ApigeeDeveloperAppInput>(appInput);
        var developerApp = await _apigeeAppRepository.CreateApp(email, developerAppInput);
        return JsonHelper.ConvertObject<ApigeeDeveloperApp, App>(developerApp);
    }
}