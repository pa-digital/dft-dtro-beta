using DfT.DTRO.Models.DtroDtos;
using DfT.DTRO.Models.DtroEvent;
using DfT.DTRO.Models.Filtering;
using DfT.DTRO.Models.Pagination;
using DfT.DTRO.Models.SchemaTemplate;
using DfT.DTRO.Models.SharedResponse;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DfT.DTRO.Models.DtroHistory;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DfT.DTRO.Services;

/// <summary>
/// Service layer implementation for storage.
/// </summary>
public interface IDtroService
{
    /// <summary>
    /// Gets a DTRO count for <paramref name="schemaVersion"/>.
    /// </summary>
    /// <param name="schemaVersion">The schema Version to search for.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous save operation.</returns>
    public Task<int> DtroCountForSchemaAsync(SchemaVersion schemaVersion);

    /// <summary>
    /// Checks if the DTRO exists in the storage.
    /// </summary>
    /// <param name="id">The unique id of the DTRO.</param>
    /// <returns>
    /// A <see cref="Task"/> whose result is <see langword="true"/>
    /// if a DTRO with the specified ID exists;
    /// otherwise <see langword="false"/>.
    /// </returns>
    Task<bool> DtroExistsAsync(Guid id);

    /// <summary>
    /// Saves a DTRO provided in <paramref name="dtroSubmit"/> to a storage device
    /// after converting it to a JSON string.
    /// </summary>
    /// <param name="dtroSubmit">The DTRO Json content.</param>
    /// <param name="correlationId">The correlation id.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous save operation.</returns>
    Task<GuidResponse> SaveDtroAsJsonAsync(DtroSubmit dtroSubmit, string correlationId, int? headerTa);

    /// <summary>
    /// Gets a DTRO domain object from storage by a quoted id.
    /// </summary>
    /// <param name="id">The unique identifier of the DTRO.</param>
    /// <returns>A <see cref="Models.DataBase.DTRO"/> instance.</returns>
    Task<DtroResponse> GetDtroByIdAsync(Guid id);

    /// <summary>
    /// Tries to update the DTRO.
    /// </summary>
    /// <param name="id">The unique id of the DTRO.</param>
    /// <param name="dtroSubmit">The DTRO Json content.</param>
    /// <param name="correlationId">The correlation id.</param>
    /// <param name="ta">Traffic Authority that is creating this D-TRO</param>
    /// <returns>
    /// A <see cref="Task"/> that resolves to <see langword="true"/>
    /// if the DTRO was successfully updated
    /// or <see langword="false"/> if it was not found.
    /// </returns>
    Task<GuidResponse> TryUpdateDtroAsJsonAsync(Guid id, DtroSubmit dtroSubmit, string correlationId, int? ta);

    /// <summary>
    /// Marks the specified DTRO as deleted (does not delete the DTRO immediately).
    /// </summary>
    /// <param name="ta">Traffic Authority that is creating this D-TRO</param>
    /// <param name="id">The unique id of the DTRO.</param>
    /// <param name="deletionTime">The time of deletion. Will default to <see cref="DateTime.UtcNow"/> if not provided.</param>
    /// <returns>
    /// A <see cref="Task"/> that resolves to <see langword="true"/>
    /// if the DTRO was successfully marked deleted
    /// or <see langword="false"/> if it was not found.
    /// </returns>
    Task<bool> DeleteDtroAsync(int? ta, Guid id, DateTime? deletionTime = null);

    /// <summary>
    /// Finds all DTROs that match the criteria specified in <paramref name="search"/>.
    /// </summary>
    /// <param name="search">The search criteria.</param>
    /// <returns>A <see cref="Task"/> that resolves to the paginated list of <see cref="Models.DataBase.DTRO"/> that match the criteria.</returns>
    Task<PaginatedResult<Models.DataBase.DTRO>> FindDtrosAsync(DtroSearch search);

    /// <summary>
    /// Finds all DTRO events that match the criteria specified in <paramref name="search"/>.
    /// </summary>
    /// <param name="search">The search criteria.</param>
    /// <returns>A <see cref="Task"/> that resolves to a collection of <see cref="Models.DataBase.DTRO"/> that match the criteria.</returns>
    Task<List<Models.DataBase.DTRO>> FindDtrosAsync(DtroEventSearch search);

    /// <summary>
    /// Find all D-TRO Source History
    /// </summary>
    /// <param name="dtroId">D-TRO ID passed</param>
    /// <returns>List of D-TRO Source History</returns>
    Task<List<DtroHistorySourceResponse>> GetDtroSourceHistoryAsync(Guid dtroId);

    /// <summary>
    /// Find all D-TRO Provision History
    /// </summary>
    /// <param name="dtroId">D-TRO ID passed</param>
    /// <returns>List of D-TRO Provision History</returns>
    Task<List<DtroHistoryProvisionResponse>> GetDtroProvisionHistoryAsync(Guid dtroId);

    /// <summary>
    /// Assign ownership.
    /// </summary>
    /// <param name="id">D-TRO ID passed.</param>
    /// <param name="apiTraId">The TRA ID passed in the header by the API manager.</param>
    /// <param name="assignToTraId">TRA ID to assign ownership to.</param>
    /// <param name="correlationId">Correlation ID passed</param>
    /// <returns>Ok.</returns>
    Task<bool> AssignOwnershipAsync(Guid id, int? apiTraId, int assignToTraId, string correlationId);
}