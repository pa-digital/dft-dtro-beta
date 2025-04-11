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
    EmailNotificationResponse SendEmail(string name, string requestEmail, string status);

    /// <summary>
    /// Asynchronously sends an email for the specified application.
    /// </summary>
    /// <param name="apigeeDeveloperApp">Apigee developer application.</param>
    /// <param name="requestEmail">The application email to be sent.</param>
    /// <returns>An email response notification.</returns>
    EmailNotificationResponse SendEmail(ApigeeDeveloperApp apigeeDeveloperApp, string requestEmail);
}