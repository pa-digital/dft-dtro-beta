using System.Collections.Generic;

namespace DfT.DTRO.Models.DtroEvent;

/// <summary>
/// The data structure of the result returned from the event search.
/// </summary>
public class DtroEventSearchResult
{
    /// <summary>
    /// List of change events in the DTRO service.
    /// </summary>
    public List<DtroEvent> Events { get; set; } = new ();

    /// <summary>
    /// Current page number.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Size of the current page.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Total number of records.
    /// </summary>
    public int TotalCount { get; set; }
}