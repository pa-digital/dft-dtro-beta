namespace DfT.DTRO.Extensions.DependencyInjection;

/// <summary>
/// Provides settings for email configuration.
/// </summary>
public static class EmailSettings
{
    /// <summary>
    /// Gets or sets the API key for the email service.
    /// </summary>
    public static string ApiKey { get; set; }

   
    /// <summary>
    /// Adds email settings to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the settings to.</param>
    public static void AddEmailSettings(this IServiceCollection services) => 
        ApiKey = Environment.GetEnvironmentVariable("EMAIL_API_KEY");
}