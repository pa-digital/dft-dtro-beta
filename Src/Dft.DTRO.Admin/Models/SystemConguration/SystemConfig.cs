
namespace DfT.DTRO.Models.SystemConfig;

[DataContract]
public class SystemConfig
{
    [DataMember(Name = "systemName")]
    public string SystemName { get; set; }

    [DataMember(Name = "xAppId")]
    public Guid xAppId { get; set; }

    [DataMember(Name = "CurrentUserName")]
    public string CurrentUserName { get; set; }

    [DataMember(Name = "isTest")]
    public bool IsTest { get; set; }
}
