namespace DfT.DTRO.Services;

/// <summary>
/// Digital Traffic Regulation Order Service
/// </summary>
public interface IDtroService
{
    /// <summary>
    /// Save Digital Traffic Regulation Order
    /// </summary>
    /// <param name="dtroSubmit">Digital Traffic Regulation Order to submit</param>
    /// <param name="correlationId">Correlation unique identifier</param>
    /// <param name="xAppId">Application unique identifier</param>
    /// <returns>Unique identifier response</returns>
    Task<GuidResponse> SaveDtroAsJsonAsync(DtroSubmit dtroSubmit, string correlationId, Guid xAppId);

    /// <summary>
    /// Get all Digital Traffic Regulation Orders withing parameters
    /// </summary>
    /// <param name="parameters">Parameters passed</param>
    /// <returns>List of Digital Traffic Regulation Orders</returns>
    Task<IEnumerable<DtroResponse>> GetDtrosAsync(GetAllQueryParameters parameters);

    /// <summary>
    /// Get all D-TRO records withing parameters
    /// </summary>
    /// <returns>List of D-TRO records</returns>
    Task<bool> GenerateDtrosAsZipAsync();

    /// <summary>
    /// Get Digital Traffic Regulation Order
    /// </summary>
    /// <param name="id">Unique identifier passed</param>
    /// <returns>Digital Traffic Regulation Order response</returns>
    Task<DtroResponse> GetDtroByIdAsync(Guid id);

    /// <summary>
    /// Update Digital Traffic Regulation Order
    /// </summary>
    /// <param name="id">Unique identifier of update by</param>
    /// <param name="dtroSubmit">Digital Traffic Regulation Order to submit</param>
    /// <param name="correlationId">Correlation unique identifier</param>
    /// <param name="xAppId">Application unique identifier</param>
    /// <returns>Unique identifier response</returns>
    Task<GuidResponse> TryUpdateDtroAsJsonAsync(Guid id, DtroSubmit dtroSubmit, string correlationId, Guid xAppId);

    /// <summary>
    /// Delete Digital Traffic Regulation Order
    /// </summary>
    /// <param name="dtroId">Digital Traffic Regulation Order unique identifier to delete by</param>
    /// <param name="deletionTime">Deletion time</param>
    /// <returns><c>true</c> if successful, otherwise <c>false</c></returns>
    Task<bool> DeleteDtroAsync(Guid dtroId, DateTime? deletionTime = null);

    /// <summary>
    /// Search Digital Traffic Regulation Orders based on search criteria
    /// </summary>
    /// <param name="search">Digital Traffic Regulation Order search parameters</param>
    /// <returns>Paginated result of Digital Traffic Regulation Order</returns>
    Task<PaginatedResult<DigitalTrafficRegulationOrder>> FindDtrosAsync(DtroSearch search);

    /// <summary>
    /// Find event actions of Digital Traffic Regulation Order
    /// </summary>
    /// <param name="search">Digital Traffic Regulation Order event parameters</param>
    /// <returns>List of Digital Traffic Regulation Order</returns>
    Task<List<DigitalTrafficRegulationOrder>> FindDtrosAsync(DtroEventSearch search);

    /// <summary>
    /// Get all Digital Traffic Regulation Order Source History
    /// </summary>
    /// <param name="dtroId">Digital Traffic Regulation Order source unique identifier to get history</param>
    /// <returns>List of Digital Traffic Regulation Order history source response</returns>
    Task<List<DtroHistorySourceResponse>> GetDtroSourceHistoryAsync(Guid dtroId);

    /// <summary>
    /// Get Digital Traffic Regulation Order Source History
    /// </summary>
    /// <param name="dtroId">Digital Traffic Regulation Order provision unique identifier to get history</param>
    /// <returns>List of Digital Traffic Regulation Order history provision response</returns>
    Task<List<DtroHistoryProvisionResponse>> GetDtroProvisionHistoryAsync(Guid dtroId);

    /// <summary>
    /// Assign ownership to Digital Traffic Regulation Order
    /// </summary>
    /// <param name="dtroId">Digital Traffic Regulation Order unique identifier</param>
    /// <param name="xAppId">Application unique identifier</param>
    /// <param name="assignToUser">User unique identifier that is assigning</param>
    /// <param name="correlationId">Correlation unique identifier</param>
    /// <returns><c>true</c> if successful, otherwise <c>false</c></returns>
    Task<bool> AssignOwnershipAsync(Guid dtroId, Guid xAppId, Guid assignToUser, string correlationId);
}