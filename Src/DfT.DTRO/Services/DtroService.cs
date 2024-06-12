using DfT.DTRO.Models.DtroDtos;
using DfT.DTRO.Models.DtroEvent;
using DfT.DTRO.Models.Errors;
using DfT.DTRO.Models.Filtering;
using DfT.DTRO.Models.Pagination;
using DfT.DTRO.Models.SchemaTemplate;
using DfT.DTRO.Models.SharedResponse;
using DfT.DTRO.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DfT.DTRO.Services;

/// <summary>
/// An implementation of <see cref="IDtroService"/>.
/// </summary>
public class DtroService : IDtroService
{
    private readonly IDtroDal _dtroDal;
    private readonly ISchemaTemplateDal _schemaTemplateDal;
    private readonly IDtroMappingService _dtroMappingService;
    private readonly IDtroGroupValidatorService _dtroGroupValidatorService;
    private readonly IDtroHistoryService _dtroHistoryService;

    /// <summary>
    /// The default constructor.
    /// </summary>
    /// <param name="dtroDal">An <see cref="IDtroDal"/> instance.</param>
    /// <param name="schemaTemplateDal">An <see cref="ISchemaTemplateDal"/> instance.</param>
    /// <param name="dtroMappingService">An <see cref="IDtroMappingService"/> instance.</param>
    /// <param name="dtroGroupValidatorService">An <see cref="IDtroGroupValidatorService"/> instance.</param>
    /// <param name="dtroHistoryService">An <see cref="IDtroHistoryService"/> instance.</param>
    public DtroService(IDtroDal dtroDal, ISchemaTemplateDal schemaTemplateDal, IDtroMappingService dtroMappingService, IDtroGroupValidatorService dtroGroupValidatorService, IDtroHistoryService dtroHistoryService)
    {
        _dtroDal = dtroDal;
        _schemaTemplateDal = schemaTemplateDal;
        _dtroMappingService = dtroMappingService;
        _dtroGroupValidatorService = dtroGroupValidatorService;
        _dtroHistoryService = dtroHistoryService;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteDtroAsync(Guid id, DateTime? deletionTime = null)
    {
        deletionTime ??= DateTime.UtcNow;
        var result = await _dtroDal.DeleteDtroAsync(id, deletionTime);
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
    public async Task<GuidResponse> SaveDtroAsJsonAsync(DtroSubmit dtroSubmit, string correlationId)
    {
        var validationErrors = await _dtroGroupValidatorService.ValidateDtro(dtroSubmit);

        if (validationErrors is not null)
        {
            throw validationErrors;
        }

        var schemaExists = await _schemaTemplateDal.SchemaTemplateExistsAsync(dtroSubmit.SchemaVersion);

        if (!schemaExists)
        {
            throw new NotFoundException("Schema Template not found");
        }

        return await _dtroDal.SaveDtroAsJsonAsync(dtroSubmit, correlationId);
    }

    /// <inheritdoc/>
    public async Task<GuidResponse> TryUpdateDtroAsJsonAsync(Guid guid, DtroSubmit dtroSubmit, string correlationId)
    {
        var validationErrors = await _dtroGroupValidatorService.ValidateDtro(dtroSubmit);

        if (validationErrors is not null)
        {
            throw validationErrors;
        }

        var dtroExists = await _dtroDal.DtroExistsAsync(guid);
        if (!dtroExists)
        {
            throw new NotFoundException();
        }

        var schemaExists = await _schemaTemplateDal.SchemaTemplateExistsAsync(dtroSubmit.SchemaVersion);

        if (!schemaExists)
        {
            throw new NotFoundException("Schema Template not found");
        }

        //DONE: Get the current DTRO
        Models.DataBase.DTRO currentDtro = await _dtroDal.GetDtroByIdAsync(guid);

        //DONE: Update the new DTRO (DTRO Submit) create date with current DTRO create date
        _dtroHistoryService.UpdateDetails(currentDtro, dtroSubmit);

        //DONE: Save current DTRO into the DtroHistory table
        var isSaved = await _dtroDal.SaveDtroAsJsonAsyncInHistoryTable(currentDtro);
        if (!isSaved)
        {
            throw new Exception();
        }

        ////TODO: Delete current DTRO from Dtro table - ERROR
        //var isDeleted = await _dtroDal.DeleteDtroAsync(guid, DateTime.Now);
        //if (!isDeleted)
        //{
        //    throw new Exception();
        //}

        //DONE: Save the new DTRO (DTRO Submit) to the Dtro table
        await _dtroDal.TryUpdateDtroAsJsonAsync(guid, dtroSubmit, correlationId);
        return new GuidResponse { Id = guid };
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

    /// <inheritdoc/>
    public async Task UpdateDtroAsJsonAsync(Guid guid, DtroSubmit dtroSubmit, string correlationId)
    {
        await _dtroDal.UpdateDtroAsJsonAsync(guid, dtroSubmit, correlationId);
    }
}