namespace DfT.DTRO.Models.DataBase;

/// <summary>
/// Wrapper for User table
/// </summary>
[ExcludeFromCodeCoverage]
[DataContract]
public class User
{
    /// <summary>
    /// User unique identifier.
    /// </summary>
    [Key]
    [SwaggerSchema(ReadOnly = true)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(TypeName = "uuid")]
    public Guid Id { get; set; }

    /// <summary>
    /// User created date
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    /// User last updated date
    /// </summary>
    public DateTime? LastUpdated { get; set; }

    /// <summary>
    /// User forename
    /// </summary>
    public string Forename { get; set; }

    /// <summary>
    /// User surname
    /// </summary>
    public string Surname { get; set; }

    /// <summary>
    /// User email
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Is user a Central Service Operator
    /// </summary>
    public bool IsCentralServiceOperator { get; set; }

    /// <summary>
    /// User Status relationship with User
    /// </summary>
    public UserStatus UserStatus { get; set; }

    public List<Application> Applications { get; set; }

    /// <summary>
    /// Foreign key to DigitalServiceProvider
    /// </summary>
    [ForeignKey("DigitalServiceProvider")]
    public Guid? DigitalServiceProviderId { get; set; }

    /// <summary>
    /// Navigation property for DigitalServiceProvider
    /// </summary>
    public DigitalServiceProvider DigitalServiceProvider { get; set; }
}