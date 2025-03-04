namespace DfT.DTRO.DAL

{
    public interface IApplicationDal
    {
        string GetApplicationUser(Guid appId);
        bool CheckApplicationNameDoesNotExist(string appName);
        ApplicationDetailsDto GetApplicationDetails(string appId);
        List<ApplicationListDto> GetApplicationList(string userId);
        PaginatedResult<ApplicationListDto> GetPendingApplications(PaginatedRequest request);
    }
}
