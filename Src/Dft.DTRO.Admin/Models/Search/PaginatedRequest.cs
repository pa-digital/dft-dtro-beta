using System.ComponentModel.DataAnnotations;


/// <summary>
///     Parameters of a request for paginated data.
/// </summary>
public class PaginatedRequest
{
    /// <summary>
    ///     Number of the requested page.
    /// </summary>
    /// <example>1.</example>
    [Range(1, int.MaxValue)]
    [Required]
    public int Page { get; set; }

    /// <summary>
    ///     Count of records in a single page.
    /// </summary>
    /// <example>10.</example>
    [Range(1, 50)]
    [Required]
    public int PageSize { get; set; }
}