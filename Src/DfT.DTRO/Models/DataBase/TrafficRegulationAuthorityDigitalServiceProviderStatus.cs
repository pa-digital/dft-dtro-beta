namespace DfT.DTRO.Models.DataBase;

/// <summary>
/// Wrapper for Traffic Regulation Authority Digital Service Provider Status
/// </summary>
[ExcludeFromCodeCoverage]
[DataContract]
public class TrafficRegulationAuthorityDigitalServiceProviderStatus
{
    /// <summary>
    /// Traffic Regulation Authority relationship Digital Service Provider Status unique identifier.
    /// </summary>
    [Key]
    [SwaggerSchema(ReadOnly = true)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(TypeName = "uuid")]
    public Guid Id { get; set; }

    /// <summary>
    /// Traffic Regulation Authority relationship Digital Service Provider status
    /// </summary>
    public TrafficRegulationAuthorityDigitalServiceProvider TrafficRegulationAuthorityDigitalServiceProvider { get; set; }
}