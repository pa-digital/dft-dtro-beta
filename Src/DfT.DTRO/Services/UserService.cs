namespace DfT.DTRO.Services;

public class UserService : IUserService
{
    private readonly IUserDal _userDal;
    private readonly IApplicationDal _applicationDal;
    private readonly IApigeeDeveloperRepository _apigeeDeveloperRepository;

    public UserService(IUserDal userDal, IApplicationDal applicationDal, IApigeeDeveloperRepository apigeeDeveloperRepository)
    {
        _userDal = userDal;
        _applicationDal = applicationDal;
        _apigeeDeveloperRepository = apigeeDeveloperRepository;
    }
    
    public async Task<PaginatedResponse<UserListDto>> GetUsers(PaginatedRequest paginatedRequest)
    {
        var paginatedResult = await _userDal.GetUsers(paginatedRequest);
        return new(paginatedResult.Results.ToList().AsReadOnly(), paginatedRequest.Page, paginatedResult.TotalCount);
    }

    public async Task<UserDto> GetUserDetails(Guid userId)
    {
        User user = await _userDal.GetUserFromId(userId);
        List<ApplicationListDto> applications = await _applicationDal.GetUserApplications(userId);

        return new UserDto {
            Id = user.Id,
            Name = user.Forename + " " + user.Surname,
            Email = user.Email,
            Created = user.Created,
            Applications = applications
        };
    }

    public async Task DeleteUser(string email, Guid userId)
    {
        await _userDal.DeleteUser(userId);
        await _apigeeDeveloperRepository.DeleteDeveloper(email);
    }
}