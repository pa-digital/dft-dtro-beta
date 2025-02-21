namespace DfT.DTRO.DAL

{
    public interface IApplicationDal
    {
        Guid GetApplicationUser(Guid appId);
        bool CheckApplicationNameDoesNotExist(string appName);
        ApplicationDetailsDto GetApplicationDetails(string appId);
        List<ApplicationListDto> GetApplicationList(string userId);
    }
}
