namespace DfT.DTRO.Services.Validation.Contracts;

/// <summary>
/// Schema validation
/// </summary>
public interface IJsonSchemaValidationService
{
    /// <summary>
    /// Determine whether schema version supports schema validation
    /// </summary>
    /// <param name="schemaVersion">Schema version</param>
    /// <returns>bool</returns>
    bool SchemaVersionSupportsValidation(SchemaVersion schemaVersion);

    /// <summary>
    /// Validate schema
    /// </summary>
    /// <param name="jsonSchemaAsString">Schema to validate</param>
    /// <param name="inputJson">Payload to validate against schema</param>
    /// <returns></returns>
    IList<DtroJsonValidationErrorResponse> ValidateSchema(string jsonSchemaAsString, string inputJson);
}