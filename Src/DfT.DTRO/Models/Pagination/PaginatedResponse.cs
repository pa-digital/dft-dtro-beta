namespace DfT.DTRO.Models.Pagination;

public class PaginatedResponse<T>
{
    public PaginatedResponse(IReadOnlyCollection<T> results, int page, int totalCount)
    {
        Results = results;
        Page = page;
        PageSize = results.Count;
        TotalCount = totalCount;
    }

    public IReadOnlyCollection<T> Results { get; set; }

    public int Page { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }
}