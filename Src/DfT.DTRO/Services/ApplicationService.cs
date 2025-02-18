namespace DfT.DTRO.Services

{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationDal _applicationDal;

        public ApplicationService(IApplicationDal applicationDal)
        {
            _applicationDal = applicationDal;
        }

        public bool ValidateApplicationName(string appName)
        {
            return _applicationDal.CheckApplicationNameDoesNotExist(appName);
        }
    }
}
