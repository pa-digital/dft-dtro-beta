namespace DfT.DTRO.DAL;

/// <summary>
/// User status data access layer
/// </summary>
public interface IUserStatusDal
{
    /// <summary>
    /// Get user statuses
    /// </summary>
    /// <returns>List of user statuses</returns>
    List<UserStatus> GetStatuses();
}