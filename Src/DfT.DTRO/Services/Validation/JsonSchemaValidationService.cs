using DfT.DTRO.Extensions.Exceptions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace DfT.DTRO.Services.Validation;

public class JsonSchemaValidationService : IJsonSchemaValidationService
{
    public IList<DtroJsonValidationErrorResponse> ValidateRequestAgainstJsonSchema(string jsonSchemaAsString, string inputJson)
    {
        var parsedSchema = JSchema.Parse(jsonSchemaAsString);
        var parsedBody = JObject.Parse(inputJson);

        parsedBody.IsValid(parsedSchema, out IList<ValidationError> validationErrors);

        var validationErrorsList = validationErrors.ToList().MapFrom();

        return validationErrorsList;
    }
}