namespace DfT.DTRO.Models.Pagination;

/// <summary>
/// Parameters of a request for a paginated data.
/// </summary>
public class PaginatedRequest
{
    /// <summary>
    /// Number of requested pages.
    /// </summary>
    [Range(1, int.MaxValue)]
    [Required]
    public int Page { get; set; }

    /// <summary>
    /// Count of records in a single page.
    /// </summary>
    [Range(1, 50)]
    [Required]
    public int PageSize { get; set; }
}