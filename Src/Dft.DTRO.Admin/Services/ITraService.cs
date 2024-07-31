
namespace Dft.DTRO.Admin.Services;

public interface ITraService
{
    Task ActivateTraAsync(int traId);
    Task CreateSchemaAsync(SwaCodeRequest swaCodeRequest);
    Task DeactivateTraAsync(int traId);
    Task<List<SwaCodeResponse>> GetSwaCodes();
    Task<List<SwaCodeResponse>> SearchSwaCodes(string partialName);
    Task UpdateSchemaAsync(SwaCodeRequest swaCodeRequest);
}