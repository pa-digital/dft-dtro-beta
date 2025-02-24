
namespace Dft.DTRO.Admin.Services;
public interface IDtroUserService
{
    Task ActivateDtroUserAsync(Guid dtroUserId);
    Task DeactivateDtroUserAsync(Guid dtroUserId);
    Task<List<DtroUser>> GetDtroUsersAsync();
    Task<DtroUser> GetDtroUserAsync(Guid dtroUserId);
    Task<DtroUser> GetDtroUserAsyncByXAppId(Guid xAppId);
    Task<List<DtroUser>> SearchDtroUsersAsync(string partialName);
    Task CreateDtroUserAsync(DtroUser dtroUserRequest);
    Task UpdateDtroUserAsync(DtroUser dtroUserRequest);
    Task<bool> DeleteDtroUserAsync(List<Guid> selectedUsers);
}