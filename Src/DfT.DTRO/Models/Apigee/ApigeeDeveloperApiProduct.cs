namespace DfT.DTRO.Models.Apigee;

/// <summary>
/// Represents API products for an Apigee app.
/// </summary>
[DataContract]
public class ApigeeDeveloperApiProduct
{
    /// <summary>
    /// Name of the API product.
    /// </summary>
    [DataMember(Name = "apiProduct")]
    public string ApiProduct { get; set; }

    /// <summary>
    /// Status of the API product (e.g., "approved").
    /// </summary>
    [DataMember(Name = "status")]
    public string Status { get; set; }
}