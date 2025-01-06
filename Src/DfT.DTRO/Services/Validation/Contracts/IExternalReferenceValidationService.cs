namespace DfT.DTRO.Services.Validation.Contracts;

/// <summary>
/// External reference validation service
/// </summary>
public interface IExternalReferenceValidationService
{
    /// <summary>
    /// Validate external reference
    /// </summary>
    /// <param name="dtroSubmit">D-TRO record payload to validate against.</param>
    /// <returns>List of validation errors</returns>
    List<SemanticValidationError> Validate(DtroSubmit dtroSubmit);
}