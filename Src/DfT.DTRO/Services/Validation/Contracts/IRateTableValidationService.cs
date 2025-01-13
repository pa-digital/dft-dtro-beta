namespace DfT.DTRO.Services.Validation.Contracts;

/// <summary>
/// Rate table validation service
/// </summary>
public interface IRateTableValidationService
{
    /// <summary>
    /// Validate rate table
    /// </summary>
    /// <param name="dtroSubmit">D-TRO record to validate</param>
    /// <returns>List of validation errors</returns>
    List<SemanticValidationError> Validate(DtroSubmit dtroSubmit);
}