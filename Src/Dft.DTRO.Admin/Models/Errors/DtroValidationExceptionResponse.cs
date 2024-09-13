namespace Dft.DTRO.Admin.Models.Errors;
public class DtroValidationExceptionResponse
{
    public ApiErrorResponse RequestComparedToSchemaVersion { get; set; }

    public List<DtroJsonValidationErrorResponse> RequestComparedToSchema { get; set; }

    public List<SemanticValidationError> RequestComparedToRules { get; set; }
}