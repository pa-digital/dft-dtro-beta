using System.Text.Json.Nodes;
using DfT.DTRO.DAL;
using DfT.DTRO.Extensions;
using DfT.DTRO.JsonLogic.CustomOperators;
using DfT.DTRO.Models.Filtering;
using DfT.DTRO.Models.Pagination;
using DfT.DTRO.Services.Mapping;

namespace DfT.DTRO.Services;

public class DtroService : IDtroService
{
    private readonly ISwaCodeDal _swaCodeDal;
    private readonly IDtroDal _dtroDal;
    private readonly IDtroHistoryDal _dtroHistoryDal;
    private readonly ISchemaTemplateDal _schemaTemplateDal;
    private readonly IDtroMappingService _dtroMappingService;
    private readonly IDtroGroupValidatorService _dtroGroupValidatorService;

    public DtroService(IDtroDal dtroDal, IDtroHistoryDal dtroHistoryDal, 
        ISchemaTemplateDal schemaTemplateDal, 
        IDtroMappingService dtroMappingService, 
        IDtroGroupValidatorService dtroGroupValidatorService,
        ISwaCodeDal swaCodeDal)
    {
        _swaCodeDal = swaCodeDal;
        _dtroDal = dtroDal;
        _dtroHistoryDal = dtroHistoryDal;
        _schemaTemplateDal = schemaTemplateDal;
        _dtroMappingService = dtroMappingService;
        _dtroGroupValidatorService = dtroGroupValidatorService;
    }

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

    public async Task<bool> DtroExistsAsync(Guid id)
    {
        return await _dtroDal.DtroExistsAsync(id);
    }

    public async Task<int> DtroCountForSchemaAsync(SchemaVersion schemaVersion)
    {
        return await _dtroDal.DtroCountForSchemaAsync(schemaVersion);
    }

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

    public async Task<GuidResponse> SaveDtroAsJsonAsync(DtroSubmit dtroSubmit, string correlationId, int? headerTa)
    {
        var validationErrors = await _dtroGroupValidatorService.ValidateDtro(dtroSubmit, headerTa);

        if (validationErrors is not null)
        {
            throw validationErrors;
        }

        return await _dtroDal.SaveDtroAsJsonAsync(dtroSubmit, correlationId);
    }

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

    public async Task<PaginatedResult<Models.DataBase.DTRO>> FindDtrosAsync(DtroSearch search)
    {
        return await _dtroDal.FindDtrosAsync(search);
    }

    public async Task<List<Models.DataBase.DTRO>> FindDtrosAsync(DtroEventSearch search)
    {
        return await _dtroDal.FindDtrosAsync(search);
    }

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

        foreach (var currentProvision in currentProvisions)
        {
            var oldProvisions = histories.Where(x => x.Reference == currentProvision.Reference).ToList();

            if (oldProvisions.Count > 0)
            {
                var firstOld = oldProvisions.First();
                if (!currentProvision.ComparePropertiesValues(firstOld))
                {
                    completeList.Add(currentProvision);
                }

                completeList.AddRange(oldProvisions);

                histories.RemoveAll(x => x.Reference == currentProvision.Reference);
            }
            else
            {
                completeList.Add(currentProvision);
            }
        }

        completeList.AddRange(histories);

        return completeList;
    }



    public async Task<bool> AssignOwnershipAsync(Guid id, int? apiTraId, int assignToTraId, string correlationId)
    {
        var swaList  = await _swaCodeDal.GetAllCodes();

        var found = swaList.FirstOrDefault(x => x.TraId == assignToTraId);
        if (found == null)
        {
            throw new NotFoundException($"Invalid -assign To TRA Id-: {assignToTraId} , the TRA does not exist");
        }

        var currentDtro = await _dtroDal.GetDtroByIdAsync(id);
        if (currentDtro is null)
        {
            throw new NotFoundException($"Invalid DTRO Id: {id}");
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