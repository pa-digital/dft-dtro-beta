using Newtonsoft.Json;

namespace DfT.DTRO.Models.RuleTemplate;

[DataContract]
public class RuleTemplateOverview
{
    [DataMember(Name = "schemaVersion")]
    [JsonConverter(typeof(SchemaVersionJsonConverter))]
    public SchemaVersion SchemaVersion { get; set; }
}
