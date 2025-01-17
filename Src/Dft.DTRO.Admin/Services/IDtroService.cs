using System.Diagnostics;

namespace Dft.DTRO.Admin.Services;
public interface IDtroService
{
    Task CreateDtroAsync(IFormFile file);
    Task UpdateDtroAsync(Guid id, IFormFile file);
    Task<List<DtroHistoryProvisionResponse>> DtroProvisionHistory(Guid id);
    Task<List<DtroHistorySourceResponse>> DtroSourceHistory(Guid id);
    Task<IActionResult> ReassignDtroAsync(Guid id, Guid toDtroUserId);
    Task<PaginatedResponse<DtroSearchResult>> SearchDtros(int? traId, int pageNumber);
}