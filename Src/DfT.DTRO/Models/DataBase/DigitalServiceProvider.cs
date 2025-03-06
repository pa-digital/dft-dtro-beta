namespace DfT.DTRO.Models.DataBase;

/// <summary>
/// Wrapper for Digital Service Provider
/// </summary>
[ExcludeFromCodeCoverage]
[DataContract]
public class DigitalServiceProvider
{
    /// <summary>
    /// Digital Service Provider unique identifier.
    /// </summary>
    [Key]
    [SwaggerSchema(ReadOnly = true)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(TypeName = "uuid")]
    public Guid Id { get; set; }

    /// <summary>
    /// Digital Service Provider name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Digital Service Provider created date
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    /// Digital Service Provider last updated date
    /// </summary>
    public DateTime? LastUpdated { get; set; }

    /// <summary>
    /// Digital Service Provider status
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// Digital Service Provider users
    /// </summary>
    public List<User> Users { get; set; }


    /// <summary>
    /// Digital Service Provider applications
    /// </summary>
    public List<Application> Applications { get; set; }

    /// <summary>
    /// Traffic Regulation Authority relationship with Digital Service Provider
    /// </summary>
    public List<TrafficRegulationAuthorityDigitalServiceProvider> TrafficRegulationAuthorityDigitalServiceProviders { get; set; }
}