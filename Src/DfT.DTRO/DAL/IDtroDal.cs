namespace DfT.DTRO.DAL;

/// <summary>
/// Service interface providing methods that support storage for D-TROs.
/// </summary>
public interface IDtroDal
{
    /// <summary>
    /// Gets a D-TRO count for <paramref name="schemaVersion"/>
    /// </summary>
    /// <param name="schemaVersion">Schema version search for.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous save operation.</returns>
    Task<int> DtroCountForSchemaAsync(SchemaVersion schemaVersion);

    /// <summary>
    /// Check if a D-TRO exists by passing its <paramref name="id"/>
    /// </summary>
    /// <param name="id">D-TRO ID</param>
    /// <returns>
    /// A <see cref="Task"/> whose result is <see langword="true" />
    /// if a D-TRO with specified ID exists. Otherwise <see langword="false" />
    /// </returns>
    Task<bool> DtroExistsAsync(Guid id);

    /// <summary>
    /// Saves a D-TRO provided by <paramref name="dtroSubmit"/>
    /// to a storage device after converting it to a JSON string.
    /// </summary>
    /// <param name="dtroSubmit">Object representing a full D-TRO.</param>
    /// <param name="correlationId">Correlation ID passed when submitting the D-TRO.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous save operation.</returns>
    Task<GuidResponse> SaveDtroAsJsonAsync(DtroSubmit dtroSubmit, string correlationId);

    /// <summary>
    /// Gets D-TRO records.
    /// </summary>
    /// <param name="parameters">Parameters passed</param>
    /// <returns>All active D-TRO records</returns>
    Task<IEnumerable<Models.DataBase.DTRO>> GetDtrosAsync(GetAllQueryParameters parameters);

    /// <summary>
    /// Gets a D-TRO by <paramref name="id"/>.
    /// </summary>
    /// <param name="id">D-TRO ID</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous get operation.</returns>
    Task<Models.DataBase.DTRO> GetDtroByIdAsync(Guid id);

    /// <summary>
    /// Update an existing D-TRO by its <paramref name="guid"/>.
    /// </summary>
    /// <param name="guid">D-TRO ID.</param>
    /// <param name="dtroSubmit">Object containing a full D-TRO details.</param>
    /// <param name="correlationId">Correlation ID passed when updating the D-TRO.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous update operation.</returns>
    Task UpdateDtroAsJsonAsync(Guid guid, DtroSubmit dtroSubmit, string correlationId);

    /// <summary>
    /// Try updating an existing D-TRO by its <paramref name="guid"/>.
    /// </summary>
    /// <param name="guid">D-TRO ID.</param>
    /// <param name="dtroSubmit">Object containing a full D-TRO details.</param>
    /// <param name="correlationId">Correlation ID passed when updating the D-TRO.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous update operation.</returns>
    Task<bool> TryUpdateDtroAsJsonAsync(Guid guid, DtroSubmit dtroSubmit, string correlationId);

    /// <summary>
    /// Mark D-TRO as delete by its <paramref name="id"/>
    /// </summary>
    /// <param name="id">D-TRO ID.</param>
    /// <param name="deletionTime">Timestamp of deletion.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous soft delete operation.</returns>
    Task<bool> SoftDeleteDtroAsync(Guid id, DateTime? deletionTime);

    /// <summary>
    /// Find D-TRO details by its <paramref name="search"/>
    /// </summary>
    /// <param name="search">Search criteria object.</param>
    /// <returns>A <see cref="Task"/> representing paginated search operation.</returns>
    Task<PaginatedResult<Models.DataBase.DTRO>> FindDtrosAsync(DtroSearch search);

    /// <summary>
    /// Find D-TRO details by its <paramref name="search"/>
    /// </summary>
    /// <param name="search">Search criteria object.</param>
    /// <returns>A <see cref="Task"/> representing the search operation.</returns>
    Task<List<Models.DataBase.DTRO>> FindDtrosAsync(DtroEventSearch search);

    /// <summary>
    /// Delete D-TRO by its <paramref name="id"/>
    /// </summary>
    /// <param name="id">D-TRO ID.</param>
    /// <param name="deletionTime">Timestamp of deletion.</param>
    /// <returns>A <see cref="Task"/> representing the delete operation.</returns>
    Task<bool> DeleteDtroAsync(Guid id, DateTime? deletionTime = null);

    /// <summary>
    /// Assign D-TRO ownership by its <paramref name="id"/>
    /// </summary>
    /// <param name="id">D-TRO ID.</param>
    /// <param name="assignToTraId">Traffic regulation authority ID.</param>
    /// <param name="correlationId">Correlation ID passed when updating the D-TRO.</param>
    /// <returns>A <see cref="Task"/> representing operation.</returns>
    Task AssignDtroOwnership(Guid id, int assignToTraId, string correlationId);

}