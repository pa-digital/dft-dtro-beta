using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Collections.Generic;

namespace DfT.DTRO.Services.Validation;

/// <inheritdoc />
public class JsonSchemaValidationService : IJsonSchemaValidationService
{
    /// <inheritdoc />
    public IList<ValidationError> ValidateRequestAgainstJsonSchema(string jsonSchemaAsString, string inputJson)
    {
        var parsedSchema = JSchema.Parse(jsonSchemaAsString);
        var parsedBody = JObject.Parse(inputJson);

        IList<ValidationError> validationErrors = new List<ValidationError>();
        parsedBody.IsValid(parsedSchema, out validationErrors);

        return validationErrors;
    }
}