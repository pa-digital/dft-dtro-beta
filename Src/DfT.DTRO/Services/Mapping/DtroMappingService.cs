using DfT.DTRO.Models.Validation;
using Newtonsoft.Json.Linq;

namespace DfT.DTRO.Services.Mapping;

public class DtroMappingService : IDtroMappingService
{
    private readonly IConfiguration _configuration;
    private readonly IBoundingBoxService _service;

    public DtroMappingService(IConfiguration configuration, IBoundingBoxService service)
    {
        _configuration = configuration;
        _service = service;
    }

    public IEnumerable<DtroEvent> MapToEvents(IEnumerable<Models.DataBase.DTRO> dtros)
    {
        var events = new List<DtroEvent>();

        var baseUrl = _configuration.GetSection("SearchServiceUrl").Value;

        foreach (var dtro in dtros)
        {
            var periods = dtro.Data
                .GetValueOrDefault<IList<object>>("Source.provision")
                .OfType<ExpandoObject>()
                .SelectMany(it => it.GetValueOrDefault<IList<object>>("regulation"))
                .Where(it => it is not null)
                .OfType<ExpandoObject>()
                .Select(it => it.GetExpando("timeValidity"))
                .OfType<ExpandoObject>();

            var regulationStartTimes = periods.Select(it => it.GetValueOrDefault<DateTime?>("start")).Where(it => it is not null).Select(it => it.Value).ToList();
            var regulationEndTimes = periods.Select(it => it.GetValueOrDefault<DateTime?>("end")).Where(it => it is not null).Select(it => it.Value).ToList();

            DtroEvent fromCreation = DtroEvent.FromCreation(dtro, baseUrl, regulationStartTimes, regulationEndTimes);
            events.Add(fromCreation);

            if (dtro.Created != dtro.LastUpdated)
            {
                DtroEvent fromUpdate = DtroEvent.FromUpdate(dtro, baseUrl, regulationStartTimes, regulationEndTimes);
                events.Add(fromUpdate);
            }

            if (dtro.Deleted)
            {
                DtroEvent fromDeletion = DtroEvent.FromDeletion(dtro, baseUrl, regulationStartTimes, regulationEndTimes);
                events.Add(fromDeletion);
            }
        }

        events.Sort((x, y) => y.EventTime.CompareTo(x.EventTime));

        return events;
    }

    public DtroResponse MapToDtroResponse(Models.DataBase.DTRO dtro) =>
        new()
        {
            Id = dtro.Id,
            SchemaVersion = dtro.SchemaVersion,
            Data = dtro.Data
        };

    public static void PrindDtroData(ExpandoObject dtroData, string indent = "")
    {
        var dtroDataDict = (IDictionary<string, object>)dtroData;
        foreach (var kvp in (IDictionary<string, object>)dtroData)
        {
            if (kvp.Value is ExpandoObject)
            {
                Console.WriteLine($"{indent}{kvp.Key}:");
                PrindDtroData((ExpandoObject)kvp.Value, indent + "  ");
            }
            else if (kvp.Value is IList<object> list)
            {
                Console.WriteLine($"{indent}{kvp.Key}:");
                foreach (var item in list)
                {
                    if (item is ExpandoObject)
                    {
                        PrindDtroData((ExpandoObject)item, indent + "  ");
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

    public IEnumerable<DtroSearchResult> MapToSearchResult(IEnumerable<Models.DataBase.DTRO> dtros)
    {
        List<DtroSearchResult> results = new List<DtroSearchResult>();
        foreach (Models.DataBase.DTRO dtro in dtros)
        {
            //            List<ExpandoObject> regulations = dtro.Data.GetValueOrDefault<IList<object>>("Source.provision")
            //                .OfType<ExpandoObject>()
            //                .SelectMany(it => it.GetValue<IList<object>>("regulation").OfType<ExpandoObject>())
            //                .ToList();
            var regulations = new List<ExpandoObject>();
            try
            {
                var provisions = dtro.Data.GetValueOrDefault<IList<object>>("Source.provision");
                if (provisions == null)
                {
                    Console.WriteLine("Error: 'Source.provision' is null or not found.");
                }
                Console.WriteLine("'Source.provision' found.");

                var expandoProvisions = provisions.OfType<ExpandoObject>();

                foreach (var provision in expandoProvisions)
                {
                    var regulationList = provision.GetValue<IList<object>>("regulation");
                    if (regulationList == null)
                    {
                        Console.WriteLine("Warning: 'regulation' not found in one of the provisions.");
                        continue;
                    }

                    var expandoRegulations = regulationList.OfType<ExpandoObject>();
                    regulations.AddRange(expandoRegulations);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Mapping regulations");
            foreach (ExpandoObject item in regulations)
            {
                var properties = item.GetType().GetProperties();
                Console.WriteLine(string.Join(", ", properties.Select(p => $"{p.Name}: {p.GetValue(item)}")));
            }

            List<ExpandoObject> timeValidity = regulations
                .SelectMany(it => it.GetListOrDefault("condition"))
                .Where(it => it is not null)
                .OfType<ExpandoObject>()
                .Select(it => it.GetExpandoOrDefault("timeValidity"))
                .Where(it => it is not null)
                .ToList();

            List<DateTime> regulationStartTimes = timeValidity
                .Select(it => it.GetValueOrDefault<string>("start"))
                .Select(it => DateTime.Parse(it).ToUniversalTime())
                .ToList();

            List<DateTime> regulationEndTimes = timeValidity
                .Select(it => it.GetValueOrDefault<string>("end"))
                .Select(it => DateTime.Parse(it).ToUniversalTime())
                .ToList();

            DtroSearchResult searchResult = CopyDtroToSearchResult(dtro, regulationStartTimes, regulationEndTimes);
            results.Add(searchResult);
        }

        return results;
    }

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

    public void SetOwnership(ref Models.DataBase.DTRO dtro, int currentTraOwner)
    {
        dtro.Data.PutValue("Source.currentTraOwner", currentTraOwner);
    }

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

            if (sourceDetails.TryGetValue("provision", out var provisionList) && provisionList is IEnumerable<object> provisions)
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

    public List<DtroHistoryProvisionResponse> GetProvision(DTROHistory dtroHistory)
    {
        IList<object> provisions = GetProvision(dtroHistory, "Source.provision");
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

    public void InferIndexFields(ref Models.DataBase.DTRO dtro)
    {
        List<ExpandoObject> regulations = dtro.Data.GetValueOrDefault<IList<object>>("Source.provision")
            .OfType<ExpandoObject>()
            .SelectMany(it => it.GetValue<IList<object>>("regulation").OfType<ExpandoObject>())
        .ToList();

        dtro.TrafficAuthorityCreatorId = dtro.Data.GetValueOrDefault<int>("Source.traCreator");

        dtro.TrafficAuthorityOwnerId = dtro.Data.GetValueOrDefault<int>("Source.currentTraOwner");

        dtro.TroName = dtro.Data.GetValueOrDefault<string>("Source.troName");
        dtro.RegulationTypes = regulations.Select(it => it.GetValueOrDefault<string>("regulationType"))
            .Where(it => it is not null)
            .Distinct()
            .ToList();

        dtro.VehicleTypes = regulations.SelectMany(it => it.GetListOrDefault("condition") ?? Enumerable.Empty<object>())
            .Where(it => it is not null)
            .OfType<ExpandoObject>()
            .Select(it => it.GetExpandoOrDefault("vehicleCharacteristics"))
            .Where(it => it is not null)
            .SelectMany(it => it.GetListOrDefault("vehicleType") ?? Enumerable.Empty<object>())
            .OfType<string>()
        .Distinct()
        .ToList();

        dtro.OrderReportingPoints = dtro.Data.GetValueOrDefault<IList<object>>("Source.provision")
            .OfType<ExpandoObject>()
            .Select(it => it.GetValue<string>("orderReportingPoint"))
            .Distinct()
            .ToList();

        List<ExpandoObject> timeValidity = regulations
            .SelectMany(it => it.GetListOrDefault("condition"))
            .Where(it => it is not null)
            .OfType<ExpandoObject>()
            .Select(it => it.GetExpandoOrDefault("timeValidity"))
            .Where(it => it is not null)
            .ToList();

        dtro.RegulationStart = timeValidity
            .Select(it => it.GetValueOrDefault<string>("start"))
            .Select(DateTime.Parse)
            .FirstOrDefault()
            .ToUniversalTime();

        dtro.RegulationEnd = timeValidity
            .Select(it => it.GetValueOrDefault<string>("end"))
            .Select(DateTime.Parse)
            .FirstOrDefault()
            .ToUniversalTime();

        string json = dtro.Data.ToIndentedJsonString();
        JObject obj = JObject.Parse(json);
        JProperty geometry = obj
            .DescendantsAndSelf()
            .OfType<JProperty>()
            .FirstOrDefault(property => property.Name == "geometry");
        JObject obj1 = geometry?.Value as JObject;

        dtro.Location = _service.SetBoundingBox(new List<SemanticValidationError>(), obj1, new BoundingBox());
    }

    private DtroSearchResult CopyDtroToSearchResult(Models.DataBase.DTRO dtro, List<DateTime> regulationStartDates, List<DateTime> regulationEndDates)
    {
        DtroSearchResult result = new()
        {
            TroName = dtro.Data.GetValueOrDefault<string>("Source.troName"),
            TrafficAuthorityCreatorId = dtro.Data.GetValueOrDefault<int>("Source.traCreator"),
            TrafficAuthorityOwnerId = dtro.Data.GetValueOrDefault<int>("Source.currentTraOwner"),
            PublicationTime = dtro.Created.Value,
            RegulationType = dtro.RegulationTypes,
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
}
