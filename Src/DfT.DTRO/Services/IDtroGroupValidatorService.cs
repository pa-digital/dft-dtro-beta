using DfT.DTRO.Models.DtroDtos;
using DfT.DTRO.Models.Errors;
using System.Threading.Tasks;

namespace DfT.DTRO.Services;
public interface IDtroGroupValidatorService
{
    Task<DtroValidationException> ValidateDtro(DtroSubmit dtroSubmit, int? headerTa);
}