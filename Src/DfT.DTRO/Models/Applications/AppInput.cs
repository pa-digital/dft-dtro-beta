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

    /// <summary>
    /// app type.
    /// </summary>
    [DataMember(Name = "type")]
    public string Type { get; set; }

    /// <summary>
    /// app purpose.
    /// </summary>
    [DataMember(Name = "purpose")]
    public string Purpose { get; set; }

    /// <summary>
    /// additional information.
    /// </summary>
    [DataMember(Name = "additionalInformation")]
    public string AdditionalInformation { get; set; }

    /// <summary>
    /// activity.
    /// </summary>
    [DataMember(Name = "activity")]
    public string Activity { get; set; }

    /// <summary>
    /// regions.
    /// </summary>
    [DataMember(Name = "regions")]
    public string Regions { get; set; }

    /// <summary>
    /// data type.
    /// </summary>
    [DataMember(Name = "dataType")]
    public string DataType { get; set; }
}