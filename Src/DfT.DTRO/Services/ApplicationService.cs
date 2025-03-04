using DfT.DTRO.Apis.Repositories;
using DfT.DTRO.Models.Apigee;
using DfT.DTRO.Models.Applications;

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

    public bool ValidateAppBelongsToUser(string userId, string appId) {
        Guid appGuid = Guid.Parse(appId);
        string user = _applicationDal.GetApplicationUser(appGuid);
        return user == userId;
    }


    public bool ValidateApplicationName(string appName) =>
        _applicationDal.CheckApplicationNameDoesNotExist(appName);

    public ApplicationDetailsDto GetApplicationDetails(string appId) =>
        _applicationDal.GetApplicationDetails(appId);

    public List<ApplicationListDto> GetApplicationList(string userId) =>
        _applicationDal.GetApplicationList(userId);

    public PaginatedResponse<ApplicationListDto> GetPendingApplications(PaginatedRequest request)
    {
        PaginatedResult<ApplicationListDto> paginatedResult = _applicationDal.GetPendingApplications(request);
        PaginatedResponse<ApplicationListDto> paginatedResponse =
            new(paginatedResult.Results.ToList().AsReadOnly(), request.Page, paginatedResult.TotalCount);

        return paginatedResponse;

   
    public async Task<App> CreateApplication(AppInput appInput)
    {
        var username = appInput.Username;
        var developerAppInput = JsonHelper.ConvertObject<AppInput, ApigeeDeveloperAppInput>(appInput);
        var developerApp = await _apigeeAppRepository.CreateApp(username, developerAppInput);
        return JsonHelper.ConvertObject<ApigeeDeveloperApp, App>(developerApp);

    }
}