namespace DfT.DTRO.Services;

/// <summary>
/// Service for handling email-related operations.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Asynchronously sends an email for the specified application.
    /// </summary>
    /// <param name="name">The application name to be send.</param>
    /// <param name="requestEmail">The application email to be send.</param>
    /// <param name="status">Status of the application.</param>
    /// <returns>An email response notification.</returns>
    EmailNotificationResponse SendEmail(string name, string requestEmail, string status);

    /// <summary>
    /// Asynchronously sends an email for the specified application.
    /// </summary>
    /// <param name="requestEmail">The application email to be send.</param>
    /// <param name="apigeeDeveloperApp">Apigee developer application.</param>
    /// <returns>An email response notification.</returns>
    EmailNotificationResponse SendEmail(string requestEmail,ApigeeDeveloperApp apigeeDeveloperApp);

    /// <summary>
    /// Asynchronously sends an email for the specified application.
    /// </summary>
    /// <param name="name">The application name to be send.</param>
    /// <param name="requestEmail">The developer email to be send.</param>
    /// <returns>An email response notification.</returns>
    EmailNotificationResponse SendEmailForApplicationConfirmation(string name, string requestEmail);
}