namespace DfT.DTRO.Models.Pagination;

/// <summary>
/// Page request
/// </summary>
public class PaginatedRequest
{
    /// <summary>
    /// page
    /// </summary>
    [Range(1, int.MaxValue)]
    [Required]
    public int Page { get; set; }

    /// <summary>
    /// page size
    /// </summary>
    [Range(1, 50)]
    [Required]
    public int PageSize { get; set; }
}