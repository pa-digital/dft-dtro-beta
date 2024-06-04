using System.Collections.Generic;

/// <summary>
/// Response with paginated data.
/// </summary>
/// <typeparam name="T">Type of response.</typeparam>
public class PaginatedResponse<T>
{
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