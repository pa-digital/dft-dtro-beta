using System.Text.Json.Serialization;

namespace Dft.DTRO.Admin.Models.Errors;

public class ApiErrorResponse 
{
    public string Message { get; set; }

    public string Error { get; set; }
}