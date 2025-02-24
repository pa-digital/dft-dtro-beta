using Dft.DTRO.Admin.Models.Errors;
namespace Dft.DTRO.Admin.Models.Views;

public class ErrorView
{
    public string ErrorType { get; set; }
    public ApiErrorResponse ApiErrorResponse  { get; set; }
    public ApiErrorResponse UiErrorResponse { get; set; }
    public DtroValidationExceptionResponse DtroValidationException { get; set; }
}
