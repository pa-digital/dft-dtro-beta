namespace DfT.DTRO.Services.Validation.Contracts;

/// <summary>
/// Main validation service
/// </summary>
public interface IDtroGroupValidatorService
{
    /// <summary>
    /// Main validation method
    /// </summary>
    /// <param name="dtroSubmit">D-TRO record submitted to check</param>
    /// <param name="traCode">Traffic regulation authority code</param>
    /// <returns>Validation error</returns>
    Task<DtroValidationException> ValidateDtro(DtroSubmit dtroSubmit, int? traCode);
}