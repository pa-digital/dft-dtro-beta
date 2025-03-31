namespace DfT.DTRO.Services;

public class UserService : IUserService
{
    private readonly IUserDal _userDal;
    private readonly IApigeeDeveloperRepository _apigeeDeveloperRepository;

    public UserService(IUserDal userDal, IApigeeDeveloperRepository apigeeDeveloperRepository)
    {
        _userDal = userDal;
        _apigeeDeveloperRepository = apigeeDeveloperRepository;
    }
    
    public async Task<PaginatedResponse<UserListDto>> GetUsers(PaginatedRequest paginatedRequest)
    {
        var paginatedResult = await _userDal.GetUsers(paginatedRequest);
        return new(paginatedResult.Results.ToList().AsReadOnly(), paginatedRequest.Page, paginatedResult.TotalCount);
    }

    public async Task DeleteUser(string email, Guid userId)
    {
        await _userDal.DeleteUser(userId);
        await _apigeeDeveloperRepository.DeleteDeveloper(email);
    }
}