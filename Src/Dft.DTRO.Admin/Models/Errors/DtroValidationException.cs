namespace Dft.DTRO.Admin.Models.Errors;
public class DtroValidationException
{
    public DtroValidationException(string message) 
    {
        Message = message;
    }
    public string Message { get; set; } = "Dtro Validation Failure";
    public ApiErrorResponse RequestComparedToSchemaVersion { get; set; }

    public List<DtroJsonValidationError> RequestComparedToSchema { get; set; }

    public List<SemanticValidationError> RequestComparedToRules { get; set; }
}