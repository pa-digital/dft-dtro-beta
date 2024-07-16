using System.ComponentModel.DataAnnotations;


public class DtroSearch : PaginatedRequest
{
    [MinLength(1)]
    public IEnumerable<SearchQuery> Queries { get; set; }
}