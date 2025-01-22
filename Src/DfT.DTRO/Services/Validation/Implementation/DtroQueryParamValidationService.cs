namespace DfT.DTRO.Services.Validation.Implementation;

public class DtroQueryParamValidationService : IDtroQueryParamValidationService
{
    public List<SemanticValidationError> ValidateQueryParams(List<int> traIds, DateTime? fromDate, DateTime? toDate)
    {
        List<SemanticValidationError> errors = new();

        // Check if at least one parameter is provided
        bool isValid = (traIds != null && traIds.Count > 0);

        if (!isValid)
        {
            errors.Add(new SemanticValidationError
            {
                Name = "No parameters provided",
                Message = "Please provide at least one valid parameter: " +
                          "a list of one of more traIds, and/or a valid date range (fromDate and toDate)."
            });
        }

        // Validate date range specifically if dates are provided partially
        fromDate ??= DateTime.MinValue;
        toDate ??= DateTime.Today;

        return errors;
    }
}