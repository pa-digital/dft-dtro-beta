namespace DfT.DTRO.Services;

public class UserService : IUserService
{
    private readonly IUserDal _userDal;

    public UserService(IUserDal userDal)
    {
        _userDal = userDal;
    }
    
    public async Task<PaginatedResponse<UserListDto>> GetUsers(PaginatedRequest paginatedRequest)
    {
        var paginatedResult = await _userDal.GetUsers(paginatedRequest);
        return new(paginatedResult.Results.ToList().AsReadOnly(), paginatedRequest.Page, paginatedResult.TotalCount);
    }

    public async Task DeleteUser(Guid userId)
    {
        await _userDal.DeleteUser(userId);
    }
}