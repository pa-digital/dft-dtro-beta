using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DfT.DTRO.Models.DataBase;

/// <summary>
/// Wrapper for digital traffic regulation order history.
/// </summary>
[DataContract]
public class DigitalTrafficRegulationOrderHistory : BaseEntity
{
    /// <summary>
    /// ID of the linked digital traffic regulation order
    /// </summary>
    [Column(TypeName = "uuid")]
    public Guid DigitalTrafficRegulationOrderId { get; set; }

    /// <summary>
    /// Schema identifier of the digital traffic regulation order history.
    /// </summary>
    [Required(ErrorMessage = "schemaVersion field must be included")]
    [DataMember(Name = "schemaVersion")]
    [JsonConverter(typeof(SchemaVersionJsonConverter))]
    public SchemaVersion SchemaVersion { get; set; }

    /// <summary>
    /// Flag representing a deletion of the digital traffic regulation order history.
    /// </summary>
    [DataMember(Name = "deleted")]
    [SwaggerSchema(ReadOnly = true)]
    public bool Deleted { get; set; }

    /// <summary>
    /// Timestamp representing the date and time when the digital traffic regulation order history was deleted.
    /// </summary>
    [DataMember(Name = "deletionTime")]
    [SwaggerSchema(ReadOnly = true)]
    public DateTime? DeletionTime { get; set; }

    /// <summary>
    /// Digital traffic regulation order history data model being submitted.
    /// </summary>
    [Required(ErrorMessage = "data field must be included")]
    [DataMember(Name = "data")]
    [JsonConverter(typeof(ExpandoObjectConverter))]
    public ExpandoObject Data { get; set; }

    /// <summary>
    /// ID of the traffic regulation authority creating this document.
    /// </summary>
    [Required(ErrorMessage = "TRA Creator field must be included")]
    [DataMember(Name = "traCreator")]
    [Column("TraCreator")]
    public int TrafficAuthorityCreatorId { get; set; }

    /// <summary>
    /// ID of the traffic regulation authority owning this document.
    /// </summary>
    [Required(ErrorMessage = "Current TRA owner field must be included")]
    [DataMember(Name = "currentTraOwner")]
    [Column("CurrentTraOwner")]
    public int TrafficAuthorityOwnerId { get; set; }
}