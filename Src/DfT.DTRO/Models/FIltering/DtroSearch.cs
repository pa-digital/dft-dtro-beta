using DfT.DTRO.Models.Pagination;

namespace DfT.DTRO.Models.Filtering;

public class DtroSearch : PaginatedRequest
{
    [MinLength(1)]
    public IEnumerable<SearchQuery> Queries { get; set; }
}