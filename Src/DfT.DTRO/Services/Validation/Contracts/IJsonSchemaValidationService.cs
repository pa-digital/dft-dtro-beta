namespace DfT.DTRO.Services.Validation.Contracts;

public interface IJsonSchemaValidationService
{
    IList<DtroJsonValidationErrorResponse> ValidateRequestAgainstJsonSchema(string jsonSchemaAsString, string inputJson);
}