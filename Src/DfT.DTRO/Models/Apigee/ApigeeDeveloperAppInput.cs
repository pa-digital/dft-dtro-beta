namespace DfT.DTRO.Models.Apigee;

/// <summary>
/// Auth token input.
/// </summary>
[DataContract]
public class ApigeeDeveloperAppInput
{

    /// <summary>
    /// app name.
    /// </summary>
    [DataMember(Name = "name")]
    public string Name { get; set; }

    /// <summary>
    /// api products.
    /// </summary>
    [DataMember(Name = "apiProducts")]
    public string[] ApiProducts { get; set; }
}