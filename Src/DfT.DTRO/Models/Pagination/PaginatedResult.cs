namespace DfT.DTRO.Models.Pagination;

/// <summary>
/// Result of a query with paginated data.
/// </summary>
/// <typeparam name="T">Result type.</typeparam>
public class PaginatedResult<T>
{
    /// <summary>
    /// A new <see cref="PaginatedResult{T}"/> instance class.
    /// </summary>
    /// <param name="results">Result list.</param>
    /// <param name="totalCount">Total count of records.</param>
    public PaginatedResult(IReadOnlyCollection<T> results, int totalCount)
    {
        Results = results;
        TotalCount = totalCount;
    }

    /// <summary>
    /// Records list.
    /// </summary>
    public IReadOnlyCollection<T> Results { get; set; }

    /// <summary>
    /// Records total count.
    /// </summary>
    public int TotalCount { get; set; }
}