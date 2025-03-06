namespace DfT.DTRO.Services
{

    public interface IApplicationService
    {
        bool ValidateAppBelongsToUser(string userId, string appId);
        bool ValidateApplicationName(string appName);
        ApplicationDetailsDto GetApplicationDetails(string appId);
        List<ApplicationListDto> GetApplicationList(string userId);
        PaginatedResponse<ApplicationListDto> GetPendingApplications(PaginatedRequest request);
    

    
    /// <summary>
    /// Create app
    /// </summary>
    /// <param name="parameters">Parameters passed</param>
    /// <returns>App</returns>
    Task<App> CreateApplication(AppInput appInput);
}

