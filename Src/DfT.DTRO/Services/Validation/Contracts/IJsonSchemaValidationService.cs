namespace DfT.DTRO.Services.Validation.Contracts;

/// <summary>
/// Schema validation
/// </summary>
public interface IJsonSchemaValidationService
{
    /// <summary>
    /// Validate schema
    /// </summary>
    /// <param name="jsonSchemaAsString">Schema to validate</param>
    /// <param name="inputJson">Payload to validate against schema</param>
    /// <returns></returns>
    IList<DtroJsonValidationErrorResponse> ValidateSchema(string jsonSchemaAsString, string inputJson);
}