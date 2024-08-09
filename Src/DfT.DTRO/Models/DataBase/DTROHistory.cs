using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DfT.DTRO.Models.DataBase;

/// <summary>
/// Wrapper for D-TRO history.
/// </summary>
[DataContract]
public class DTROHistory
{
    /// <summary>
    /// ID of the D-TRO history.
    /// </summary>
    [Key]
    [SwaggerSchema(ReadOnly = true)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(TypeName = "uuid")]
    public Guid Id { get; set; }

    /// <summary>
    /// ID of the linked D-TRO
    /// </summary>
    [Column(TypeName = "uuid")]
    public Guid DtroId { get; set; }

    /// <summary>
    /// Schema identifier of the D-TRO history.
    /// </summary>
    [Required(ErrorMessage = "schemaVersion field must be included")]
    [DataMember(Name = "schemaVersion")]
    [JsonConverter(typeof(SchemaVersionJsonConverter))]
    public SchemaVersion SchemaVersion { get; set; }

    /// <summary>
    /// Timestamp that represents the date and time this document was created.
    /// </summary>
    [DataMember(Name = "created")]
    [SaveOnce]
    [SwaggerSchema(ReadOnly = true)]
    public DateTime? Created { get; set; }

    /// <summary>
    /// Timestamp that represents the last date and time this document was updated. 
    /// </summary>
    [DataMember(Name = "lastUpdated")]
    [SwaggerSchema(ReadOnly = true)]
    public DateTime? LastUpdated { get; set; }

    /// <summary>
    /// Flag representing a deletion of the D-TRO history.
    /// </summary>
    [DataMember(Name = "deleted")]
    [SwaggerSchema(ReadOnly = true)]
    public bool Deleted { get; set; }

    /// <summary>
    /// Timestamp representing the date and time when the D-TRO history was deleted.
    /// </summary>
    [DataMember(Name = "deletionTime")]
    [SwaggerSchema(ReadOnly = true)]
    public DateTime? DeletionTime { get; set; }

    /// <summary>
    /// D-TRO history data model being submitted.
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