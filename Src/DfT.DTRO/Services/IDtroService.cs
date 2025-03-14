namespace DfT.DTRO.Services;

public interface IDtroService
{
    public Task<int> DtroCountForSchemaAsync(SchemaVersion schemaVersion);

    Task<bool> DtroExistsAsync(Guid id);

    Task<GuidResponse> SaveDtroAsJsonAsync(DtroSubmit dtroSubmit, string correlationId, Guid appId);

    /// <summary>
    /// Get all D-TRO records withing parameters
    /// </summary>
    /// <param name="parameters">Parameters passed</param>
    /// <returns>List of D-TRO records</returns>
    Task<IEnumerable<DtroResponse>> GetDtrosAsync(GetAllQueryParameters parameters);

    /// <summary>
    /// Get all D-TRO records withing parameters
    /// </summary>
    /// <param name="parameters">Parameters passed</param>
    /// <returns>List of D-TRO records</returns>
    Task<bool> GenerateDtrosAsZipAsync();

    Task<DtroResponse> GetDtroByIdAsync(Guid id);

    Task<GuidResponse> TryUpdateDtroAsJsonAsync(Guid id, DtroSubmit dtroSubmit, string correlationId, Guid appId);

    Task<bool> DeleteDtroAsync(Guid dtroId, DateTime? deletionTime = null);

    Task<PaginatedResult<Models.DataBase.DTRO>> FindDtrosAsync(DtroSearch search);

    Task<List<Models.DataBase.DTRO>> FindDtrosAsync(DtroEventSearch search);

    Task<List<DtroHistorySourceResponse>> GetDtroSourceHistoryAsync(Guid dtroId);

    Task<List<DtroHistoryProvisionResponse>> GetDtroProvisionHistoryAsync(Guid dtroId);

    Task<bool> AssignOwnershipAsync(Guid dtroId, Guid appId, Guid assignToUser, string correlationId);
}