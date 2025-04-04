namespace DfT.DTRO.Models.Applications;

/// <summary>
/// Represents an Apigee App.
/// </summary>
[DataContract]
public class ApplicationResponse
{
    /// <summary>
    /// Unique identifier for the app.
    /// </summary>
    [DataMember(Name = "appId")]
    public Guid AppId { get; set; }

    /// <summary>
    /// The timestamp when the app was created.
    /// </summary>
    [DataMember(Name = "createdAt")]
    public long CreatedAt { get; set; }

    /// <summary>
    /// List of credentials associated with the app.
    /// </summary>
    [DataMember(Name = "credentials")]
    public List<AppCredential> Credentials { get; set; }

    /// <summary>
    /// Unique identifier for the developer who owns the app.
    /// </summary>
    [DataMember(Name = "developerId")]
    public string DeveloperId { get; set; }

    /// <summary>
    /// The timestamp when the app was last modified.
    /// </summary>
    [DataMember(Name = "lastModifiedAt")]
    public long LastModifiedAt { get; set; }

    /// <summary>
    /// The name of the app.
    /// </summary>
    [DataMember(Name = "name")]
    public string Name { get; set; }

    /// <summary>
    /// The current status of the app (e.g., "approved").
    /// </summary>
    [DataMember(Name = "status")]
    public string Status { get; set; }
    
    /// <summary>
    /// The purpose of the app.
    /// </summary>
    [DataMember(Name = "purpose")]
    public string Purpose { get; set; }
    
    /// <summary>
    /// The SWA code of the app.
    /// </summary>
    [DataMember(Name = "swaCode")]
    public int SwaCode { get; set; }
}