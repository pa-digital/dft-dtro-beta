
namespace DfT.DTRO.Models.SystemConfig;

[DataContract]
public class SystemConfig
{
    [DataMember(Name = "systemName")]
    public string SystemName { get; set; }

    [DataMember(Name = "isTest")]
    public bool IsTest { get; set; }
}
