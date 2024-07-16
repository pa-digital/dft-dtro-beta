using System.Runtime.Serialization;
using DfT.DTRO.Converters;
using DfT.DTRO.Models.SchemaTemplate;
using Newtonsoft.Json;

namespace DfT.DTRO.Models.RuleTemplate;

[DataContract]
public class RuleTemplateOverview
{
    [DataMember(Name = "schemaVersion")]
    [JsonConverter(typeof(SchemaVersionJsonConverter))]
    public SchemaVersion SchemaVersion { get; set; }
}
