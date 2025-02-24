using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DfT.DTRO.Models.DataBase;

/// <summary>
/// Wrapper for D-TRO submission.
/// </summary>
[DataContract]
public class DTRO
{
    /// <summary>
    /// ID of the D-TRO
    /// </summary>
    [Key]
    [SwaggerSchema(ReadOnly = true)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(TypeName = "uuid")]
    public Guid Id { get; set; }

    /// <summary>
    /// Schema identifier of the D-TRO data payload being submitted.
    /// </summary>
    /// <example>3.2.1</example>
    [Required(ErrorMessage = "schemaVersion field must be included")]
    [DataMember(Name = "schemaVersion")]
    [JsonConverter(typeof(SchemaVersionJsonConverter))]
    public SchemaVersion SchemaVersion { get; set; }

    /// <summary>
    /// Timestamp representing the date and time this document was created.
    /// </summary>
    [DataMember(Name = "created")]
    [SaveOnce]
    [SwaggerSchema(ReadOnly = true)]
    public DateTime? Created { get; set; }

    /// <summary>
    /// Timestamp representing the last date and time this document was updated.
    /// </summary>
    [DataMember(Name = "lastUpdated")]
    [SwaggerSchema(ReadOnly = true)]
    public DateTime? LastUpdated { get; set; }

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
    /// Descriptive name of the D-TRO document.
    /// </summary>
    public string TroName { get; set; }

    /// <summary>
    /// Correlation ID of the request with which this D-TRO document was created.
    /// </summary>
    [DataMember(Name = "createdCorrelationId")]
    [SaveOnce]
    [SwaggerSchema(ReadOnly = true)]
    public string CreatedCorrelationId { get; set; }

    /// <summary>
    /// Request Correlation ID which D-TRO document was updated by.
    /// </summary>
    [DataMember(Name = "lastUpdatedCorrelationId")]
    [SwaggerSchema(ReadOnly = true)]
    public string LastUpdatedCorrelationId { get; set; }

    /// <summary>
    /// Flag representing a deletion of the D-TRO.
    /// </summary>
    [DataMember(Name = "deleted")]
    [SwaggerSchema(ReadOnly = true)]
    public bool Deleted { get; set; } = false;

    /// <summary>
    /// Timestamp representing the date and time when the D-TRO was deleted.
    /// </summary>
    [DataMember(Name = "deletionTime")]
    [SwaggerSchema(ReadOnly = true)]
    public DateTime? DeletionTime { get; set; }

    /// <summary>
    /// D-TRO data model being submitted.
    /// </summary>
    [Required(ErrorMessage = "data field must be included")]
    [DataMember(Name = "data")]
    [JsonConverter(typeof(ExpandoObjectConverter))]
    public ExpandoObject Data { get; set; }

    /// <summary>
    /// Unique regulation types that this D-TRO applies to.
    /// </summary>
    public List<string> RegulationTypes { get; set; }

    /// <summary>
    /// Unique vehicle types that this D-TRO applies to.
    /// </summary>
    public List<string> VehicleTypes { get; set; }

    /// <summary>
    /// Unique order reporting points that this D-TRO applies to.
    /// </summary>
    public List<string> OrderReportingPoints { get; set; }

    /// <summary>
    /// Unique regulated place types this D-TRO applies to.
    /// </summary>
    public List<string> RegulatedPlaceTypes { get; set; }

    /// <summary>
    /// The bounding box containing all points for this D-TRO regulated places.
    /// </summary>
    public BoundingBox Location { get; set; }
}