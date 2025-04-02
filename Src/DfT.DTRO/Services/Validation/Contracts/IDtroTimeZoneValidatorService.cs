namespace DfT.DTRO.Services.Validation.Contracts;

/// <summary>
/// Time zone validator service
/// </summary>
public interface IDtroTimeZoneValidatorService
{
    /// <summary>
    /// Validates the D-TRO submission.
    /// </summary>
    /// <param name="dtroSubmit">D-TRO record payload to validate against.</param>
    /// <returns>
    /// A D-TRO validation exception
    /// </returns>
    DtroValidationException Validate(DtroSubmit dtroSubmit);
}