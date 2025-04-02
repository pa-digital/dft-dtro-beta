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
    /// A task that represents the asynchronous operation. The task result contains a 
    /// <see cref="DtroValidationException"/> if validation fails.
    /// </returns>
    /// <exception cref="DtroValidationException">
    /// Thrown when the D-TRO submission fails validation.
    /// </exception>
    Task<DtroValidationException> Validate(DtroSubmit dtroSubmit);
}