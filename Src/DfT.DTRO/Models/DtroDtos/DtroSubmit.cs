using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Runtime.Serialization;
using DfT.DTRO.Converters;
using DfT.DTRO.Models.SchemaTemplate;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DfT.DTRO.Models.DtroDtos;

[DataContract]
public class DtroSubmit
{
    [Required(ErrorMessage = "schemaVersion field must be included")]
    [DataMember(Name = "schemaVersion")]
    [JsonConverter(typeof(SchemaVersionJsonConverter))]
    public SchemaVersion SchemaVersion { get; set; }

    [Required(ErrorMessage = "data field must be included")]
    [DataMember(Name = "data")]
    [JsonConverter(typeof(ExpandoObjectConverter))]
    public ExpandoObject Data { get; set; }
}