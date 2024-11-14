using DfT.DTRO.Extensions.Exceptions;
using DfT.DTRO.Services.Validation.Contracts;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace DfT.DTRO.Services.Validation.Implementation;

public class JsonSchemaValidationService : IJsonSchemaValidationService
{
    public IList<DtroJsonValidationErrorResponse> ValidateSchema(string jsonSchemaAsString, string inputJson)
    {
        var parsedSchema = JSchema.Parse(jsonSchemaAsString);
        var parsedBody = JObject.Parse(inputJson);

        parsedBody.IsValid(parsedSchema, out IList<ValidationError> validationErrors);

        var validationErrorsList = validationErrors.ToList().MapFrom();

        return validationErrorsList;
    }
}