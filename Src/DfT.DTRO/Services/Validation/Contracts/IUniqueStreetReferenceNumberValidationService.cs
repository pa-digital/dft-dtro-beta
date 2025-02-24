namespace DfT.DTRO.Services.Validation.Contracts;

/// <summary>
/// Unique street reference number validation service 
/// </summary>
public interface IUniqueStreetReferenceNumberValidationService
{
    /// <summary>
    /// Validate service
    /// </summary>
    /// <param name="dtroSubmit">D-TRO record to validate</param>
    /// <returns>List of validation errors</returns>
    List<SemanticValidationError> Validate(DtroSubmit dtroSubmit);
}