namespace DfT.DTRO.Models.Errors;

public class ApiErrorResponse
{
    public string Message { get; set; }

    public string Error { get; set; }

    public ApiErrorResponse(string message, string error)
    {
        Message = message;
        Error = error;
    }
}