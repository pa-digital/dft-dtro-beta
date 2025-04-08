using DfT.DTRO.Apis.Consts;

namespace DfT.DTRO.Services;

/// <inheritdoc cref="IEmailService"/>
public class EmailService(ISecretManagerClient client) : IEmailService
{
    /// <inheritdoc cref="IEmailService"/>
    public async Task SendAsync(App app, string requestEmail)
    {
        var apiKey = client.GetSecret(ApiConsts.SendGridApiKey);
        var emailOriginator = client.GetSecret(ApiConsts.EmailOriginator);
        var dtroCsoEmail = client.GetSecret(ApiConsts.DtroCsoEmail);

        try
        {
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(emailOriginator, "DfT Central Service Operator");
            var to = new EmailAddress(requestEmail, "Application created");
            var subject = $"Approval for {app.Name}";
            var body = $"Your request for '{app.Name}' app has been approved. " +
                       "Please log in to the D-TRO portal to view your app credentials. " +
                       $"If you need assistance please email '{dtroCsoEmail}'. " +
                       "Do not reply to this email.";
            var email = MailHelper.CreateSingleEmail(from, to, subject, body, string.Empty);
            var response = await client.SendEmailAsync(email, CancellationToken.None);
            if (!response.IsSuccessStatusCode)
            {
                throw new EmailSendException();
            }
        }
        catch (EmailSendException esex)
        {
            throw esex;
        }
    }

}