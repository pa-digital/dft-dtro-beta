using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DfT.DTRO.Models.SystemConfig;

[DataContract]
public class SystemConfigResponse
{
    [DataMember(Name = "systemName")]
    public string SystemName { get; set; }

    [DataMember(Name = "CurrentUserName")]
    public string CurrentUserName { get; set; }

    [DataMember(Name = "isTest")]
    public bool IsTest { get; set; }
}
