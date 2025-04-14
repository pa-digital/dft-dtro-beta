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
        string apiKey = _secretManagerClient.GetSecret(ApiConsts.NotifyApiKey);
        _notificationClient = new NotificationClient(apiKey);
    }

    /// <inheritdoc cref="IEmailService"/>
    public EmailNotificationResponse SendEmail(string name, string requestEmail, string status)
    {
        EmailNotificationResponse emailNotificationResponse = new();
        TemplateResponse templateResponse = new();
        string templateId;
        switch (status)
        {
            case "Inactive":
                templateId = _secretManagerClient.GetSecret(ApiConsts.ApplicationCreated);
                templateResponse = _notificationClient.GetTemplateById(templateId);
                break;
            case "Active":
                templateId = _secretManagerClient.GetSecret(ApiConsts.ApplicationPendingApproval);
                templateResponse = _notificationClient.GetTemplateById(templateId);
                break;
        }
        var personalisation = new Dictionary<string, dynamic>()
        {
            {"email address", requestEmail},
            {"application name", name},
            {"dtro-cso-email", _secretManagerClient.GetSecret(ApiConsts.DtroCsoEmail)}
        };

        emailNotificationResponse = _notificationClient.SendEmail(requestEmail, templateResponse.id, personalisation);
        return emailNotificationResponse;
    }

    /// <inheritdoc cref="IEmailService"/>
    public EmailNotificationResponse SendEmail(string requestEmail, ApigeeDeveloperApp apigeeDeveloperApp)
    {
        EmailNotificationResponse emailNotification = new();
        var templateId = _secretManagerClient.GetSecret(ApiConsts.RefreshToken);
        var templateResponse = _notificationClient.GetTemplateById(templateId);

        var personalisation = new Dictionary<string, dynamic>()
        {
            { "email address", requestEmail },
            { "application name", apigeeDeveloperApp.Name },
            { "consumer-key", apigeeDeveloperApp.Credentials.FirstOrDefault().ConsumerKey },
            { "consumer-secret", apigeeDeveloperApp.Credentials.FirstOrDefault().ConsumerSecret },
            { "dtro-cso-email", _secretManagerClient.GetSecret(ApiConsts.DtroCsoEmail) }
        };

        emailNotification = _notificationClient.SendEmail(requestEmail, templateResponse.id, personalisation);
        return emailNotification;
    }
}