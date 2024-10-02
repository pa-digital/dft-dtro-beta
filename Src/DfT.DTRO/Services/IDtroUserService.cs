
namespace DfT.DTRO.Services;
public interface IDtroUserService
{
    Task<List<DtroUserResponse>> GetAllDtroUsersAsync();
    Task<DtroUserResponse> GetDtroUserAsync(Guid id);
    Task<List<DtroUserResponse>> SearchDtroUsers(string partialName);
    Task<GuidResponse> SaveDtroUserAsync(DtroUserRequest dtroUserRequest);
    Task<GuidResponse> UpdateDtroUserAsync(DtroUserRequest dtroUserRequest);
    Task<bool> DeleteDtroUsersAsync(List<Guid> dtroUserIds);
}