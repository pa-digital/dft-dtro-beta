namespace DfT.DTRO.DAL;

/// <summary>
/// User data access layer
/// </summary>
public interface IUserDal
{
    /// <summary>
    /// Get users
    /// </summary>
    /// <param name="request">User request parameters</param>
    /// <returns>User paginated result</returns>
    PaginatedResult<User> GetUsers(UserRequest request);
}