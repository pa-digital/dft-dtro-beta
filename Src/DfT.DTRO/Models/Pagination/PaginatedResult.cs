namespace DfT.DTRO.Models.Pagination;

public class PaginatedResult<T>
{
    public PaginatedResult(IReadOnlyCollection<T> results, int totalCount)
    {
        Results = results;
        TotalCount = totalCount;
    }

    public IReadOnlyCollection<T> Results { get; set; }

    public int TotalCount { get; set; }
}