namespace DfT.DTRO.Services;

/// <summary>
/// User service
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Get users
    /// </summary>
    /// <param name="request">User request parameters</param>
    /// <returns>Paginated response of users</returns>
    PaginatedResponse<UserDto> GetUsers(UserRequest request);
}