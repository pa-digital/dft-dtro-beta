namespace DfT.DTRO.Services;

/// <inheritdoc cref="IEmailService"/>
public class EmailService : IEmailService
{
    /// <inheritdoc cref="IEmailService"/>
    public async Task SendAsync(App app, string requestEmail)
    {
        try
        {
            var client = new SendGridClient(EmailSettings.ApiKey);
            var from = new EmailAddress("ben.payley@dft.gov.uk", "DfT Central Service Operator");
            var to = new EmailAddress(requestEmail, "Application created");
            var subject = $"Approval for {app.Name}";
            var body = $"Your request for '{app.Name}' app has been approved. " +
                       "Please log in to the D-TRO portal to view your app credentials. " +
                       "If you need assistance please email 'dtro-cso@dft.gov.uk'. " +
                       "Do not reply to this email.";
            var email = MailHelper.CreateSingleEmail(from, to, subject, body, string.Empty);
            await client.SendEmailAsync(email, CancellationToken.None);
        }
        catch (EmailSendException esex)
        {
            throw esex;
        }
    }
}