using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DfT.DTRO.Models.SchemaTemplate;

[DataContract]
public class SchemaTemplateResponse
{
    [DataMember(Name = "schemaVersion")]
    [JsonConverter(typeof(SchemaVersionJsonConverter))]
    public SchemaVersion SchemaVersion { get; set; }

    [DataMember(Name = "template")]
    [JsonConverter(typeof(ExpandoObjectConverter))]
    public ExpandoObject Template { get; set; }

    [DataMember(Name = "isActive")]
    public bool IsActive { get; set; }
}
