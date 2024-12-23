namespace DfT.DTRO.Services.Validation.Contracts;

/// <summary>
/// Source validation service
/// </summary>
public interface ISourceValidationService
{
    /// <summary>
    /// Validate source
    /// </summary>
    /// <param name="dtroSubmit">D-TRO record to validate</param>
    /// <param name="traCode">Traffic regulation authority code to check</param>
    /// <returns>List of validation errors.</returns>
    List<SemanticValidationError> Validate(DtroSubmit dtroSubmit, int? traCode);
}