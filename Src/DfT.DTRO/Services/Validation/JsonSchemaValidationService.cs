using System.Collections.Generic;
using Google.Protobuf.WellKnownTypes;
using Microsoft.CodeAnalysis.Text;
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

        var validationErrorsList = DepackFrom(validationErrors.ToList());

        return validationErrorsList;
    }

    private List<DtroJsonValidationErrorResponse> DepackFrom(List<ValidationError> validationErrors)
    {
        if (validationErrors.Count == 0)
        {
            return new List<DtroJsonValidationErrorResponse>();
        }
        var depackedList = new List<DtroJsonValidationErrorResponse>();
        foreach (var validationError in validationErrors)
        {
            var depacked = new DtroJsonValidationErrorResponse
            {
                Message = validationError.Message,
                LineNumber = validationError.LineNumber,
                LinePosition = validationError.LinePosition,
                Path = validationError.Path,
                Value = validationError.Value,
                ErrorType = validationError.ErrorType.ToString(),
                ChildErrors = DepackFrom(validationError.ChildErrors.ToList())
            };
            depackedList.Add(depacked);
        }
        
        return depackedList;
    }
}