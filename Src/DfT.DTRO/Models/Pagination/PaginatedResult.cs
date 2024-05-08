using System.Collections.Generic;

namespace DfT.DTRO.Models.Pagination;

/// <summary>
/// Result of a query with paginated data.
/// </summary>
/// <typeparam name="T">Type of result.</typeparam>
public class PaginatedResult<T>
{
    /// <summary>
    /// Creates a paginated query result.
    /// </summary>
    /// <param name="results">Results list.</param>
    /// <param name="totalCount">Total count of records.</param>
    public PaginatedResult(IReadOnlyCollection<T> results, int totalCount)
    {
        Results = results;
        TotalCount = totalCount;
    }

    /// <summary>
    /// List of records.
    /// </summary>
    public IReadOnlyCollection<T> Results { get; set; }

    /// <summary>
    /// Total number of records.
    /// </summary>
    public int TotalCount { get; set; }
}