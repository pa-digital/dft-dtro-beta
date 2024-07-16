using System.Collections.Generic;
using Newtonsoft.Json.Schema;

namespace DfT.DTRO.Services.Validation;

public interface IJsonSchemaValidationService
{
    IList<ValidationError> ValidateRequestAgainstJsonSchema(string jsonSchemaAsString, string inputJson);
}