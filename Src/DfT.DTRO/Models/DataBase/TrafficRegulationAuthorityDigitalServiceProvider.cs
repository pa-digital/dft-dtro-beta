namespace DfT.DTRO.Models.DataBase;

/// <summary>
/// Wrapper for Traffic Regulation Authority relationship with Digital Service Provider
/// </summary>
[ExcludeFromCodeCoverage]
[DataContract]
public class TrafficRegulationAuthorityDigitalServiceProvider
{
    /// <summary>
    /// Traffic Regulation Authority relationship with Digital Service Provider unique identifier.
    /// </summary>
    [Key]
    [SwaggerSchema(ReadOnly = true)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(TypeName = "uuid")]
    public Guid Id { get; set; }
}