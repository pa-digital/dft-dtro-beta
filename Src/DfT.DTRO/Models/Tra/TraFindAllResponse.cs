namespace DfT.DTRO.Models.Tra;

/// <summary>
/// TRA find all response.
/// </summary>
[DataContract]
public class TraFindAllResponse
{
    /// <summary>
    /// TRA SWA code.
    /// </summary>
    [DataMember(Name = "swaCode")]
    public int SwaCode { get; set; }
    
    /// <summary>
    /// TRA name.
    /// </summary>
    [DataMember(Name = "name")]
    public string Name { get; set; }

}