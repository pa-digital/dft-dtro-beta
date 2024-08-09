using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DfT.DTRO.Models.DataBase;

/// <summary>
/// Wrapper for a Schema.
/// </summary>
[DataContract]
public class SchemaTemplate
{
    /// <summary>
    /// ID of a schema.
    /// </summary>
    [Key]
    [SwaggerSchema(ReadOnly = true)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(TypeName = "uuid")]
    public Guid Id { get; set; }

    /// <summary>
    /// Schema identifier of the schema data payload being submitted.
    /// </summary>
    [Required(ErrorMessage = "schemaVersion field must be included")]
    [DataMember(Name = "schemaVersion")]
    [JsonConverter(typeof(SchemaVersionJsonConverter))]
    public SchemaVersion SchemaVersion { get; set; }

    /// <summary>
    /// Timestamp representing the date and time of this document creation.
    /// </summary>
    [DataMember(Name = "created")]
    [SaveOnce]
    [SwaggerSchema(ReadOnly = true)]
    public DateTime? Created { get; set; }

    /// <summary>
    /// Timestamp representing the date and time when this document was last updated.
    /// </summary>
    [DataMember(Name = "lastUpdated")]
    [SwaggerSchema(ReadOnly = true)]
    public DateTime? LastUpdated { get; set; }

    /// <summary>
    /// Correlation ID of the request with which this Schema document was created.
    /// </summary>
    [DataMember(Name = "createdCorrelationId")]
    [SaveOnce]
    [SwaggerSchema(ReadOnly = true)]
    public string CreatedCorrelationId { get; set; }

    /// <summary>
    /// Correlation ID of the request witch which Schema document was updated.
    /// </summary>
    [DataMember(Name = "lastUpdatedCorrelationId")]
    [SwaggerSchema(ReadOnly = true)]
    public string LastUpdatedCorrelationId { get; set; }

    /// <summary>
    /// Flag representing whether the Schema is available.
    /// </summary>
    [DataMember(Name = "isActive")]
    [SwaggerSchema(ReadOnly = true)]
    public bool IsActive { get; set; }

    /// <summary>
    /// The schema data model being submitted;
    /// </summary>
    [Required(ErrorMessage = "Template field must be included")]
    [DataMember(Name = "template")]
    [JsonConverter(typeof(ExpandoObjectConverter))]
    public ExpandoObject Template { get; set; }
}