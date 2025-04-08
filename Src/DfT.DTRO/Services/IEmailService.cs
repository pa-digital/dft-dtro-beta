using Notify.Models.Responses;

namespace DfT.DTRO.Services;

/// <summary>
/// Service for handling email-related operations.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Asynchronously sends an email for the specified application.
    /// </summary>
    /// <param name="app">The application name to be sent.</param>
    /// <param name="requestEmail">The application email to be sent.</param>
    /// <returns>An email response notification.</returns>
    EmailNotificationResponse SendAsync(App app, string requestEmail);
}