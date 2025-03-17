namespace DfT.DTRO.Services.Validation.Contracts;

/// <summary>
/// Consultation validation service
/// </summary>
public interface IConsultationValidationService
{
    /// <summary>
    /// Validate consultation
    /// </summary>
    /// <param name="dtroSubmit">D-TRO record to validate</param>
    /// <returns>List of validation errors.</returns>
    List<SemanticValidationError> Validate(DtroSubmit dtroSubmit);
}