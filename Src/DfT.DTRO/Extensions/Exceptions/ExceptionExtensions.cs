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

    public static List<SemanticValidationError> MapFrom(this List<SemanticValidationError> validationErrors)
    {
        if (validationErrors.Count == 0)
        {
            return new List<SemanticValidationError>();
        }

        return validationErrors
            .Select(validationError => new SemanticValidationError
            {
                Message = validationError.Message,
                Path = validationError.Path
            })
            .ToList();
    }

    public static string Beautify(this DtroValidationExceptionResponse response)
    {
        var errors = string.Empty;
        if (response == null)
        {
            return errors;
        }

        if (response.RequestComparedToRules.Any())
        {
            errors += string
                .Join("", response
                    .RequestComparedToRules
                    .Select(error => $"{error.Path}{Environment.NewLine}{error.Message}"));
        }
        else if (response.RequestComparedToSchema.Any())
        {
            errors += string
                .Join("", response
                    .RequestComparedToSchema
                    .Select(error => $"Path: {error.Path}{Environment.NewLine}" +
                                     $"Message: {error.Message}{Environment.NewLine}" +
                                     $"Child errors count: {error.ChildErrors.Count}{Environment.NewLine}" +
                                     $"Error Type: {error.ErrorType}{Environment.NewLine}" +
                                     $"Line number: {error.LineNumber}{Environment.NewLine}" +
                                     $"Line position: {error.LinePosition}{Environment.NewLine}" +
                                     $"Value: {error.Value}"));
        }
        else if (response.RequestComparedToSchemaVersion != null)

        {
            errors += response.RequestComparedToSchemaVersion?.Error + Environment.NewLine +
                      response.RequestComparedToSchemaVersion?.Message;
        }

        return errors;
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
