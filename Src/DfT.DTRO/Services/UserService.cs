namespace DfT.DTRO.Services;

public class UserService(IUserDal userDal) : IUserService
{
    public PaginatedResponse<UserDto> GetUsers(string userId)
    {
        var id = userId.ToGuid();
        var users = userDal.GetUsers(id);

        return new PaginatedResponse<UserDto>([], 0, 1);
    }
}