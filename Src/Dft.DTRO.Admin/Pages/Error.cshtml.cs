using Dft.DTRO.Admin.Models.Errors;
namespace Dft.DTRO.Admin.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string Message { get; set; } = "noooo message";

    [BindProperty(SupportsGet = true)]
    public Object Error { get; set; }

    [BindProperty(SupportsGet = true)]
    public DtroJsonValidationError ValidationError { get; set; }  

    public void OnGet(string message, Object error)
    {
        Message = message;
        Error = error;

        if (error is DtroValidationException dtroValidationException)
        {
            // Example of accessing properties; this should be used if needed to prepare data for the view
            var schemaErrors = dtroValidationException.RequestComparedToSchema;
            var ruleErrors = dtroValidationException.RequestComparedToRules;
            // Populate ValidationError or any other properties based on the requirement
        }

        if (error is ApiErrorResponse xxxx)
        {
            var ssss = (ApiErrorResponse)xxxx;
        }
    }
}
