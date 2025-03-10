namespace DfT.DTRO.Models.Tra;

/// <summary>
/// TRA find all response.
/// </summary>
[DataContract]
public class TraFindAllResponse
{
    /// <summary>
    /// TRA name.
    /// </summary>
    [DataMember(Name = "name")]
    public string Name { get; set; }

}