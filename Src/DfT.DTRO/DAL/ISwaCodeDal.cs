
namespace DfT.DTRO.DAL;

public interface ISwaCodeDal
{
    Task<GuidResponse> ActivateTraAsync(int traId);
    Task<GuidResponse> DeActivateTraAsync(int traId);
    Task<List<SwaCodeResponse>> GetAllCodesAsync();
    Task<List<SwaCodeResponse>> SearchSwaCodesAsync(string partialName);
    Task<SwaCode> GetTraAsync(int traId);
    Task<GuidResponse> SaveTraAsync(SwaCodeRequest swaCodeRequest);
    Task<bool> TraExistsAsync(int traId);
    Task<GuidResponse> UpdateTraAsync(SwaCodeRequest swaCodeRequest);

}