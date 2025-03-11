using DfT.DTRO.Models.Applications;

namespace DfT.DTRO.Services;

public interface IApplicationService
{
    Task<App> CreateApplication(string email, AppInput appInput);
    Task<bool> ValidateAppBelongsToUser(string email, Guid appId);
    Task<bool> ValidateApplicationName(string appName);
    Task<ApplicationDetailsDto> GetApplicationDetails(Guid appId);
    Task<List<ApplicationListDto>> GetApplicationList(string email);
    Task<bool> ActivateApplicationById(Guid appId);
}