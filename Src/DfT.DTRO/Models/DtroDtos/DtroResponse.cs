using DfT.DTRO.Converters;
using DfT.DTRO.Models.SchemaTemplate;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Dynamic;
using System.Runtime.Serialization;

namespace DfT.DTRO.Models.DtroDtos;

/// <summary>
/// Response DTO of the DTRO data.
/// </summary>
[DataContract]
public class DtroResponse
{
    /// <summary>
    /// The schema identifier of the DTRO data payload being submitted.
    /// </summary>
    /// <example>3.1.1.</example>
    [DataMember(Name = "schemaVersion")]
    [JsonConverter(typeof(SchemaVersionJsonConverter))]
    public SchemaVersion SchemaVersion { get; set; }

    /// <summary>
    /// The DTRO data model being submitted.
    /// </summary>
    [DataMember(Name = "data")]
    [JsonConverter(typeof(ExpandoObjectConverter))]
    public ExpandoObject Data { get; set; }
}
