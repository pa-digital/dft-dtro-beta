
namespace DfT.DTRO.Services;

public class ApplicationService : IApplicationService
{
    private readonly IApplicationDal _applicationDal;

    public ApplicationService(IApplicationDal applicationDal) => _applicationDal = applicationDal;

    public bool ValidateAppBelongsToUser(string userId, string appId)
    {
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

    public PaginatedResponse<ApplicationListDto> GetPendingApplications(ApplicationRequest request)
    {
        PaginatedResult<ApplicationListDto> paginatedResult = _applicationDal.GetPendingApplications(request);
        PaginatedResponse<ApplicationListDto> paginatedResponse =
            new(paginatedResult.Results.ToList().AsReadOnly(), request.Page, paginatedResult.TotalCount);

        return paginatedResponse;
    }
}