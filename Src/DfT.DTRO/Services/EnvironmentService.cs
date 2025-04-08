namespace DfT.DTRO.Services;

public class EnvironmentService : IEnvironmentService
{
    private readonly IApplicationDal _applicationDal;
    private readonly IUserDal _userDal;


    public EnvironmentService(IApplicationDal applicationDal, IUserDal userDal)
    {
        _applicationDal = applicationDal;
        _userDal = userDal;
    }

    public async Task<bool> CanRequestProductionAccess(string email)
    {
        User user = await _userDal.GetUserFromEmail(email);
        int numApplications = await _applicationDal.GetUserApplicationsCount(user.Id);
        bool hasRequested = await _userDal.HasUserRequestedProductionAccess(user.Id);
        return numApplications > 0 && !hasRequested;
    }

    public async Task RequestProductionAccess(string email)
    {
        User user = await _userDal.GetUserFromEmail(email);
        await _userDal.RequestProductionAccess(user.Id);

        // TODO: email CSO inbox to confirm
        // TODO: email user to confirm
    }
}