namespace DfT.DTRO.Models.Parameters;

/// <summary>
/// Passed in parameters to query by
/// </summary>
public class GetAllQueryParameters : QueryParameters
{
    /// <summary>
    /// SWA-like code that representing D-TRO record creator and owner
    /// </summary>
    public int? TraCode { get; set; }

    /// <summary>
    /// Query D-TRO records starting date
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Query D-TRO records ending date
    /// </summary>
    public DateTime? EndDate { get; set; }
}