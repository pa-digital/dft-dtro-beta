﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DfT.DTRO.DAL;
using DfT.DTRO.Extensions;
using DfT.DTRO.Models.DataBase;
using DfT.DTRO.Models.DtroDtos;
using DfT.DTRO.Models.DtroEvent;
using DfT.DTRO.Models.DtroHistory;
using DfT.DTRO.Models.Errors;
using DfT.DTRO.Models.Filtering;
using DfT.DTRO.Models.Pagination;
using DfT.DTRO.Models.SchemaTemplate;
using DfT.DTRO.Models.SharedResponse;
using DfT.DTRO.Services.Mapping;

namespace DfT.DTRO.Services;

/// <summary>
/// An implementation of <see cref="IDtroService"/>.
/// </summary>
public class DtroService : IDtroService
{
    private readonly IDtroDal _dtroDal;
    private readonly IDtroHistoryDal _dtroHistoryDal;
    private readonly ISchemaTemplateDal _schemaTemplateDal;
    private readonly IDtroMappingService _dtroMappingService;
    private readonly IDtroGroupValidatorService _dtroGroupValidatorService;

    /// <summary>
    /// The default constructor.
    /// </summary>
    /// <param name="dtroDal">An <see cref="IDtroDal"/> instance.</param>
    /// <param name="dtroHistoryDal">An <see cref="IDtroHistoryDal"/> instance.</param>
    /// <param name="schemaTemplateDal">An <see cref="ISchemaTemplateDal"/> instance.</param>
    /// <param name="dtroMappingService">An <see cref="IDtroMappingService"/> instance.</param>
    /// <param name="dtroGroupValidatorService">An <see cref="IDtroGroupValidatorService"/> instance.</param>
    public DtroService(IDtroDal dtroDal, IDtroHistoryDal dtroHistoryDal, ISchemaTemplateDal schemaTemplateDal, IDtroMappingService dtroMappingService, IDtroGroupValidatorService dtroGroupValidatorService)
    {
        _dtroDal = dtroDal;
        _dtroHistoryDal = dtroHistoryDal;
        _schemaTemplateDal = schemaTemplateDal;
        _dtroMappingService = dtroMappingService;
        _dtroGroupValidatorService = dtroGroupValidatorService;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteDtroAsync(int? ta, Guid id, DateTime? deletionTime = null)
    {
        deletionTime ??= DateTime.UtcNow;
        if (ta == null)
        {
            throw new Exception();
        }

        var result = await _dtroDal.SoftDeleteDtroAsync(id, deletionTime);
        if (!result)
        {
            throw new NotFoundException();
        }

        return true;
    }

    /// <inheritdoc/>
    public async Task<bool> DtroExistsAsync(Guid id)
    {
        return await _dtroDal.DtroExistsAsync(id);
    }

    /// <inheritdoc/>
    public async Task<int> DtroCountForSchemaAsync(SchemaVersion schemaVersion)
    {
        return await _dtroDal.DtroCountForSchemaAsync(schemaVersion);
    }

    /// <inheritdoc/>
    public async Task<DtroResponse> GetDtroByIdAsync(Guid id)
    {
        var dtro = await _dtroDal.GetDtroByIdAsync(id);
        if (dtro is null)
        {
            throw new NotFoundException();
        }

        var dtroResponse = _dtroMappingService.MapToDtroResponse(dtro);
        return dtroResponse;
    }

    /// <inheritdoc/>
    public async Task<GuidResponse> SaveDtroAsJsonAsync(DtroSubmit dtroSubmit, string correlationId, int? headerTa)
    {
        var validationErrors = await _dtroGroupValidatorService.ValidateDtro(dtroSubmit, headerTa);

        if (validationErrors is not null)
        {
            throw validationErrors;
        }

        return await _dtroDal.SaveDtroAsJsonAsync(dtroSubmit, correlationId);
    }

    /// <inheritdoc/>
    public async Task<GuidResponse> TryUpdateDtroAsJsonAsync(Guid id, DtroSubmit dtroSubmit, string correlationId, int? headerTa)
    {
        var validationErrors = await _dtroGroupValidatorService.ValidateDtro(dtroSubmit, headerTa);

        if (validationErrors is not null)
        {
            throw validationErrors;
        }

        var dtroExists = await _dtroDal.DtroExistsAsync(id);
        if (!dtroExists)
        {
            throw new NotFoundException();
        }

        Models.DataBase.DTRO currentDtro = await _dtroDal.GetDtroByIdAsync(id);


        DTROHistory historyDtro = _dtroMappingService.MapToDtroHistory(currentDtro);

        var isSaved = await _dtroHistoryDal.SaveDtroInHistoryTable(historyDtro);
        if (!isSaved)
        {
            throw new Exception("Failed to write to history table");
        }

        await _dtroDal.UpdateDtroAsJsonAsync(id, dtroSubmit, correlationId);
        return new GuidResponse { Id = id };
    }

    /// <inheritdoc/>
    public async Task<PaginatedResult<Models.DataBase.DTRO>> FindDtrosAsync(DtroSearch search)
    {
        return await _dtroDal.FindDtrosAsync(search);
    }

    /// <inheritdoc />
    public async Task<List<Models.DataBase.DTRO>> FindDtrosAsync(DtroEventSearch search)
    {
        return await _dtroDal.FindDtrosAsync(search);
    }

    /// <inheritdoc />
    public async Task<List<DtroHistorySourceResponse>> GetDtroSourceHistoryAsync(Guid dtroId)
    {
        List<DTROHistory> dtroHistories = await _dtroHistoryDal.GetDtroHistory(dtroId);
        var histories = dtroHistories
            .Select(_dtroMappingService.GetSource)
            .Where(response => response != null)
            .ToList();

        var current = await _dtroDal.GetDtroByIdAsync(dtroId);
        var currentAsHistory = _dtroMappingService.MapToDtroHistory(current);
        var currentSource = _dtroMappingService.GetSource(currentAsHistory);

        var completeList = new List<DtroHistorySourceResponse>();

        if (currentSource != null)
        {
            var first = histories.FirstOrDefault();

            if (first == null || !currentSource.ComparePropertiesValues(first))
            {
                completeList.Add(currentSource);
            }
        }

        completeList.AddRange(histories);

        return completeList;
    }

    public async Task<List<DtroHistoryProvisionResponse>> GetDtroProvisionHistoryAsync(Guid dtroId)
    {
        List<DTROHistory> dtroHistories = await _dtroHistoryDal.GetDtroHistory(dtroId);

        var histories = dtroHistories
            .SelectMany(history => _dtroMappingService.GetProvision(history))
            .Where(response => response != null)
            .ToList();

        var currentDtro = await _dtroDal.GetDtroByIdAsync(dtroId);
        var currentAsHistory = _dtroMappingService.MapToDtroHistory(currentDtro);
        var currentProvisions = _dtroMappingService.GetProvision(currentAsHistory);
        var completeList = new List<DtroHistoryProvisionResponse>();

        // Process each current provision
        foreach (var currentProvision in currentProvisions)
        {
            // Find old provisions with the same reference
            var oldProvisions = histories.Where(x => x.Reference == currentProvision.Reference).ToList();

            if (oldProvisions.Count > 0)
            {
                // Add current provision if it differs from the first old provision
                var firstOld = oldProvisions.First();
                if (!currentProvision.ComparePropertiesValues(firstOld))
                {
                    completeList.Add(currentProvision);
                }

                // Add all old provisions
                completeList.AddRange(oldProvisions);

                // Remove all old provisions from histories
                histories.RemoveAll(x => x.Reference == currentProvision.Reference);
            }
            else
            {
                // If no old provisions found, add current provision directly
                completeList.Add(currentProvision);
            }
        }

        // Add remaining histories to complete list
        completeList.AddRange(histories);

        return completeList;
    }



    /// <inheritdoc/>
    public async Task<bool> AssignOwnershipAsync(Guid id, int? apiTraId, int assignToTraId, string correlationId)
    {

        var currentDtro = await _dtroDal.GetDtroByIdAsync(id);
        if (currentDtro is null)
        {
            throw new NotFoundException();
        }

        if (apiTraId != null)
        {
            var ownership = _dtroMappingService.GetOwnership(currentDtro);

            var isCreatorOrOwner = ownership.TrafficAuthorityCreatorId == apiTraId | ownership.TrafficAuthorityOwnerId == apiTraId;
            if (!isCreatorOrOwner)
            {
                throw new DtroValidationException($"Traffic authority {apiTraId} is not the creator or owner in the DTRO data submitted");
            }
        }

        DTROHistory historyDtro = _dtroMappingService.MapToDtroHistory(currentDtro);
        var isSaved = await _dtroHistoryDal.SaveDtroInHistoryTable(historyDtro);
        if (!isSaved)
        {
            throw new Exception("Failed to write to history table");
        }

        await _dtroDal.AssignDtroOwnership(id, assignToTraId, correlationId);

        return true;
    }
}