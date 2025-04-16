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
    public EmailNotificationResponse NotifyUserWhenApplicationCreatedOrApproved(string name, string requestEmail, string status)
    {
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

        return _notificationClient.SendEmail(requestEmail, templateResponse.id, personalisation);
    }

    /// <inheritdoc cref="IEmailService"/>
    public EmailNotificationResponse NotifyUserWhenSecretsRefreshes(string requestEmail, ApigeeDeveloperApp apigeeDeveloperApp)
    {
        var templateId = _secretManagerClient.GetSecret(ApiConsts.RefreshToken);
        var templateResponse = _notificationClient.GetTemplateById(templateId);

        var personalisation = new Dictionary<string, dynamic>()
        {
            { "email address", requestEmail },
            { "application name", apigeeDeveloperApp.Name },
            { "dtro-cso-email", _secretManagerClient.GetSecret(ApiConsts.DtroCsoEmail) }
        };

        return _notificationClient.SendEmail(requestEmail, templateResponse.id, personalisation);
    }

    /// <inheritdoc cref="IEmailService"/>
    public EmailNotificationResponse NotifyCSOWhenApplicationCreated(string username, string csoEmail)
    {
        var noReplyEmailAddress = _secretManagerClient.GetSecret(ApiConsts.NoReplyEmail);
        var templateId = _secretManagerClient.GetSecret(ApiConsts.ApplicationCreatedCSONotified);
        var templateResponse = _notificationClient.GetTemplateById(templateId);
        var personalisation = new Dictionary<string, dynamic>()
        {
            { "email address", csoEmail },
            { "username", username }
        };

        return _notificationClient.SendEmail(csoEmail, templateResponse.id, personalisation);
    }
}