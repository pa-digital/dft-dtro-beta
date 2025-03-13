namespace DfT.DTRO.Models.Applications;

/// <summary>
/// Auth token input.
/// </summary>
[DataContract]
public class AppInput
{

    /// <summary>
    /// app name.
    /// </summary>
    [DataMember(Name = "name")]
    public string Name { get; set; }

}