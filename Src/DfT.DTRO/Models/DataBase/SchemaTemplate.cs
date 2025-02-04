using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DfT.DTRO.Models.DataBase;

/// <summary>
/// Wrapper for a Schema.
/// </summary>
[DataContract]
public class SchemaTemplate : BaseEntity
{
    /// <summary>
    /// Schema identifier of the schema data payload being submitted.
    /// </summary>
    [Required(ErrorMessage = "schemaVersion field must be included")]
    [DataMember(Name = "schemaVersion")]
    [JsonConverter(typeof(SchemaVersionJsonConverter))]
    public SchemaVersion SchemaVersion { get; set; }

    /// <summary>
    /// Correlation ID of the request with which this Schema document was created.
    /// </summary>
    [DataMember(Name = "createdCorrelationId")]
    [SaveOnce]
    [SwaggerSchema(ReadOnly = true)]
    public string CreatedCorrelationId { get; set; }

    /// <summary>
    /// Correlation ID of the request witch Schema document was updated.
    /// </summary>
    [DataMember(Name = "lastUpdatedCorrelationId")]
    [SwaggerSchema(ReadOnly = true)]
    public string LastUpdatedCorrelationId { get; set; }

    /// <summary>
    /// The schema data model being submitted;
    /// </summary>
    [Required(ErrorMessage = "Template field must be included")]
    [DataMember(Name = "template")]
    [JsonConverter(typeof(ExpandoObjectConverter))]
    public ExpandoObject Template { get; set; }

    /// <summary>
    /// Status of this document
    /// </summary>
    [DataMember(Name = "isActive")]
    [SwaggerSchema(ReadOnly = true)]
    public bool IsActive { get; set; }

    /// <summary>
    /// Application related to the Schema Template
    /// </summary>
    public Application Application { get; set; }
}