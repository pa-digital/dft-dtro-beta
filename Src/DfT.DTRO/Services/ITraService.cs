using DfT.DTRO.Models.SwaCode;

namespace DfT.DTRO.Services;
public interface ITraService
{
    Task<List<SwaCodeResponse>> GetUiFormattedSwaCodeAsync();
}