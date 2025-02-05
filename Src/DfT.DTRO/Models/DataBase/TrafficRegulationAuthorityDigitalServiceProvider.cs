namespace DfT.DTRO.Models.DataBase;

/// <summary>
/// Wrapper for relation between Traffic Regulation Authority and Digital Service Provider.
/// </summary>
public class TrafficRegulationAuthorityDigitalServiceProvider
{
    /// <summary>
    /// System unique identifier of this document 
    /// </summary>
    [Key]
    [SwaggerSchema(ReadOnly = true)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(TypeName = "uuid")]
    public Guid Id { get; set; }

    /// <summary>
    /// Traffic Regulation Authority unique identifier
    /// </summary>
    public Guid TrafficRegulationAuthorityId { get; set; }


    /// <summary>
    /// Digital Service Provider unique identifier
    /// </summary>
    public Guid DigitalServiceProviderId { get; set; }

    /// <summary>
    /// Traffic Regulation Authority and Digital Service Provider Status unique identifier
    /// </summary>
    public Guid TrafficRegulationAuthorityDigitalServiceProviderStatusId { get; set; }
}