namespace DfT.DTRO.Services.Validation.Contracts;

/// <summary>
/// Rate line collection validation service
/// </summary>
public interface IRateLineCollectionValidationService
{
    /// <summary>
    /// Validate rate line collection 
    /// </summary>
    /// <param name="dtroSubmit">D-TRO record to validate</param>
    /// <returns>List of validation errors</returns>
    List<SemanticValidationError> Validate(DtroSubmit dtroSubmit);
}