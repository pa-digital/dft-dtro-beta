namespace DfT.DTRO.Models.DataBase;

/// <summary>
/// Wrapper for Traffic Regulation Authority 
/// </summary>
[DataContract]
public class TrafficRegulationAuthority
{
    /// <summary>
    /// Traffic Regulation Authority unique identifier.
    /// </summary>
    [Key]
    [SwaggerSchema(ReadOnly = true)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(TypeName = "uuid")]
    public Guid Id { get; set; }

    /// <summary>
    /// Traffic Regulation Authority name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Traffic Regulation Authority created date
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    /// Traffic Regulation Authority last updated date
    /// </summary>
    public DateTime? LastUpdated { get; set; }

    /// <summary>
    /// Traffic Regulation Authority status
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// Traffic Regulation Authority applications
    /// </summary>
    public List<Application> Applications { get; set; }

    /// <summary>
    /// Traffic Regulation Authority relationship with Digital Service Provider
    /// </summary>
    public List<TrafficRegulationAuthorityDigitalServiceProvider> TrafficRegulationAuthorityDigitalServiceProviders { get; set; }
}