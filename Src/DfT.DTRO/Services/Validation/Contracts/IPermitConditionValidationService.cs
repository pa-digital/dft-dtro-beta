namespace DfT.DTRO.Services.Validation.Contracts;

/// <summary>
/// Permit Condition Validation Service
/// </summary>
public interface IPermitConditionValidationService
{
    /// <summary>
    /// Check permit condition
    /// </summary>
    /// <param name="dtroSubmit">D-TRO record to validate</param>
    /// <returns>List of validation errors</returns>
    List<SemanticValidationError> Validate(DtroSubmit dtroSubmit);
}