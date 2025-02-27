namespace DfT.DTRO.Services
{
    public interface IApplicationService
    {
        bool ValidateAppBelongsToUser(string userId, string appId);
        bool ValidateApplicationName(string appName);
        ApplicationDetailsDto GetApplicationDetails(string appId);
        List<ApplicationListDto> GetApplicationList(string userId);
        List<ApplicationListDto> GetPendingApplications(string userId);
    }
}
