namespace DfT.DTRO.Models.Parameters;

/// <summary>
/// Passed in parameters to query by
/// </summary>
public class QueryParameters
{
    /// <summary>
    /// SWA-like codes that representing D-TRO record creator, owner and affected
    /// </summary>
    public int[]? TraCodes { get; set; }

    /// <summary>
    /// Query start date
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Query end date
    /// </summary>
    public DateTime? EndDate { get; set; }
}