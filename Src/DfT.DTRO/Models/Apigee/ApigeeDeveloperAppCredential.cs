namespace DfT.DTRO.Models.Apigee;

/// <summary>
/// Represents API credentials for an Apigee app.
/// </summary>
[DataContract]
public class ApigeeDeveloperAppCredential
{
    /// <summary>
    /// The consumer key for authentication.
    /// </summary>
    [DataMember(Name = "consumerKey")]
    public string ConsumerKey { get; set; }

    /// <summary>
    /// The consumer secret for authentication.
    /// </summary>
    [DataMember(Name = "consumerSecret")]
    public string ConsumerSecret { get; set; }

    /// <summary>
    /// Expiration timestamp of the credentials (-1 means no expiry).
    /// </summary>
    [DataMember(Name = "expiresAt")]
    public long ExpiresAt { get; set; }

    /// <summary>
    /// Issued timestamp of the credentials.
    /// </summary>
    [DataMember(Name = "issuedAt")]
    public long IssuedAt { get; set; }

    /// <summary>
    /// Status of the credentials (e.g., "approved").
    /// </summary>
    [DataMember(Name = "status")]
    public string Status { get; set; }
}