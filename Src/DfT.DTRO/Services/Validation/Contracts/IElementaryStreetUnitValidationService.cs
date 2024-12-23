namespace DfT.DTRO.Services.Validation.Contracts;

/// <summary>
/// Elementary street unit validation service 
/// </summary>
public interface IElementaryStreetUnitValidationService
{
    /// <summary>
    /// Validate elementary street unit
    /// </summary>
    /// <param name="dtroSubmit">D-TRO record payload to validate against.</param>
    /// <returns>List of validation errors</returns>
    List<SemanticValidationError> Validate(DtroSubmit dtroSubmit);
}