namespace DfT.DTRO.Services.Validation.Contracts;

/// <summary>
/// Regulated place validation service
/// </summary>
public interface IRegulatedPlaceValidationService
{
    /// <summary>
    /// Validate regulated place
    /// </summary>
    /// <param name="dtroSubmit">D-TRO record to validate</param>
    /// <returns>List of validation errors</returns>
    List<SemanticValidationError> Validate(DtroSubmit dtroSubmit);
}