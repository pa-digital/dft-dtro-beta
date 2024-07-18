using DfT.DTRO.Models.Validation;
using Newtonsoft.Json.Schema;

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

    public List<ValidationError> RequestComparedToSchema { get; set; }

    public List<SemanticValidationError> RequestComparedToRules { get; set; }
}