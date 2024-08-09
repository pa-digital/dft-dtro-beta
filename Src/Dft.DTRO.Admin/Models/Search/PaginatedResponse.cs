namespace Dft.DTRO.Admin.Models.Search;

public class PaginatedResponse<T>
{
    public IReadOnlyCollection<T> Results { get; set; }

    public int Page { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }
}