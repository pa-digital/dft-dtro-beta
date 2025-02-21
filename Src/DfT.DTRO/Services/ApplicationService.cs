
namespace DfT.DTRO.Services

{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationDal _applicationDal;

        public ApplicationService(IApplicationDal applicationDal)
        {
            _applicationDal = applicationDal;
        }

        public bool ValidateAppBelongsToUser(string userId, string appId) {
            Guid appGuid = Guid.Parse(appId);
            Guid user = _applicationDal.GetApplicationUser(appGuid);
            Guid userGuid = Guid.Parse(userId);
            return user == userGuid;
        }

        public bool ValidateApplicationName(string appName)
        {
            return _applicationDal.CheckApplicationNameDoesNotExist(appName);
        }

        public ApplicationDetailsDto GetApplicationDetails(string appId)
        {
            return _applicationDal.GetApplicationDetails(appId);
        }

        public List<ApplicationListDto> GetApplicationList(string userId)
        {
            return _applicationDal.GetApplicationList(userId);
        }

    }
}
