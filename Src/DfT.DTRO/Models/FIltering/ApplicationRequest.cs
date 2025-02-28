namespace DfT.DTRO.Models.Filtering;

public class ApplicationRequest : PaginatedRequest
{
    [DataType(DataType.EmailAddress)]
    public string UserId { get; set; }
}