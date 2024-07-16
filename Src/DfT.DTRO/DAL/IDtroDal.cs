using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DfT.DTRO.Models.DtroDtos;
using DfT.DTRO.Models.DtroEvent;
using DfT.DTRO.Models.Filtering;
using DfT.DTRO.Models.Pagination;
using DfT.DTRO.Models.SchemaTemplate;
using DfT.DTRO.Models.SharedResponse;

namespace DfT.DTRO.Services;

public interface IDtroDal
{
    Task<int> DtroCountForSchemaAsync(SchemaVersion schemaVersion);

    Task<bool> DtroExistsAsync(Guid id);

    Task<GuidResponse> SaveDtroAsJsonAsync(DtroSubmit dtroSubmit, string correlationId);

    Task<Models.DataBase.DTRO> GetDtroByIdAsync(Guid id);

    Task UpdateDtroAsJsonAsync(Guid guid, DtroSubmit dtroSubmit, string correlationId);

    Task<bool> TryUpdateDtroAsJsonAsync(Guid guid, DtroSubmit dtroSubmit, string correlationId);

    Task<bool> SoftDeleteDtroAsync(Guid id, DateTime? deletionTime);

    Task<PaginatedResult<Models.DataBase.DTRO>> FindDtrosAsync(DtroSearch search);

    Task<List<Models.DataBase.DTRO>> FindDtrosAsync(DtroEventSearch search);

    Task<bool> DeleteDtroAsync(Guid id, DateTime? deletionTime = null);

    Task AssignDtroOwnership(Guid id, int assignToTraId, string correlationId);
}