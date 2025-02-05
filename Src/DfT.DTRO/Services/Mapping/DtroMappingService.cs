using Newtonsoft.Json.Linq;

namespace DfT.DTRO.Services.Mapping;

/// <inheritdoc cref="IDtroMappingService"/>
public class DtroMappingService : IDtroMappingService
{
    private readonly IConfiguration _configuration;
    private readonly IBoundingBoxService _service;
    private readonly LoggingExtension _loggingExtension;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="service"></param>
    /// <param name="loggingExtension"></param>
    public DtroMappingService(IConfiguration configuration, IBoundingBoxService service, LoggingExtension loggingExtension)
    {
        _configuration = configuration;
        _service = service;
        _loggingExtension = loggingExtension;
    }

    /// <inheritdoc cref="IDtroMappingService"/>
    public IEnumerable<DtroEvent> MapToEvents(IEnumerable<Models.DataBase.DTRO> dtros, DateTime? searchSince)
    {
        var events = new List<DtroEvent>();

        var baseUrl = _configuration.GetSection("SearchServiceUrl").Value;

        foreach (var dtro in dtros)
        {
            var regulations = new List<ExpandoObject>();
            var provisions = dtro.Data.GetValueOrDefault<IList<object>>("Source.Provision".ToBackwardCompatibility(dtro.SchemaVersion)).OfType<ExpandoObject>().ToList();

            foreach (var provision in provisions)
            {
                regulations.AddRange(provision.GetValue<IList<object>>("Regulation".ToBackwardCompatibility(dtro.SchemaVersion)).OfType<ExpandoObject>().ToList());

            }

            List<ExpandoObject> timeValidity;
            try
            {
                timeValidity = regulations
                .SelectMany(it => it.GetListOrDefault("Condition".ToBackwardCompatibility(dtro.SchemaVersion)) ?? Enumerable.Empty<object>())
                .Where(it => it is not null)
                .OfType<ExpandoObject>()
                .Select(it => it.GetExpandoOrDefault("TimeValidity".ToBackwardCompatibility(dtro.SchemaVersion)))
                .Where(it => it is not null)
                .ToList();
            }
            catch (Exception ex)
            {
                _loggingExtension.LogError(nameof(InferIndexFields), "", "An error occurred while processing timeValidity.", ex.Message);
                timeValidity = new List<ExpandoObject>();
            }

            var regulationStartTimes = timeValidity
                .Select(it => it.GetValueOrDefault<DateTime?>("start"))
                .Where(it => it is not null)
                .Select(it => it.Value.ToLocalTime())
                .ToList();

            var regulationEndTimes = timeValidity
                .Select(it => it.GetValueOrDefault<DateTime?>("end"))
                .Where(it => it is not null)
                .Select(it => it.Value.ToLocalTime())
                .ToList();

            if (dtro.Created >= searchSince)
            {
                DtroEvent fromCreation = DtroEvent.FromCreation(dtro, baseUrl, regulationStartTimes, regulationEndTimes);
                events.Add(fromCreation);
            }

            if (dtro.Created != dtro.LastUpdated && dtro.LastUpdated >= searchSince)
            {
                DtroEvent fromUpdate = DtroEvent.FromUpdate(dtro, baseUrl, regulationStartTimes, regulationEndTimes);
                events.Add(fromUpdate);
            }

            if (dtro.Deleted && dtro.DeletionTime >= searchSince)
            {
                DtroEvent fromDeletion = DtroEvent.FromDeletion(dtro, baseUrl, regulationStartTimes, regulationEndTimes);
                events.Add(fromDeletion);
            }
        }

        events.Sort((x, y) => y.EventTime.CompareTo(x.EventTime));

        return events;
    }

    /// <inheritdoc cref="IDtroMappingService"/>
    public DtroResponse MapToDtroResponse(Models.DataBase.DTRO dtro) =>
        new()
        {
            Id = dtro.Id,
            SchemaVersion = dtro.SchemaVersion,
            Data = dtro.Data
        };

    /// <inheritdoc cref="IDtroMappingService"/>
    public IEnumerable<DtroSearchResult> MapToSearchResult(IEnumerable<Models.DataBase.DTRO> dtros)
    {
        List<DtroSearchResult> results = new();
        foreach (Models.DataBase.DTRO dtro in dtros)
        {
            var regulations = new List<ExpandoObject>();
            try
            {
                var provisions = dtro.Data.GetValueOrDefault<IList<object>>("Source.Provision".ToBackwardCompatibility(dtro.SchemaVersion));
                if (provisions == null)
                {
                    _loggingExtension.LogError(nameof(MapToSearchResult), "", "Error: 'Source.provision' is null or not found.", "");
                    continue;
                }

                var expandoProvisions = provisions.OfType<ExpandoObject>().ToList();

                foreach (var provision in expandoProvisions)
                {
                    var regulationList = provision.GetValue<IList<object>>("Regulation".ToBackwardCompatibility(dtro.SchemaVersion));
                    if (regulationList == null)
                    {
                        _loggingExtension.LogError(nameof(MapToSearchResult), "", "Error: 'regulation' not found in one of the provisions.", "");
                        continue;
                    }

                    var expandoRegulations = regulationList.OfType<ExpandoObject>();
                    regulations.AddRange(expandoRegulations);
                }

                foreach (var expandoProvision in expandoProvisions)
                {
                    var regulatedPlaceList =
                        expandoProvision.GetValueOrDefault<IList<object>>(
                            "RegulatedPlace".ToBackwardCompatibility(dtro.SchemaVersion));
                    if (regulatedPlaceList == null)
                    {
                        _loggingExtension.LogError(nameof(MapToSearchResult), "", "Error: 'regulatedPlace' not found in one of the provisions.", "");
                        continue;
                    }

                    var expandoRegulatedPlaces = regulatedPlaceList.OfType<ExpandoObject>();
                    regulations.AddRange(expandoRegulatedPlaces);
                }
            }
            catch (Exception ex)
            {
                _loggingExtension.LogError(nameof(MapToSearchResult), "", "An error occurred", ex.Message);
                throw new NotFoundException(ex.Message);
            }

            List<ExpandoObject> timeValidity;
            try
            {
                timeValidity = regulations
                .SelectMany(it => it.GetListOrDefault("Condition".ToBackwardCompatibility(dtro.SchemaVersion)) ?? Enumerable.Empty<object>())
                .Where(it => it is not null)
                .OfType<ExpandoObject>()
                .Select(it => it.GetExpandoOrDefault("TimeValidity".ToBackwardCompatibility(dtro.SchemaVersion)))
                .Where(it => it is not null)
                .ToList();
            }
            catch (Exception ex)
            {
                _loggingExtension.LogError(nameof(MapToSearchResult), "", "An error occurred while processing timeValidity.", ex.Message);
                timeValidity = new List<ExpandoObject>();
            }

            List<DateTime> regulationStartTimes = timeValidity
                .Select(it => it.GetValueOrDefault<string>("start"))
                .Where(it => it is not null)
                .Select(it => DateTime.Parse(it).ToLocalTime())
                .ToList();

            List<DateTime> regulationEndTimes = timeValidity
                .Select(it => it.GetValueOrDefault<string>("end"))
                .Where(it => it is not null)
                .Select(it => DateTime.Parse(it).ToLocalTime())
                .ToList();

            DtroSearchResult searchResult = CopyDtroToSearchResult(dtro, regulationStartTimes, regulationEndTimes);
            results.Add(searchResult);
        }

        return results;
    }

    /// <inheritdoc cref="IDtroMappingService"/>
    public DTROHistory MapToDtroHistory(Models.DataBase.DTRO currentDtro) =>
        new()
        {
            Id = Guid.NewGuid(),
            DtroId = currentDtro.Id,
            Created = currentDtro.Created,
            Data = currentDtro.Data,
            Deleted = currentDtro.Deleted,
            DeletionTime = currentDtro.DeletionTime,
            LastUpdated = DateTime.UtcNow,
            SchemaVersion = currentDtro.SchemaVersion,
            TrafficAuthorityCreatorId = currentDtro.TrafficAuthorityCreatorId,
            TrafficAuthorityOwnerId = currentDtro.TrafficAuthorityOwnerId
        };

    public DtroHistorySourceResponse GetSource(DTROHistory dtroHistory)
    {
        var sourceActionType = Get(dtroHistory, "Source.actionType");
        var sourceReference = Get(dtroHistory, "Source.reference");
        var sourceSection = Get(dtroHistory, "Source.section");
        var sourceTroName = Get(dtroHistory, "Source.troName");

        if (sourceActionType == SourceActionType.NoChange.GetDisplayName())
        {
            return null;
        }

        return new DtroHistorySourceResponse
        {
            ActionType = sourceActionType,
            Created = dtroHistory.Created,
            LastUpdated = dtroHistory.LastUpdated,
            Reference = sourceReference,
            SchemaVersion = dtroHistory.SchemaVersion.ToString(),
            Section = sourceSection,
            TrafficAuthorityCreatorId = dtroHistory.TrafficAuthorityCreatorId,
            TrafficAuthorityOwnerId = dtroHistory.TrafficAuthorityOwnerId,
            TroName = sourceTroName,
        };
    }

    /// <inheritdoc cref="IDtroMappingService"/>
    public DtroOwner GetOwnership(Models.DataBase.DTRO dtro)
    {
        var traCreator = dtro.Data.GetValueOrDefault<int>("Source.traCreator");
        var currentTraOwner = dtro.Data.GetValueOrDefault<int>("Source.currentTraOwner");

        return new DtroOwner
        {
            TrafficAuthorityCreatorId = traCreator,
            TrafficAuthorityOwnerId = currentTraOwner,
        };
    }

    /// <inheritdoc cref="IDtroMappingService"/>
    public void SetOwnership(ref Models.DataBase.DTRO dtro, int currentTraOwner)
    {
        dtro.Data.PutValue("Source.currentTraOwner", currentTraOwner);
    }

    /// <inheritdoc cref="IDtroMappingService"/>
    public void SetSourceActionType(ref Models.DataBase.DTRO dtro, SourceActionType sourceActionType)
    {
        ExpandoObject source = dtro.Data;
        var sourceDict = source as IDictionary<string, object>;
        if (sourceDict == null)
        {
            throw new ArgumentException("Source must be an ExpandoObject", nameof(source));
        }

        if (sourceDict.TryGetValue("Source", out var sourceObject) && sourceObject is IDictionary<string, object> sourceDetails)
        {
            sourceDetails["actionType"] = sourceActionType.GetDisplayName();

            if (sourceDetails.TryGetValue("Provision", out var provisionList) && provisionList is IEnumerable<object> provisions)
            {
                foreach (var provision in provisions)
                {
                    if (provision is IDictionary<string, object> provisionDict)
                    {
                        provisionDict["actionType"] = ProvisionActionType.NoChange.GetDisplayName();
                    }
                }
            }
        }
    }

    /// <inheritdoc cref="IDtroMappingService"/>
    public List<DtroHistoryProvisionResponse> GetProvision(DTROHistory dtroHistory)
    {
        IList<object> provisions = GetProvision(dtroHistory, "Source.Provision".ToBackwardCompatibility(dtroHistory.SchemaVersion));
        var ret = new List<DtroHistoryProvisionResponse>();
        foreach (var provision in provisions)
        {

            DtroHistoryProvisionResponse provisionResponse = new()
            {
                Created = dtroHistory.Created,
                Data = provision as ExpandoObject,
                LastUpdated = dtroHistory.LastUpdated,
            };
            provisionResponse.Reference = provisionResponse.Data?.GetValueOrDefault<string>("reference");
            provisionResponse.ActionType = provisionResponse.Data?.GetValueOrDefault<string>("actionType");
            provisionResponse.SchemaVersion = dtroHistory.SchemaVersion.ToString();
            if (provisionResponse.ActionType != ProvisionActionType.NoChange.GetDisplayName())
            {
                ret.Add(provisionResponse);
            }
        }

        return ret.OrderByDescending(x => x.Reference).ThenByDescending(x => x.LastUpdated).ToList();
    }

    /// <inheritdoc cref="IDtroMappingService"/>
    public void InferIndexFields(ref Models.DataBase.DTRO dtro)
    {
        var schemaVersion = dtro.SchemaVersion;
        var regulations = dtro
            .Data
            .GetValueOrDefault<IList<object>>("Source.Provision".ToBackwardCompatibility(schemaVersion))
            .OfType<ExpandoObject>()
            .SelectMany(expandoObject => expandoObject
                .GetValue<IList<object>>("Regulation".ToBackwardCompatibility(schemaVersion))
                .OfType<ExpandoObject>())
            .ToList();


        var regulatedPlaces = dtro
            .Data
            .GetValueOrDefault<IList<object>>("Source.Provision".ToBackwardCompatibility(schemaVersion))
            .OfType<ExpandoObject>()
            .SelectMany(expandoObject => expandoObject
                .GetValue<IList<object>>("RegulatedPlace".ToBackwardCompatibility(schemaVersion))
                .OfType<ExpandoObject>())
            .ToList();

        dtro.RegulatedPlaceTypes = regulatedPlaces
            .Select(it => it.GetValueOrDefault<string>("type"))
            .ToList();

        dtro.TrafficAuthorityCreatorId = dtro.Data.GetValueOrDefault<int>("Source.traCreator");

        dtro.TrafficAuthorityOwnerId = dtro.Data.GetValueOrDefault<int>("Source.currentTraOwner");

        dtro.TroName = dtro.Data.GetValueOrDefault<string>("Source.troName");

        dtro.RegulationTypes = regulations.Select(reg => reg.GetExpandoOrDefault("GeneralRegulation"))
            .Where(generalReg => generalReg is not null)
            .Select(generalReg => generalReg.GetValueOrDefault<string>("regulationType"))
            .Where(regType => regType is not null)
        .Distinct()
            .ToList();

        dtro.VehicleTypes = regulations.SelectMany(it => it.GetListOrDefault("Condition".ToBackwardCompatibility(schemaVersion)) ?? Enumerable.Empty<object>())
            .Where(it => it is not null)
            .OfType<ExpandoObject>()
            .Select(it => it.GetExpandoOrDefault("VehicleCharacteristics".ToBackwardCompatibility(schemaVersion)))
            .Where(it => it is not null)
            .SelectMany(it => it.GetListOrDefault("vehicleType") ?? Enumerable.Empty<object>())
            .OfType<string>()
        .Distinct()
        .ToList();

        dtro.OrderReportingPoints = dtro.Data.GetValueOrDefault<IList<object>>("Source.Provision".ToBackwardCompatibility(schemaVersion))
            .OfType<ExpandoObject>()
            .Select(it => it.GetValue<string>("orderReportingPoint"))
            .Distinct()
            .ToList();

        List<ExpandoObject> timeValidity;
        try
        {
            timeValidity = regulations
            .SelectMany(it => it.GetListOrDefault("Condition".ToBackwardCompatibility(schemaVersion)) ?? Enumerable.Empty<object>())
            .Where(it => it is not null)
            .OfType<ExpandoObject>()
            .Select(it => it.GetExpandoOrDefault("TimeValidity".ToBackwardCompatibility(schemaVersion)))
            .Where(it => it is not null)
            .ToList();
        }
        catch (Exception ex)
        {
            _loggingExtension.LogError(nameof(InferIndexFields), "", "An error occurred while processing timeValidity.", ex.Message);
            timeValidity = new List<ExpandoObject>();
        }

        dtro.RegulationStart = timeValidity
            .Select(it => it.GetValueOrDefault<string>("start"))
            .Where(it => it is not null)
            .Select(it => DateTime.Parse(it).ToDateTimeTruncated())
            .FirstOrDefault();

        dtro.RegulationEnd = timeValidity
            .Select(it => it.GetValueOrDefault<string>("end"))
            .Where(it => it is not null)
            .Select(it => DateTime.Parse(it).ToDateTimeTruncated())
            .FirstOrDefault();

        string json = dtro.Data.ToIndentedJsonString();
        JObject obj = JObject.Parse(json);

        if (dtro.SchemaVersion >= new SchemaVersion("3.3.0"))
        {
            var geometries = obj
                .Descendants()
                .OfType<JProperty>()
                .Where(property => Constants.ConcreteGeometries.Any(property.Name.Contains))
                .ToList();
            JProperty geometry = geometries.FirstOrDefault();

            dtro.Location =
                _service.SetBoundingBoxForMultipleGeometries(new List<SemanticValidationError>(), geometry,
                    new BoundingBox());
        }
        else
        {
            var geometry = obj
                .DescendantsAndSelf()
                .OfType<JProperty>()
                .FirstOrDefault(property => property.Name == "Geometry".ToBackwardCompatibility(schemaVersion));
            dtro.Location = _service.SetBoundingBoxForSingleGeometry(new List<SemanticValidationError>(), geometry, new BoundingBox());
        }
    }

    /// <inheritdoc cref="IDtroMappingService"/>
    private DtroSearchResult CopyDtroToSearchResult(Models.DataBase.DTRO dtro, List<DateTime> regulationStartDates, List<DateTime> regulationEndDates)
    {
        DtroSearchResult result = new()
        {
            TroName = dtro.Data.GetValueOrDefault<string>("Source.troName"),
            TrafficAuthorityCreatorId = dtro.Data.GetValueOrDefault<int>("Source.traCreator"),
            TrafficAuthorityOwnerId = dtro.Data.GetValueOrDefault<int>("Source.currentTraOwner"),
            PublicationTime = dtro.Created.Value.ToDateTimeTruncated(),
            RegulationType = dtro.RegulationTypes,
            RegulatedPlaceType = dtro.RegulatedPlaceTypes,
            VehicleType = dtro.VehicleTypes,
            OrderReportingPoint = dtro.OrderReportingPoints,
            RegulationStart = regulationStartDates,
            RegulationEnd = regulationEndDates,
            Id = dtro.Id
        };
        return result;
    }

    private string Get(DTROHistory request, string key) =>
        request.Data.GetValueOrDefault<string>(key);

    private IList<object> GetProvision(DTROHistory request, string key) =>
        request.Data.GetValueOrDefault<IList<object>>(key);

    private static void Debug_PrintdDtroData(ExpandoObject dtroData, string indent = "")
    {
        foreach (var kvp in (IDictionary<string, object>)dtroData)
        {
            if (kvp.Value is ExpandoObject)
            {
                Console.WriteLine($"{indent}{kvp.Key}:");
                Debug_PrintdDtroData((ExpandoObject)kvp.Value, indent + "  ");
            }
            else if (kvp.Value is IList<object> list)
            {
                Console.WriteLine($"{indent}{kvp.Key}:");
                foreach (var item in list)
                {
                    if (item is ExpandoObject)
                    {
                        Debug_PrintdDtroData((ExpandoObject)item, indent + "  ");
                    }
                    else
                    {
                        Console.WriteLine($"{indent}  - {item}");
                    }
                }
            }
            else
            {
                Console.WriteLine($"{indent}{kvp.Key}: {kvp.Value}");
            }
        }
    }
}
