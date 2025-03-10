using DfT.DTRO.Models.Applications;

namespace DfT.DTRO.Services;

public interface IApplicationService
{
    Task<App> CreateApplication(AppInput appInput);
    Task<bool> ValidateAppBelongsToUser(string userId, string appId);
    Task<bool> ValidateApplicationName(string appName);
    Task<ApplicationDetailsDto> GetApplicationDetails(string appId);
    Task<List<ApplicationListDto>> GetApplicationList(string userId);
    Task<bool> ActivateApplicationById(string applicationId);
}