using DfT.DTRO.Models.Pagination;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DfT.DTRO.Models.Filtering;

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