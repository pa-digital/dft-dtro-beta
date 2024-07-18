namespace DfT.DTRO.Services;
public interface IDtroGroupValidatorService
{
    Task<DtroValidationException> ValidateDtro(DtroSubmit dtroSubmit, int? headerTa);
}