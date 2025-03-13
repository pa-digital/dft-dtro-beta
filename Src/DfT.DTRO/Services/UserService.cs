namespace DfT.DTRO.Services;

public class UserService : IUserService
{
    private readonly IUserDal _userDal;

    public UserService(IUserDal userDal)
    {
        _userDal = userDal;
    }

    public async Task DeleteUser(string userId)
    {
        Guid userGuid = Guid.Parse(userId);
        await _userDal.DeleteUser(userGuid);
    }
}