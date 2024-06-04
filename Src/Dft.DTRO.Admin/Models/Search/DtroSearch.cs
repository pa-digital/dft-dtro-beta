using System.ComponentModel.DataAnnotations;


/// <summary>
/// DTRO search criteria definition.
/// </summary>
public class DtroSearch : PaginatedRequest
{
    /// <summary>
    /// List of search queries.
    /// </summary>
    [MinLength(1)]
    public IEnumerable<SearchQuery> Queries { get; set; }
}