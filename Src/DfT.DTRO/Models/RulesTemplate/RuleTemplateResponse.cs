using Newtonsoft.Json;

namespace DfT.DTRO.Models.RuleTemplate;

[DataContract]
public class RuleTemplateResponse
{
    [DataMember(Name = "schemaVersion")]
    [JsonConverter(typeof(SchemaVersionJsonConverter))]
    public SchemaVersion SchemaVersion { get; set; }

    [DataMember(Name = "template")]
    public string Template { get; set; }
}
