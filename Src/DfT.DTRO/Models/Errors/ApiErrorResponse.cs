namespace DfT.DTRO.Models.Errors;

public class ApiErrorResponse
{
    public string Message { get; set; }

    public List<object> Errors { get; set; }

    public ApiErrorResponse(string message, List<object> errors)
    {
        Message = message;
        Errors = errors;
    }

    public ApiErrorResponse(string message, string error)
    {
        Message = message;
        Errors = new List<object> { error };
    }

    public ApiErrorResponse()
    {
    }
}