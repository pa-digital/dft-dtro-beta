using Microsoft.AspNetCore.Mvc;

namespace Dft.DTRO.Admin.Services;
public interface IDtroService
{
    Task CreateDtroAsync(IFormFile file);
    Task<List<DtroHistoryProvisionResponse>> DtroProvisionHistory(Guid id);
    Task<List<DtroHistorySourceResponse>> DtroSourceHistory(Guid id);
    Task<IActionResult> ReassignDtroAsync(Guid id, int toTraId);
    Task<PaginatedResponse<DtroSearchResult>> SearchDtros();
    Task UpdateDtroAsync(Guid id, IFormFile file);
}