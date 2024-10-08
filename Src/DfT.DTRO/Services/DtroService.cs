﻿namespace DfT.DTRO.Services;

public class DtroService : IDtroService
{
    private readonly IDtroUserDal _dtroUserDal;
    private readonly IDtroDal _dtroDal;
    private readonly IDtroHistoryDal _dtroHistoryDal;
    private readonly ISchemaTemplateDal _schemaTemplateDal;
    private readonly IDtroMappingService _dtroMappingService;
    private readonly IDtroGroupValidatorService _dtroGroupValidatorService;
    private readonly ILogger<IDtroService> _logger;

    public DtroService(IDtroDal dtroDal, IDtroHistoryDal dtroHistoryDal,
        ISchemaTemplateDal schemaTemplateDal,
        IDtroMappingService dtroMappingService,
        IDtroGroupValidatorService dtroGroupValidatorService,
        IDtroUserDal swaCodeDal, ILogger<IDtroService> logger)
    {
        _dtroUserDal = swaCodeDal;
        _dtroDal = dtroDal;
        _dtroHistoryDal = dtroHistoryDal;
        _schemaTemplateDal = schemaTemplateDal;
        _dtroMappingService = dtroMappingService;
        _dtroGroupValidatorService = dtroGroupValidatorService;
        _logger = logger;
    }

    public async Task<bool> DeleteDtroAsync(Guid dtroId, DateTime? deletionTime = null)
    {
        deletionTime ??= DateTime.UtcNow;

        var result = await _dtroDal.SoftDeleteDtroAsync(dtroId, deletionTime);
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

    public async Task<GuidResponse> SaveDtroAsJsonAsync(DtroSubmit dtroSubmit, string correlationId, Guid xAppId)
    {
        var apiDtroUser = await _dtroUserDal.GetDtroUserOnAppIdAsync(xAppId);
        var validationErrors = await _dtroGroupValidatorService.ValidateDtro(dtroSubmit, apiDtroUser.TraId);

        if (validationErrors is not null)
        {
            throw validationErrors;
        }

        return await _dtroDal.SaveDtroAsJsonAsync(dtroSubmit, correlationId);
    }

    public async Task<GuidResponse> TryUpdateDtroAsJsonAsync(Guid id, DtroSubmit dtroSubmit, string correlationId, Guid xAppId)
    {
        var apiDtroUser = await _dtroUserDal.GetDtroUserOnAppIdAsync(xAppId);
        var validationErrors = await _dtroGroupValidatorService.ValidateDtro(dtroSubmit, apiDtroUser.TraId);

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
        _logger.LogInformation($"Entering method: {nameof(FindDtrosAsync)}");
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


    public async Task<bool> AssignOwnershipAsync(Guid dtroId, Guid xAppId, Guid assignToUser, string correlationId)
    {
        var dtroUserList = await _dtroUserDal.GetAllDtroUsersAsync();

        var found = dtroUserList.FirstOrDefault(x => x.Id == assignToUser);
        if (found == null)
        {
            throw new NotFoundException($"Invalid -assign To Id-: {assignToUser} , the User does not exist");
        }

        var currentDtro = await _dtroDal.GetDtroByIdAsync(dtroId);
        if (currentDtro is null)
        {
            throw new NotFoundException($"Invalid DTRO Id: {dtroId}");
        }

        var apiDtroUser = await _dtroUserDal.GetDtroUserOnAppIdAsync(xAppId);





        if (apiDtroUser.UserGroup != (int)UserGroup.Admin)
        {
            var ownership = _dtroMappingService.GetOwnership(currentDtro);
            var apiTraId = apiDtroUser.TraId;
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
        var assign = await _dtroUserDal.GetDtroUserByIdAsync(assignToUser);

        if (assign == null)
        {
            throw new NotFoundException($"Invalid -assign To Id-: {assignToUser} , the User does not exist");
        }

        if (assign.TraId == null)
        {
            throw new NotFoundException($"Invalid -assign To Id-: {assignToUser} , the User does not have a Traffic Authority Id");
        }

        await _dtroDal.AssignDtroOwnership(dtroId, (int)assign.TraId, correlationId);

        return true;
    }
}