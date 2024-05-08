using DfT.DTRO.Converters;
using DfT.DTRO.Models.SchemaTemplate;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace DfT.DTRO.Models.RuleTemplate;

/// <summary>
/// Response dto of the RuleDefinition data.
/// </summary>
[DataContract]
public class RuleTemplateResponse
{
    /// <summary>
    /// Gets or sets the schema identifier of the RuleTemplate payload being submitted.
    /// </summary>
    /// <example>3.1.1.</example>
    [DataMember(Name = "schemaVersion")]
    [JsonConverter(typeof(SchemaVersionJsonConverter))]
    public SchemaVersion SchemaVersion { get; set; }

    /// <summary>
    /// Gets or sets the template model being submitted.
    /// </summary>
    [DataMember(Name = "template")]
    public string Template { get; set; }
}
