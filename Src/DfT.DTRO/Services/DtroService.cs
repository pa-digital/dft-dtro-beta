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
using DfT.DTRO.DAL;
using DfT.DTRO.Models.DataBase;

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
    public async Task<bool> DeleteDtroAsync(Guid id, DateTime? deletionTime = null)
    {
        deletionTime ??= DateTime.UtcNow;
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


        DTROHistory historyDtro = _dtroMappingService.AsHistoryDtro(currentDtro);

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
    public async Task<List<DTROHistory>> GetDtroSourceHistoryAsync(string reference)
    {
        List<DTROHistory> dtroHistories = await _dtroHistoryDal.GetDtroSourceHistory(reference);

        List<DTROHistory> sourceHistories = _dtroMappingService.StripProvisions(dtroHistories);

        return sourceHistories;
    }

    /// <inheritdoc/>
    //public async Task UpdateDtroAsJsonAsync(Guid guid, DtroSubmit dtroSubmit, string correlationId)
    //{
    //    await _dtroDal.UpdateDtroAsJsonAsync(guid, dtroSubmit, correlationId);
    //}
}