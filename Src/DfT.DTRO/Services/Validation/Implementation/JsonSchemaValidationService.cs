using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using SchemaVersion = DfT.DTRO.Models.SchemaTemplate.SchemaVersion;

namespace DfT.DTRO.Services.Validation.Implementation;

/// <inheritdoc cref="IJsonSchemaValidationService"/>
public class JsonSchemaValidationService : IJsonSchemaValidationService
{
    /// <inheritdoc cref="IJsonSchemaValidationService"/>
    public bool SchemaVersionSupportsValidation(SchemaVersion schemaVersion)
    {
        return schemaVersion.Major > 3 ||
            (schemaVersion.Major == 3 && schemaVersion.Minor >= 4);
    }

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