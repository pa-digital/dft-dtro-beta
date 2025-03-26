namespace DfT.DTRO.DAL;

public interface IApplicationDal
{
    Task<string> GetApplicationUser(Guid appId);
    Task<bool> CheckApplicationNameDoesNotExist(string appName);
    Task<ApplicationDetailsDto> GetApplicationDetails(Guid appId);
    Task<List<ApplicationListDto>> GetApplicationList(string email);
    PaginatedResult<ApplicationInactiveListDto> GetInactiveApplications(PaginatedRequest paginatedRequest);
    Task<bool> ActivateApplicationById(Guid appId);
}