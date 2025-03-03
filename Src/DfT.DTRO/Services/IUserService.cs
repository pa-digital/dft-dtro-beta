namespace DfT.DTRO.Services;

public interface IUserService
{
    PaginatedResponse<UserDto> GetUsers(string userId);
}