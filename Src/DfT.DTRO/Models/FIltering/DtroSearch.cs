namespace DfT.DTRO.Models.Filtering;

/// <summary>
/// D-TRO search criteria definition.
/// </summary>
public class DtroSearch : PaginatedRequest
{
    /// <summary>
    /// List of search queries.
    /// </summary>
    [MinLength(1)]
    public IEnumerable<SearchQuery> Queries { get; set; }
}