using System.Threading.Tasks;
using DfT.DTRO.Models.DtroDtos;
using DfT.DTRO.Models.Errors;

namespace DfT.DTRO.Services;
public interface IDtroGroupValidatorService
{
    Task<DtroValidationException> ValidateDtro(DtroSubmit dtroSubmit, int? headerTa);
}