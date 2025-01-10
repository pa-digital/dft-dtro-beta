namespace DfT.DTRO.Services.Validation.Contracts;

/// <summary>
/// Rate line validation service
/// </summary>
public interface IRateLineValidationService
{
    /// <summary>
    /// Validate rate line
    /// </summary>
    /// <param name="dtroSubmit">D-TRO record to validate</param>
    /// <returns>List of validation errors</returns>
    List<SemanticValidationError> Validate(DtroSubmit dtroSubmit);
}