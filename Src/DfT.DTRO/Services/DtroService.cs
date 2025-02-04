using System.Data;
namespace DfT.DTRO.Services;

/// <inheritdoc cref="IDtroService"/>
public class DtroService : IDtroService
{
    private readonly IDtroUserDal _dtroUserDal;
    private readonly IDtroDal _dtroDal;
    private readonly IDtroHistoryDal _dtroHistoryDal;
    private readonly IDtroMappingService _dtroMappingService;
    private readonly IDtroGroupValidatorService _dtroGroupValidatorService;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="dtroUserDal">Digital Traffic Regulation Order User data access layer</param>
    /// <param name="dtroDal">Digital Traffic Regulation Order data access layer</param>
    /// <param name="dtroHistoryDal">Digital Traffic Regulation Order History data access layer</param>
    /// <param name="dtroMappingService">Digital Traffic Regulation Order mapping service</param>
    /// <param name="dtroGroupValidatorService">Digital Traffic Regulation Order validator service</param>
    public DtroService(
        IDtroUserDal dtroUserDal,
        IDtroDal dtroDal,
        IDtroHistoryDal dtroHistoryDal,
        IDtroMappingService dtroMappingService,
        IDtroGroupValidatorService dtroGroupValidatorService)
    {
        _dtroUserDal = dtroUserDal;
        _dtroDal = dtroDal;
        _dtroHistoryDal = dtroHistoryDal;
        _dtroMappingService = dtroMappingService;
        _dtroGroupValidatorService = dtroGroupValidatorService;
    }

    /// <inheritdoc cref="IDtroService"/>
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

    /// <inheritdoc cref="IDtroService"/>
    public async Task<bool> DtroExistsAsync(Guid id) => await _dtroDal.DtroExistsAsync(id);

    /// <inheritdoc cref="IDtroService"/>
    public async Task<int> DtroCountForSchemaAsync(SchemaVersion schemaVersion) =>
        await _dtroDal.DtroCountForSchemaAsync(schemaVersion);

    /// <inheritdoc cref="IDtroService"/>
    public async Task<IEnumerable<DtroResponse>> GetDtrosAsync(GetAllQueryParameters parameters)
    {
        var dtros = (await _dtroDal.GetDtrosAsync(parameters)).ToList();
        if (!dtros.Any())
        {
            throw new NotFoundException();
        }

        foreach (DigitalTrafficRegulationOrder dtro in dtros)
        {
            dtro.Created = (dtro.Created ?? DateTime.UtcNow).ToDateTimeTruncated();
            dtro.LastUpdated = dtro.LastUpdated?.ToDateTimeTruncated();
        }

        return dtros.Select(_dtroMappingService.MapToDtroResponse);
    }

    /// <inheritdoc cref="IDtroService"/>
    public async Task<DtroResponse> GetDtroByIdAsync(Guid id)
    {
        var digitalTrafficRegulationOrder = await _dtroDal.GetDtroByIdAsync(id);
        digitalTrafficRegulationOrder.Created = (digitalTrafficRegulationOrder.Created ?? DateTime.UtcNow).ToDateTimeTruncated();
        digitalTrafficRegulationOrder.LastUpdated = digitalTrafficRegulationOrder.LastUpdated?.ToDateTimeTruncated();
        if (digitalTrafficRegulationOrder is null)
        {
            throw new NotFoundException();
        }

        return _dtroMappingService.MapToDtroResponse(digitalTrafficRegulationOrder);
    }

    /// <inheritdoc cref="IDtroService"/>
    public async Task<GuidResponse> SaveDtroAsJsonAsync(DtroSubmit dtroSubmit, string correlationId, Guid xAppId)
    {
        var user = await _dtroUserDal.GetDtroUserOnAppIdAsync(xAppId);
        var errors = await _dtroGroupValidatorService.ValidateDtro(dtroSubmit, user.TraId);

        if (errors is not null)
        {
            throw errors;
        }

        return await _dtroDal.SaveDtroAsJsonAsync(dtroSubmit, correlationId);
    }

    /// <inheritdoc cref="IDtroService"/>
    public async Task<GuidResponse> TryUpdateDtroAsJsonAsync(Guid id, DtroSubmit dtroSubmit, string correlationId, Guid xAppId)
    {
        var user = await _dtroUserDal.GetDtroUserOnAppIdAsync(xAppId);
        var errors = await _dtroGroupValidatorService.ValidateDtro(dtroSubmit, user.TraId);

        if (errors is not null)
        {
            throw errors;
        }

        var dtroExists = await _dtroDal.DtroExistsAsync(id);
        if (!dtroExists)
        {
            throw new NotFoundException();
        }

        var currentDigitalTrafficRegulationOrder = await _dtroDal.GetDtroByIdAsync(id);

        var historyDigitalTrafficRegulationOrder = _dtroMappingService.MapToDtroHistory(currentDigitalTrafficRegulationOrder);

        var isSaved = await _dtroHistoryDal.SaveDtroInHistoryTable(historyDigitalTrafficRegulationOrder);
        if (!isSaved)
        {
            throw new DataException("Failed to write update to history table");
        }

        await _dtroDal.UpdateDtroAsJsonAsync(id, dtroSubmit, correlationId);
        return new GuidResponse { Id = id };
    }

    /// <inheritdoc cref="IDtroService"/>
    public async Task<PaginatedResult<DigitalTrafficRegulationOrder>> FindDtrosAsync(DtroSearch search)
        => await _dtroDal.FindDtrosAsync(search);

    /// <inheritdoc cref="IDtroService"/>
    public async Task<List<DigitalTrafficRegulationOrder>> FindDtrosAsync(DtroEventSearch search)
        => await _dtroDal.FindDtrosAsync(search);

    /// <inheritdoc cref="IDtroService"/>
    public async Task<List<DtroHistorySourceResponse>> GetDtroSourceHistoryAsync(Guid dtroId)
    {
        var dtroHistories = await _dtroHistoryDal.GetDtroHistory(dtroId);

        if (dtroHistories.Any())
        {
            throw new NotFoundException($"History for Dtro '{dtroId}' cannot be found.");
        }

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

    /// <inheritdoc cref="IDtroService"/>
    public async Task<List<DtroHistoryProvisionResponse>> GetDtroProvisionHistoryAsync(Guid dtroId)
    {
        var dtroHistories = await _dtroHistoryDal.GetDtroHistory(dtroId);

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

    /// <inheritdoc cref="IDtroService"/>
    public async Task<bool> AssignOwnershipAsync(Guid dtroId, Guid appId, Guid assignToUser, string correlationId)
    {
        var dtroUserList = await _dtroUserDal.GetAllDtroUsersAsync();

        var found = dtroUserList.FirstOrDefault(x => x.Id == assignToUser);
        if (found == null)
        {
            throw new NotFoundException($"Invalid -assign To Id-: {assignToUser} , the User does not exist");
        }

        var currentDtro = await _dtroDal.GetDtroByIdAsync(dtroId);
        var apiDtroUser = await _dtroUserDal.GetDtroUserOnAppIdAsync(appId);
        if (apiDtroUser.UserGroup != (int)UserGroup.Admin)
        {
            var ownership = _dtroMappingService.GetOwnership(currentDtro);
            var apiTraId = apiDtroUser.TraId;
            var isCreatorOrOwner = ownership.TrafficAuthorityCreatorId == apiTraId | ownership.TrafficAuthorityOwnerId == apiTraId;
            if (!isCreatorOrOwner)
            {
                throw new DtroValidationException($"Traffic authority {apiTraId} is not the creator or owner in the DigitalTrafficRegulationOrder data submitted");
            }
        }

        var historyDigitalTrafficRegulationOrder = _dtroMappingService.MapToDtroHistory(currentDtro);
        var isSaved = await _dtroHistoryDal.SaveDtroInHistoryTable(historyDigitalTrafficRegulationOrder);
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