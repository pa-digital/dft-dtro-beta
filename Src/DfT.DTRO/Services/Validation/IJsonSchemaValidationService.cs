using Newtonsoft.Json.Schema;

namespace DfT.DTRO.Services.Validation;

public interface IJsonSchemaValidationService
{
    IList<DtroJsonValidationError> ValidateRequestAgainstJsonSchema(string jsonSchemaAsString, string inputJson);
}