namespace DfT.DTRO.Services;

/// <summary>
/// Interface for application services.
/// </summary>
public interface IApplicationService
{
    /// <summary>
    /// Creates a new application.
    /// </summary>
    /// <param name="email">The email of the user creating the application.</param>
    /// <param name="appInput">The input data for the application.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created application.</returns>
    Task<App> CreateApplication(string email, AppInput appInput);

    /// <summary>
    /// Validates if the application belongs to the user.
    /// </summary>
    /// <param name="email">The email of the user.</param>
    /// <param name="appId">The ID of the application.</param>
    /// <returns>A task that represents the asynchronous operation. The task result indicates whether the application belongs to the user.</returns>
    Task<bool> ValidateAppBelongsToUser(string email, Guid appId);

    /// <summary>
    /// Validates the application name.
    /// </summary>
    /// <param name="appName">The name of the application.</param>
    /// <returns>A task that represents the asynchronous operation. The task result indicates whether the application name is valid.</returns>
    Task<bool> ValidateApplicationName(string appName);

    /// <summary>
    /// Gets the application details.
    /// </summary>
    /// <param name="email">The email of the user.</param>
    /// <param name="appId">The ID of the application.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the application details.</returns>
    Task<ApplicationResponse> GetApplication(string email, Guid appId);

    /// <summary>
    /// Gets the Apigee developer application.
    /// </summary>
    /// <param name="email">The email of the user.</param>
    /// <param name="name">The name of the Apigee developer application.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the Apigee developer application.</returns>
    Task<ApigeeDeveloperApp> GetApigeeDeveloperApp(string email, string name);

    /// <summary>
    /// Gets a paginated list of applications.
    /// </summary>
    /// <param name="email">The email of the user.</param>
    /// <param name="paginatedRequest">The pagination request details.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the paginated list of applications.</returns>
    Task<PaginatedResponse<ApplicationListDto>> GetApplications(string email, PaginatedRequest paginatedRequest);

    /// <summary>
    /// Gets a paginated list of inactive applications.
    /// </summary>
    /// <param name="paginatedRequest">The pagination request details.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the paginated list of inactive applications.</returns>
    Task<PaginatedResponse<ApplicationInactiveListDto>> GetInactiveApplications(PaginatedRequest paginatedRequest);

    /// <summary>
    /// Activates an application by its ID.
    /// </summary>
    /// <param name="email">The email of the user.</param>
    /// <param name="appId">The ID of the application.</param>
    /// <returns>A task that represents the asynchronous operation. The task result indicates whether the activation was successful.</returns>
    Task<bool> ActivateApplicationById(string email, Guid appId);
}
