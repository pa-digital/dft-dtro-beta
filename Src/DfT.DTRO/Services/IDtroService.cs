using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DfT.DTRO.Models.DtroDtos;
using DfT.DTRO.Models.DtroEvent;
using DfT.DTRO.Models.DtroHistory;
using DfT.DTRO.Models.Filtering;
using DfT.DTRO.Models.Pagination;
using DfT.DTRO.Models.SchemaTemplate;
using DfT.DTRO.Models.SharedResponse;

namespace DfT.DTRO.Services;

public interface IDtroService
{
    public Task<int> DtroCountForSchemaAsync(SchemaVersion schemaVersion);

    Task<bool> DtroExistsAsync(Guid id);

    Task<GuidResponse> SaveDtroAsJsonAsync(DtroSubmit dtroSubmit, string correlationId, int? headerTa);

    Task<DtroResponse> GetDtroByIdAsync(Guid id);

    Task<GuidResponse> TryUpdateDtroAsJsonAsync(Guid id, DtroSubmit dtroSubmit, string correlationId, int? ta);

    Task<bool> DeleteDtroAsync(int? ta, Guid id, DateTime? deletionTime = null);

    Task<PaginatedResult<Models.DataBase.DTRO>> FindDtrosAsync(DtroSearch search);

    Task<List<Models.DataBase.DTRO>> FindDtrosAsync(DtroEventSearch search);

    Task<List<DtroHistorySourceResponse>> GetDtroSourceHistoryAsync(Guid dtroId);

    Task<List<DtroHistoryProvisionResponse>> GetDtroProvisionHistoryAsync(Guid dtroId);

    Task<bool> AssignOwnershipAsync(Guid id, int? apiTraId, int assignToTraId, string correlationId);
}