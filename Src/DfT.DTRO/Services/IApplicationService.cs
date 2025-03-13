using DfT.DTRO.Models.Applications;

namespace DfT.DTRO.Services;

public interface IApplicationService
{
    Task<App> CreateApplication(string email, AppInput appInput);
    Task<bool> ValidateAppBelongsToUser(string email, Guid appId);
    Task<bool> ValidateApplicationName(string appName);
    Task<ApplicationDetailsDto> GetApplication(Guid appId);
    Task<List<ApplicationListDto>> GetApplications(string email);
    Task<PaginatedResponse<ApplicationInactiveListDto>> GetInactiveApplications(PaginatedRequest paginatedRequest);
    Task<bool> ActivateApplicationById(Guid appId);
}