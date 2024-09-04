namespace DfT.DTRO.Admin.Models.Errors;
public class NotFoundException 
{
    public NotFoundException(string message)
    {
        Message = message;
    }
    public string Message { get; set; } = "not found";
}