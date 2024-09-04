using DfT.DTRO.Models.Validation;

namespace DfT.DTRO.Models.Errors;

public class DtroValidationException : Exception
{
    public DtroValidationException()
       : base("Dtro Validation Failure")
    {
    }

    public DtroValidationException(string message) : base(message)
    {
    }

    public ApiErrorResponse RequestComparedToSchemaVersion { get; set; }

    public List<DtroJsonValidationError> RequestComparedToSchema { get; set; }

    public List<SemanticValidationError> RequestComparedToRules { get; set; }
}