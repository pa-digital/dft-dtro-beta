
namespace DfT.DTRO.Services;
public interface ITraService
{
    Task<GuidResponse> ActivateTraAsync(int traId);
    Task<GuidResponse> DeActivateTraAsync(int traId);
    Task<List<SwaCodeResponse>> GetSwaCodeAsync();
    Task<List<SwaCodeResponse>> SearchSwaCodes(string partialName);
    Task<GuidResponse> SaveTraAsync(SwaCodeRequest swaCodeRequest);
    Task<GuidResponse> UpdateTraAsync(SwaCodeRequest swaCodeRequest);
}