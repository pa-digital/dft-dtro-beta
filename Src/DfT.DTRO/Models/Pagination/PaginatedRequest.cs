namespace DfT.DTRO.Models.Pagination;

public class PaginatedRequest
{
    [Range(1, int.MaxValue)]
    [Required]
    public int Page { get; set; }

    [Range(1, 50)]
    [Required]
    public int PageSize { get; set; }
}