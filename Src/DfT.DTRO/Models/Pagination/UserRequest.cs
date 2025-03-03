namespace DfT.DTRO.Models.Pagination;

/// <summary>
/// Request parameters
/// </summary>
public class UserRequest : PaginatedRequest
{
    /// <summary>
    /// User identification
    /// </summary>
    [DataType(DataType.EmailAddress)]
    public string UserId { get; set; }
}