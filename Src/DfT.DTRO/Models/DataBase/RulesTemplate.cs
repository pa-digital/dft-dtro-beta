using Newtonsoft.Json;

namespace DfT.DTRO.Models.DataBase;

/// <summary>
/// Wrapper for a Rule.
/// </summary>
[DataContract]
public class RuleTemplate : BaseEntity
{
    /// <summary>
    /// The schema identifier of the rule data payload being submitted.
    /// </summary>
    /// <example>3.2.1.</example>
    [Required(ErrorMessage = "SchemaVersion field must be included")]
    [DataMember(Name = "SchemaVersion")]
    [JsonConverter(typeof(SchemaVersionJsonConverter))]
    public SchemaVersion SchemaVersion { get; set; }

    /// <summary>
    /// Correlation ID of the request with which this DigitalTrafficRegulationOrder was created.
    /// </summary>
    [DataMember(Name = "createdCorrelationId")]
    [SaveOnce]
    [SwaggerSchema(ReadOnly = true)]
    public string CreatedCorrelationId { get; set; }

    /// <summary>
    /// Correlation ID of the request with which this DigitalTrafficRegulationOrder was last updated.
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

    /// <summary>
    /// Application related to the Rule Template
    /// </summary>
    public Application Application { get; set; }
}