using Newtonsoft.Json.Schema;
using System.Collections.Generic;

namespace DfT.DTRO.Services.Validation;

/// <summary>
/// Service layer implementation for working with JSON schemas.
/// </summary>
public interface IJsonSchemaValidationService
{
    /// <summary>
    /// Validates a request against a JSON Schema version.
    /// </summary>
    /// <param name="jsonSchemaAsString">The JSON schema to validate against in a string format.</param>
    /// <param name="inputJson">The DTRO submission request JSON string value.</param>
    /// <returns>A list of validation errors (if any are found).</returns>
    IList<ValidationError> ValidateRequestAgainstJsonSchema(string jsonSchemaAsString, string inputJson);
}