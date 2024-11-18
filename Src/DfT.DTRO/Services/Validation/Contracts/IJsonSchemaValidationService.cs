namespace DfT.DTRO.Services.Validation.Contracts;

public interface IJsonSchemaValidationService
{
    IList<DtroJsonValidationErrorResponse> ValidateSchema(string jsonSchemaAsString, string inputJson);
}