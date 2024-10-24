using DfT.DTRO.Models.Validation;
using Newtonsoft.Json.Schema;

namespace DfT.DTRO.Extensions.Exceptions;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public static class ExceptionExtensions
{
    public static List<DtroJsonValidationErrorResponse> MapFrom(this List<ValidationError> validationErrors)
    {
        if (validationErrors.Count == 0)
        {
            return new List<DtroJsonValidationErrorResponse>();
        }

        return validationErrors
            .Select(validationError => new DtroJsonValidationErrorResponse
            {
                Message = validationError.Message,
                LineNumber = validationError.LineNumber,
                LinePosition = validationError.LinePosition,
                Path = validationError.Path,
                Value = validationError.Value,
                ErrorType = validationError.ErrorType.ToString(),
                ChildErrors = MapFrom(validationError.ChildErrors.ToList())
            })
            .ToList();
    }

    public static List<SemanticValidationError> MapFrom(this IList<SemanticValidationError> validationErrors)
    {
        if (validationErrors.Count == 0)
        {
            return new List<SemanticValidationError>();
        }

        return validationErrors
            .Select(validationError => new SemanticValidationError
            {
                Message = validationError.Message,
                Path = validationError.Path,
                Name = validationError.Name,
                Rule = validationError.Rule
            })
            .ToList();
    }

    public static IDictionary<string, object> Beautify(this DtroValidationExceptionResponse response)
    {
        var errors = new Dictionary<string, object>();
        if (response == null)
        {
            return errors;
        }

        if (response.RequestComparedToRules.Any())
        {
            for (int index = 0; index < response.RequestComparedToRules.Count; index++)
            {
                SemanticValidationError error = response.RequestComparedToRules[index];
                errors.Add($"{index}_Name", error.Name);
                errors.Add($"{index}_Message", error.Message);
                errors.Add($"{index}_Path", error.Path);
                errors.Add($"{index}_Rule", error.Rule);
            }
        }
        else if (response.RequestComparedToSchema.Any())
        {
            response.RequestComparedToSchema.Beautify();
        }
        else if (response.RequestComparedToSchemaVersion != null)
        {
            errors.Add("Error", response.RequestComparedToSchemaVersion.Error);
            errors.Add("Message", response.RequestComparedToSchemaVersion.Message);
        }

        return errors;
    }

    private static IDictionary<string, object> Beautify(this IList<DtroJsonValidationErrorResponse> response)
    {
        IDictionary<string, object> errors = new Dictionary<string, object>();
        for (int index = 0; index < response.Count; index++)
        {
            DtroJsonValidationErrorResponse item = response[index];
            errors.Add($"{index}_Path", item.Path);
            errors.Add($"{index}_Message", item.Message);
            errors.Add($"{index}_Children", string.Join("|", item.ChildErrors.Beautify()));
            errors.Add($"{index}_ErrorType", item.ErrorType);
            errors.Add($"{index}_LineNumber", item.LineNumber);
            errors.Add($"{index}_LinePosition", item.LinePosition);
            errors.Add($"{index}_Value", item.Value);
        }

        return errors;
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
