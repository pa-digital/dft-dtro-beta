
namespace Dft.DTRO.Admin.Services;

public interface ITraService
{
    Task ActivateTraAsync(int traId);
    Task CreateTraAsync(SwaCode swaCodeRequest);
    Task DeactivateTraAsync(int traId);
    Task<List<SwaCode>> GetSwaCodes();
    Task<SwaCode> GetSwaCode(int id);
    Task<List<SwaCode>> SearchSwaCodes(string partialName);
    Task UpdateTraAsync(SwaCode swaCodeRequest);
}