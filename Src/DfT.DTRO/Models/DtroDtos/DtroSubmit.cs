using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DfT.DTRO.Models.DtroDtos;

/// <summary>
/// D-TRO data request.
/// </summary>
[DataContract]
public class DtroSubmit
{
    /// <summary>
    /// Schema version of the D-TRO data payload request.
    /// </summary>
    [Required(ErrorMessage = "schemaVersion field must be included")]
    [DataMember(Name = "schemaVersion")]
    [JsonConverter(typeof(SchemaVersionJsonConverter))]
    public SchemaVersion SchemaVersion { get; set; }

    /// <summary>
    /// D-TRO data model request.
    /// </summary>
    [Required(ErrorMessage = "data field must be included")]
    [DataMember(Name = "data")]
    [JsonConverter(typeof(ExpandoObjectConverter))]
    public ExpandoObject Data { get; set; }
}