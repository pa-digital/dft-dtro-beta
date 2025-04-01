namespace DfT.DTRO.Models.Apigee;

/// <summary>
/// Represents an Apigee Developer.
/// </summary>
[DataContract]
public class ApigeeDeveloper
{
    /// <summary>
    /// The developer's email address.
    /// </summary>
    [DataMember(Name = "email")]
    public string Email { get; set; }

    /// <summary>
    /// The developer's first name.
    /// </summary>
    [DataMember(Name = "firstName")]
    public string FirstName { get; set; }

    /// <summary>
    /// The developer's last name.
    /// </summary>
    [DataMember(Name = "lastName")]
    public string LastName { get; set; }

    /// <summary>
    /// The developer's username.
    /// </summary>
    [DataMember(Name = "userName")]
    public string UserName { get; set; }

    /// <summary>
    /// Unique identifier for the developer.
    /// </summary>
    [DataMember(Name = "developerId")]
    public string DeveloperId { get; set; }

    /// <summary>
    /// The name of the organization the developer belongs to.
    /// </summary>
    [DataMember(Name = "organizationName")]
    public string OrganizationName { get; set; }

    /// <summary>
    /// The current status of the developer (e.g., "active").
    /// </summary>
    [DataMember(Name = "status")]
    public string Status { get; set; }

    /// <summary>
    /// The timestamp when the developer was created.
    /// </summary>
    [DataMember(Name = "createdAt")]
    public long CreatedAt { get; set; }

    /// <summary>
    /// The timestamp when the developer was last modified.
    /// </summary>
    [DataMember(Name = "lastModifiedAt")]
    public long LastModifiedAt { get; set; }
}