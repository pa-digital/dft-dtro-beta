using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DfT.DTRO.Models.SystemConfig;

[DataContract]
public class SystemConfigRequest
{
    [DataMember(Name = "systemName")]
    public string SystemName { get; set; }

    [DataMember(Name = "isTest")]
    public bool IsTest { get; set; }
}
