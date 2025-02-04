using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DfT.DTRO.Models.DataBase;

/// <summary>
/// Wrapper for digital traffic regulation order submission.
/// </summary>
[DataContract]
public class DigitalTrafficRegulationOrder : BaseEntity
{

    /// <summary>
    /// Schema identifier of the digital traffic regulation order data payload being submitted.
    /// </summary>
    /// <example>3.2.1</example>
    [Required(ErrorMessage = "schemaVersion field must be included")]
    [DataMember(Name = "schemaVersion")]
    [JsonConverter(typeof(SchemaVersionJsonConverter))]
    public SchemaVersion SchemaVersion { get; set; }

    /// <summary>
    /// Earliest of regulation start date and time.
    /// </summary>
    public DateTime? RegulationStart { get; set; }

    /// <summary>
    /// Latest of regulation end date and time.
    /// </summary>
    public DateTime? RegulationEnd { get; set; }

    /// <summary>
    /// ID of the traffic regulation authority creating this document.
    /// </summary>
    [DataMember(Name = "traCreator")]
    [Column("TraCreator")]
    public int TrafficAuthorityCreatorId { get; set; }

    /// <summary>
    /// ID of the traffic regulation authority owning this document.
    /// </summary>
    [DataMember(Name = "currentTraOwner")]
    [Column("CurrentTraOwner")]
    public int TrafficAuthorityOwnerId { get; set; }

    /// <summary>
    /// Descriptive name of the digital traffic regulation order document.
    /// </summary>
    public string TroName { get; set; }

    /// <summary>
    /// Create correlation unique identifier of the request with which this digital traffic regulation order document was created.
    /// </summary>
    [DataMember(Name = "createdCorrelationId")]
    [SaveOnce]
    [SwaggerSchema(ReadOnly = true)]
    public string CreatedCorrelationId { get; set; }

    /// <summary>
    /// Last updated correlation unique identifier, which digital traffic regulation order document was updated by.
    /// </summary>
    [DataMember(Name = "lastUpdatedCorrelationId")]
    [SwaggerSchema(ReadOnly = true)]
    public string LastUpdatedCorrelationId { get; set; }

    /// <summary>
    /// Flag representing a deletion of the digital traffic regulation order.
    /// </summary>
    [DataMember(Name = "deleted")]
    [SwaggerSchema(ReadOnly = true)]
    public bool Deleted { get; set; }

    /// <summary>
    /// Timestamp representing the date and time when the digital traffic regulation order was deleted.
    /// </summary>
    [DataMember(Name = "deletionTime")]
    [SwaggerSchema(ReadOnly = true)]
    public DateTime? DeletionTime { get; set; }

    /// <summary>
    /// Digital traffic regulation order data model being submitted.
    /// </summary>
    [Required(ErrorMessage = "data field must be included")]
    [DataMember(Name = "data")]
    [JsonConverter(typeof(ExpandoObjectConverter))]
    public ExpandoObject Data { get; set; }

    /// <summary>
    /// Unique regulation types that this digital traffic regulation order applies to.
    /// </summary>
    public List<string> RegulationTypes { get; set; }

    /// <summary>
    /// Unique vehicle types that this digital traffic regulation order applies to.
    /// </summary>
    public List<string> VehicleTypes { get; set; }

    /// <summary>
    /// Unique order reporting points that this digital traffic regulation order applies to.
    /// </summary>
    public List<string> OrderReportingPoints { get; set; }

    /// <summary>
    /// Unique regulated place types this digital traffic regulation order applies to.
    /// </summary>
    public List<string> RegulatedPlaceTypes { get; set; }

    /// <summary>
    /// The bounding box containing all points for this digital traffic regulation order regulated places.
    /// </summary>
    public BoundingBox Location { get; set; }

    /// <summary>
    /// Application related to the digital traffic regulation order
    /// </summary>
    public Application Application { get; set; }
}