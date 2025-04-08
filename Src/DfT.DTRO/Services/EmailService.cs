namespace DfT.DTRO.Services;

/// <inheritdoc cref="IEmailService"/>
public class EmailService : IEmailService
{
    private readonly ISecretManagerClient _secretManagerClient;
    private readonly INotificationClient _notificationClient;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="secretManagerClient">secret manager client</param>
    public EmailService(ISecretManagerClient secretManagerClient)
    {
        _secretManagerClient = secretManagerClient;
        _notificationClient = new NotificationClient(_secretManagerClient.GetSecret(ApiConsts.NotifyApiKey));
    }

    /// <inheritdoc cref="IEmailService"/>
    public EmailNotificationResponse SendEmail(App app, string requestEmail)
    {
        //TODO: This template will be refactored once I will have more of the email tickets implemented.
        var templateResponse = _notificationClient.GetTemplateById(_secretManagerClient.GetSecret(ApiConsts.ApplicationPendingApproval));
        var personalisation = new Dictionary<string, dynamic>()
        {
            {"email address", requestEmail},
            {"application name", app.Name},
            {"dtro-cso-email", _secretManagerClient.GetSecret(ApiConsts.DtroCsoEmail)}
        };

        return _notificationClient.SendEmail(requestEmail, templateResponse.id, personalisation);
    }

}