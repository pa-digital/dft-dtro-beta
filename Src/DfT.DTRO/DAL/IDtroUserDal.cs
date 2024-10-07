namespace DfT.DTRO.DAL;

/// <summary>
/// Service interface providing methods that storage DtroUsers.
/// </summary>
public interface IDtroUserDal
{
    /// <summary>
    /// Get all Dtro Users based on their IDs.
    /// </summary>
    /// <param name="ids">List of Dtro users.</param>
    /// <returns> A <see cref="Task"/> representing the asynchronous get operation.</returns>
    Task<List<DtroUserResponse>> GetAllDtroUsersAsync(List<Guid> ids);

    /// <summary>
    /// Get all DtroUser.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous get operation.</returns>
    Task<List<DtroUserResponse>> GetAllDtroUsersAsync();

    /// <summary>
    /// Find existing Dtro Users by <paramref name="partialName"/>
    /// </summary>
    /// <param name="partialName">Partial name of the DtroUser.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous search operation.</returns>
    Task<List<DtroUserResponse>> SearchDtroUsersAsync(string partialName);

    /// <summary>
    /// Get DtroUser by <paramref name="id"/>
    /// </summary>
    /// <param name="id">DtroUser ID.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous get operation.</returns>
    Task<DtroUserResponse> GetDtroUserResponseAsync(Guid id);

    /// <summary>
    /// Get DtroUser by <paramref name="userId"/>
    /// </summary>
    /// <param name="userId">User ID.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous get operation.</returns>
    Task<DtroUserResponse> GetDtroUserByIdAsync(Guid userId);

    /// <summary>
    /// Get DtroUser by <paramref name="traId"/>
    /// </summary>
    /// <param name="traId">Traffic authority regulation ID.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous get operation.</returns>
    Task<DtroUser> GetDtroUserByTraIdAsync(int traId);

    /// <summary>
    /// Get DtroUser by <paramref name="appId"/>
    /// </summary>
    /// <param name="appId">the x-app-id.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous get operation.</returns>
    Task<DtroUser> GetDtroUserOnAppIdAsync(Guid appId);


    Task<bool> AnyAdminUserExistsAsync();
    /// <summary>
    /// Save DtroUser by <paramref name="dtroUserRequest"/>
    /// </summary>
    /// <param name="dtroUserRequest">DtroUser request.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous save operation.</returns>
    Task<GuidResponse> SaveDtroUserAsync(DtroUserRequest dtroUserRequest);

    /// <summary>
    /// Check if traffic regulation authority exists by <paramref name="traId"/>
    /// </summary>
    /// <param name="traId">Traffic regulation authority ID.</param>
    /// <returns>
    /// A <see cref="Task"/> whose result is <see langword="true" /> if traffic
    /// regulation authority with specified ID exists. Otherwise <see langword="false" />
    /// </returns>
    Task<bool> TraExistsAsync(int traId);

    /// <summary>
    /// Check if DtroUser exists by <paramref name="guid"/>
    /// </summary>
    /// <param name="guid">user ID.</param>
    /// <returns>
    /// A <see cref="Task"/> whose result is <see langword="true" /> if user with specified ID exists. Otherwise <see langword="false" />
    /// </returns>
    Task<bool> DtroUserExistsAsync(Guid guid);

    /// <summary>
    /// Update traffic regulation authority by <paramref name="dtroUserRequest"/>
    /// </summary>
    /// <param name="dtroUserRequest">DtroUser request.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous update operation.</returns>
    Task<GuidResponse> UpdateDtroUserAsync(DtroUserRequest dtroUserRequest);

    /// <summary>
    /// Delete Dtro users
    /// </summary>
    /// <param name="users">List of IDs of the users to be deleted.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous delete operation.</returns>
    Task<bool> DeleteDtroUsersAsync(List<DtroUser> users);
}