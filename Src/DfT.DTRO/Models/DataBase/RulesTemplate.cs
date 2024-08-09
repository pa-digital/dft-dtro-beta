using Newtonsoft.Json;

namespace DfT.DTRO.Models.DataBase;

/// <summary>
/// Wrapper for a Rule.
/// </summary>
[DataContract]
public class RuleTemplate
{
    /// <summary>
    /// ID of the Rule.
    /// </summary>
    [Key]
    [SwaggerSchema(ReadOnly = true)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(TypeName = "uuid")]
    public Guid Id { get; set; }

    /// <summary>
    /// The schema identifier of the rule data payload being submitted.
    /// </summary>
    /// <example>3.2.1.</example>
    [Required(ErrorMessage = "SchemaVersion field must be included")]
    [DataMember(Name = "SchemaVersion")]
    [JsonConverter(typeof(SchemaVersionJsonConverter))]
    public SchemaVersion SchemaVersion { get; set; }

    /// <summary>
    /// Timestamp that represents the creation time of this document.
    /// </summary>
    [DataMember(Name = "created")]
    [SaveOnce]
    [SwaggerSchema(ReadOnly = true)]
    public DateTime? Created { get; set; }

    /// <summary>
    /// Timestamp that represents the last time this document was updated.
    /// </summary>
    [DataMember(Name = "lastUpdated")]
    [SwaggerSchema(ReadOnly = true)]
    public DateTime? LastUpdated { get; set; }

    /// <summary>
    /// Correlation ID of the request with which this DTRO was created.
    /// </summary>
    [DataMember(Name = "createdCorrelationId")]
    [SaveOnce]
    [SwaggerSchema(ReadOnly = true)]
    public string CreatedCorrelationId { get; set; }

    /// <summary>
    /// Correlation ID of the request with which this DTRO was last updated.
    /// </summary>
    [DataMember(Name = "lastUpdatedCorrelationId")]
    [SwaggerSchema(ReadOnly = true)]
    public string LastUpdatedCorrelationId { get; set; }

    /// <summary>
    /// The Rule data model being submitted.
    /// </summary>
    [Required(ErrorMessage = "Template field must be included")]
    [DataMember(Name = "template")]
    public string Template { get; set; }
}