using DfT.DTRO.Converters;
using DfT.DTRO.Models.SchemaTemplate;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Runtime.Serialization;

namespace DfT.DTRO.Models.DtroDtos;

/// <summary>
/// Wrapper for a DTRO submission.
/// </summary>
[DataContract]
public class DtroSubmit
{
    /// <summary>
    /// The schema identifier of the DTRO data payload being submitted.
    /// </summary>
    /// <example>3.1.1.</example>
    [Required(ErrorMessage = "schemaVersion field must be included")]
    [DataMember(Name = "schemaVersion")]
    [JsonConverter(typeof(SchemaVersionJsonConverter))]
    public SchemaVersion SchemaVersion { get; set; }

    /// <summary>
    /// The DTRO data model being submitted.
    /// </summary>
    [Required(ErrorMessage = "data field must be included")]
    [DataMember(Name = "data")]
    [JsonConverter(typeof(ExpandoObjectConverter))]
    public ExpandoObject Data { get; set; }
}