using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DfT.DTRO.Models.DtroDtos;

/// <summary>
/// D-TRO data response.
/// </summary>
[DataContract]
public class DtroResponse
{
    /// <summary>
    /// D-TRO unique identifier.
    /// </summary>
    [DataMember(Name = "id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Schema version of the D-TRO data payload response.
    /// </summary>
    [DataMember(Name = "schemaVersion")]
    [JsonConverter(typeof(SchemaVersionJsonConverter))]
    public SchemaVersion SchemaVersion { get; set; }

    /// <summary>
    /// D-TRO data model response.
    /// </summary>
    [DataMember(Name = "data")]
    [JsonConverter(typeof(ExpandoObjectConverter))]
    public ExpandoObject Data { get; set; }
}
