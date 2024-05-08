using System.Collections.Generic;

namespace DfT.DTRO.Models.Pagination;

/// <summary>
/// Response with paginated data.
/// </summary>
/// <typeparam name="T">Type of response.</typeparam>
public class PaginatedResponse<T>
{
    /// <summary>
    /// Creates a paginated response.
    /// </summary>
    /// <param name="results">List of results.</param>
    /// <param name="page">Page number.</param>
    /// <param name="totalCount">Total count of records.</param>
    public PaginatedResponse(IReadOnlyCollection<T> results, int page, int totalCount)
    {
        Results = results;
        Page = page;
        PageSize = results.Count;
        TotalCount = totalCount;
    }

    /// <summary>
    /// List of records.
    /// </summary>
    public IReadOnlyCollection<T> Results { get; set; }

    /// <summary>
    /// Current page number.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Size of the current page.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Total number of records.
    /// </summary>
    public int TotalCount { get; set; }
}