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

        var validationErrorsList = ExtractValidationErrors(validationErrors).ToList();

        return validationErrorsList;
    }

    private IEnumerable<DtroJsonValidationErrorResponse> ExtractValidationErrors(IEnumerable<ValidationError> errors, string parentPath = "")
    {
        foreach (var error in errors)
        {
            string fullPath = string.IsNullOrEmpty(error.Path) ? parentPath : $"{parentPath}.{error.Path}".Trim('.');
            yield return new DtroJsonValidationErrorResponse
            {
                Path = string.IsNullOrEmpty(fullPath) ? "root" : fullPath,
                Message = error.Message,
                ErrorType = error.ErrorType.ToString(),
                Value = error.Value,
            };

            if (error.ChildErrors?.Count > 0)
            {
                foreach (var childError in ExtractValidationErrors(error.ChildErrors, fullPath))
                {
                    yield return childError;
                }
            }
        }
    }
}