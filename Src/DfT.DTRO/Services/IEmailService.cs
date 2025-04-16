namespace DfT.DTRO.Services;

/// <summary>
/// Service for handling email-related operations.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Asynchronously sends an email for the specified application.
    /// </summary>
    /// <param name="name">The application name to be sent.</param>
    /// <param name="requestEmail">The application email to be sent.</param>
    /// <param name="status">Status of the application.</param>
    /// <returns>An email response notification.</returns>
    EmailNotificationResponse NotifyUserWhenApplicationCreatedOrApproved(string name, string requestEmail, string status);

    /// <summary>
    /// Asynchronously sends an email for the specified application.
    /// </summary>
    /// <param name="requestEmail">The application email to be sent.</param>
    /// <param name="apigeeDeveloperApp">Apigee developer application.</param>
    /// <returns>An email response notification.</returns>
    EmailNotificationResponse NotifyUserWhenSecretsRefreshes(string requestEmail,ApigeeDeveloperApp apigeeDeveloperApp);

    /// <summary>
    /// Asynchronously sends an email to the CSO when an application is created.
    /// </summary>
    /// <param name="username">The username creating the application</param>
    /// <param name="csoEmail">The D-TRO CSO email address</param>
    /// <returns>An email response notification.</returns>
    EmailNotificationResponse NotifyCSOWhenApplicationCreated(string username, string csoEmail);
}