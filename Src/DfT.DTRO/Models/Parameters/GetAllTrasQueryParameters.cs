namespace DfT.DTRO.Models.Parameters;

/// <summary>
/// Passed in parameters to query by
/// </summary>
public class GetAllTrasQueryParameters : QueryParameters
{
    /// <summary>
    /// TRA name
    /// </summary>
    public string TraName { get; set; }
}