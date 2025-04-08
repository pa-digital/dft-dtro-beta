namespace DfT.DTRO.DAL;

public interface IApplicationDal
{
    Task<string> GetApplicationUser(Guid appId);
    Task<bool> CheckApplicationNameDoesNotExist(string appName);
    Task<ApplicationDetailsDto> GetApplicationDetails(Guid appId);
    Task<PaginatedResponse<ApplicationListDto>> GetApplicationList(string email, PaginatedRequest paginatedRequest);
    Task<PaginatedResult<ApplicationInactiveListDto>> GetInactiveApplications(PaginatedRequest paginatedRequest);
    Task<bool> ActivateApplicationById(Guid appId);
    Task CreateApplication(Application application);
    Task<int> GetUserApplicationsCount(Guid userId);
}