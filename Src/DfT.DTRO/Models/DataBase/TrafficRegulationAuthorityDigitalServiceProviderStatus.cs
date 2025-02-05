namespace DfT.DTRO.Models.DataBase;

/// <summary>
/// Wrapper for status between Traffic Regulation Authority and Digital Service Provider.
/// </summary>
public class TrafficRegulationAuthorityDigitalServiceProviderStatus
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
    /// Status of relationship between Traffic Regulation Authority and Digital Service Provider.
    /// </summary>
    public string Status { get; set; }
}