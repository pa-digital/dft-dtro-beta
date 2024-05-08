using DfT.DTRO.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Dynamic;
using System.Runtime.Serialization;

namespace DfT.DTRO.Models.SchemaTemplate;

/// <summary>
/// Response dto of the SchemaDefinition data.
/// </summary>
[DataContract]
public class SchemaTemplateResponse
{
    /// <summary>
    /// Gets or sets the schema identifier of the SchemaTemplate payload being submitted.
    /// </summary>
    /// <example>3.1.1.</example>
    [DataMember(Name = "schemaVersion")]
    [JsonConverter(typeof(SchemaVersionJsonConverter))]
    public SchemaVersion SchemaVersion { get; set; }

    /// <summary>
    /// Gets or sets the template model being submitted.
    /// </summary>
    [DataMember(Name = "template")]
    [JsonConverter(typeof(ExpandoObjectConverter))]
    public ExpandoObject Template { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether indicates whether the template is active.
    /// </summary>
    [DataMember(Name = "isActive")]
    public bool IsActive { get; set; }
}
