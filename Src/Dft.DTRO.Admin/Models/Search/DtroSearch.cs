namespace Dft.DTRO.Admin.Models.Search;

public class DtroSearch : PaginatedRequest
{
    [MinLength(1)]
    public IEnumerable<SearchQuery> Queries { get; set; }
}