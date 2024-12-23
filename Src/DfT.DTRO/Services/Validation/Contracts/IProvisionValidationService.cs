namespace DfT.DTRO.Services.Validation.Contracts;

/// <summary>
/// Provision validation service
/// </summary>
public interface IProvisionValidationService
{
    /// <summary>
    /// Validate provision
    /// </summary>
    /// <param name="dtroSubmit">D-TRO record to validate</param>
    /// <returns>List of validation errors</returns>
    List<SemanticValidationError> Validate(DtroSubmit dtroSubmit);
}