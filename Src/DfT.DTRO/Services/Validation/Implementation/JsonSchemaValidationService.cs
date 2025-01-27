using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace DfT.DTRO.Services.Validation.Implementation;

/// <inheritdoc cref="IJsonSchemaValidationService"/>
public class JsonSchemaValidationService : IJsonSchemaValidationService
{
    /// <inheritdoc cref="IJsonSchemaValidationService"/>
    public IList<DtroJsonValidationErrorResponse> ValidateSchema(string jsonSchemaAsString, string inputJson)
    {
        var parsedSchema = JSchema.Parse(jsonSchemaAsString);
        var parsedBody = JObject.Parse(inputJson);

        parsedBody.IsValid(parsedSchema, out IList<ValidationError> validationErrors);

        var validationErrorsList = validationErrors.ToList().MapFrom();

        return validationErrorsList;
    }
}