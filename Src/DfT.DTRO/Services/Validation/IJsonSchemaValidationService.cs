using Newtonsoft.Json.Schema;

namespace DfT.DTRO.Services.Validation;

public interface IJsonSchemaValidationService
{
    IList<DtroJsonValidationErrorResponse> ValidateRequestAgainstJsonSchema(string jsonSchemaAsString, string inputJson);
}