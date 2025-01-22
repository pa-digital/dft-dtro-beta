namespace DfT.DTRO.Services.Validation.Contracts;

public interface IDtroQueryParamValidationService
{
    /// <summary>
    /// Check query string arguments
    /// </summary>
    /// <param name="traIds">List of ids of Tras that either currently own or created a DTRO</param>
    /// <param name="startDate">Earliest Creation or Last Updated date of a DTRO</param>
    /// <param name="endDate">Latest Creation or Last Updated date of a DTRO</param>
    /// <returns>List of validation errors</returns>
    List<SemanticValidationError> ValidateQueryParams(List<int> traIds, DateTime? fromDate, DateTime? toDate);
}