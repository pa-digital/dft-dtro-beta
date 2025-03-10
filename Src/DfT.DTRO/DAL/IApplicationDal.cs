namespace DfT.DTRO.DAL;

public interface IApplicationDal
{
    Task<string> GetApplicationUser(Guid appId);
    Task<bool> CheckApplicationNameDoesNotExist(string appName);
    Task<ApplicationDetailsDto> GetApplicationDetails(string appId);
    Task<List<ApplicationListDto>> GetApplicationList(string userId);
    Task<bool> ActivateApplicationById(Guid appId);
}