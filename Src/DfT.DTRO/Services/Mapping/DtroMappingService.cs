using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using DfT.DTRO.Enums;
using DfT.DTRO.Extensions;
using DfT.DTRO.Models.DataBase;
using DfT.DTRO.Models.DtroDtos;
using DfT.DTRO.Models.DtroEvent;
using DfT.DTRO.Models.DtroHistory;
using DfT.DTRO.Models.DtroJson;
using DfT.DTRO.Models.Search;
using DfT.DTRO.Services.Conversion;
using Microsoft.Extensions.Configuration;

namespace DfT.DTRO.Services.Mapping;

public class DtroMappingService : IDtroMappingService
{
    private readonly IConfiguration _configuration;
    private readonly ISpatialProjectionService _projectionService;

    public DtroMappingService(IConfiguration configuration, ISpatialProjectionService projectionService)
    {
        this._configuration = configuration;
        this._projectionService = projectionService;
    }

    public IEnumerable<DtroEvent> MapToEvents(IEnumerable<Models.DataBase.DTRO> dtros)
    {
        var results = new List<DtroEvent>();

        var baseUrl = _configuration.GetSection("SearchServiceUrl").Value;

        foreach (var dtro in dtros)
        {
            var periods = dtro.Data
                .GetValueOrDefault<IList<object>>("source.provision")
                .OfType<ExpandoObject>()
                .SelectMany(it => it.GetValueOrDefault<IList<object>>("regulations"))
                .Where(it => it is not null)
                .OfType<ExpandoObject>()
                .Select(it => it.GetExpando("overallPeriod"))
                .OfType<ExpandoObject>();

            var regulationStartTimes = periods.Select(it => it.GetValueOrDefault<DateTime?>("start")).Where(it => it is not null).Select(it => it.Value).ToList();
            var regulationEndTimes = periods.Select(it => it.GetValueOrDefault<DateTime?>("end")).Where(it => it is not null).Select(it => it.Value).ToList();

            results.Add(DtroEvent.FromCreation(dtro, baseUrl, regulationStartTimes, regulationEndTimes));

            if (dtro.Created != dtro.LastUpdated)
            {
                results.Add(DtroEvent.FromUpdate(dtro, baseUrl, regulationStartTimes, regulationEndTimes));
            }

            if (dtro.Deleted)
            {
                results.Add(DtroEvent.FromDeletion(dtro, baseUrl, regulationStartTimes, regulationEndTimes));
            }
        }

        results.Sort((x, y) => y.EventTime.CompareTo(x.EventTime));

        return results;
    }

    public DtroResponse MapToDtroResponse(Models.DataBase.DTRO dtro)
    {
        var result = new DtroResponse()
        {
            SchemaVersion = dtro.SchemaVersion,
            Data = dtro.Data
        };

        return result;
    }

    public IEnumerable<DtroSearchResult> MapToSearchResult(IEnumerable<Models.DataBase.DTRO> dtros)
    {
        var results = new List<DtroSearchResult>();

        var baseUrl = _configuration.GetSection("SearchServiceUrl").Value;

        foreach (var dtro in dtros)
        {
            var periods = dtro.Data
                .GetValueOrDefault<IList<object>>("source.provision")
                .OfType<ExpandoObject>()
                .SelectMany(it => it.GetValueOrDefault<IList<object>>("regulations"))
                .Where(it => it is not null)
                .OfType<ExpandoObject>()
                .Select(it => it.GetExpando("overallPeriod"))
                .OfType<ExpandoObject>();

            var regulationStartTimes = periods.Select(it => it.GetValueOrDefault<DateTime?>("start")).Where(it => it is not null).Select(it => it.Value).ToList();
            var regulationEndTimes = periods.Select(it => it.GetValueOrDefault<DateTime?>("end")).Where(it => it is not null).Select(it => it.Value).ToList();

            results.Add(CopyDtroToSearchResult(dtro, baseUrl, regulationStartTimes, regulationEndTimes));
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
        var sourceActionType = Get(dtroHistory, "source.actionType");
        var sourceReference = Get(dtroHistory, "source.reference");
        var sourceSection = Get(dtroHistory, "source.section");
        var sourceTroName = Get(dtroHistory, "source.troName");

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
        var traCreator = dtro.Data.GetValueOrDefault<int>("source.traCreator");
        var currentTraOwner = dtro.Data.GetValueOrDefault<int>("source.currentTraOwner");

        return new DtroOwner
        {
            TrafficAuthorityCreatorId = traCreator,
            TrafficAuthorityOwnerId = currentTraOwner,
        };
    }

    public void SetOwnership(ref Models.DataBase.DTRO dtro, int currentTraOwner)
    {
        dtro.Data.PutValue("source.currentTraOwner", currentTraOwner);
    }

    public void SetSourceActionType(ref Models.DataBase.DTRO dtro, SourceActionType sourceActionType)
    {
        ExpandoObject source = dtro.Data;
        var sourceDict = source as IDictionary<string, object>;
        if (sourceDict == null)
        {
            throw new ArgumentException("Source must be an ExpandoObject", nameof(source));
        }

        if (sourceDict.TryGetValue("source", out var sourceObject) && sourceObject is IDictionary<string, object> sourceDetails)
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
        IList<object> provisions = this.GetProvision(dtroHistory, "source.provision");
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
        List<ExpandoObject> regulations = dtro.Data.GetValueOrDefault<IList<object>>("source.provision")
            .OfType<ExpandoObject>()
            .SelectMany(it => it.GetValue<IList<object>>("regulations").OfType<ExpandoObject>())
        .ToList();

        dtro.TrafficAuthorityCreatorId = dtro.Data.GetExpando("source").HasField("traCreator")
            ? dtro.Data.GetValueOrDefault<int>("source.traCreator")
            : dtro.Data.GetValueOrDefault<int>("source.ha");

        dtro.TrafficAuthorityOwnerId = dtro.Data.GetExpando("source").HasField("currentTraOwner")
           ? dtro.Data.GetValueOrDefault<int>("source.currentTraOwner")
           : dtro.Data.GetValueOrDefault<int>("source.ha");

        dtro.TroName = dtro.Data.GetValueOrDefault<string>("source.troName");
        dtro.RegulationTypes = regulations.Select(it => it.GetValueOrDefault<string>("regulationType"))
            .Where(it => it is not null)
            .Distinct()
            .ToList();

        dtro.VehicleTypes = regulations.SelectMany(it => it.GetListOrDefault("conditions") ?? Enumerable.Empty<object>())
            .Where(it => it is not null)
            .OfType<ExpandoObject>()
            .Select(it => it.GetExpandoOrDefault("vehicleCharacteristics"))
            .Where(it => it is not null)
            .SelectMany(it => it.GetListOrDefault("vehicleType") ?? Enumerable.Empty<object>())
            .OfType<string>()
        .Distinct()
        .ToList();

        dtro.OrderReportingPoints = dtro.Data.GetValueOrDefault<IList<object>>("source.provision")
            .OfType<ExpandoObject>()
            .Select(it => it.GetValue<string>("orderReportingPoint"))
            .Distinct()
            .ToList();

        dtro.RegulationStart = regulations.Select(it => it.GetExpando("overallPeriod").GetDateTimeOrNull("start"))
            .Where(it => it is not null)
            .Min();
        dtro.RegulationEnd = regulations.Select(it => it.GetExpando("overallPeriod").GetDateTimeOrNull("end"))
            .Where(it => it is not null)
            .Max();

        IEnumerable<Coordinates> FlattenAndConvertCoordinates(ExpandoObject expandoObject, string crs)
        {
            var type = expandoObject.GetValue<string>("type");
            IList<object> coordinates = expandoObject.GetValue<IList<object>>("coordinates");

            IEnumerable<Coordinates> result = type switch
            {
                "Polygon" => coordinates
                    .OfType<IList<object>>()
                    .SelectMany(objects => objects)
                    .OfType<IList<object>>()
                    .Select(objects => new Coordinates((double)objects[0], (double)objects[1])),
                "LineString" => coordinates
                    .OfType<IList<object>>()
                    .Select(objects => new Coordinates((double)objects[0], (double)objects[1])),
                "Point" => new List<Coordinates> { new((double)coordinates[0], (double)coordinates[1]) },
                _ => throw new InvalidOperationException($"Coordinate type '{type}' unsupported.")
            };

            if (crs != "osgb36Epsg27700")
            {
                return result.Select(_projectionService.Wgs84ToOsgb36);
            }

            return result;
        }

        IEnumerable<Coordinates> coordinates = dtro
            .Data
            .GetValueOrDefault<IList<object>>("source.provision")
            .OfType<ExpandoObject>()
            .SelectMany(expandoObject => expandoObject.GetList("regulatedPlaces")
                .OfType<ExpandoObject>())
            .SelectMany(expandoObject => FlattenAndConvertCoordinates(
                expandoObject.GetExpando("geometry").GetExpando("coordinates"),
                expandoObject.GetExpando("geometry").GetValue<string>("crs")));

        dtro.Location = BoundingBox.Wrapping(coordinates);
    }

    private DtroSearchResult CopyDtroToSearchResult(Models.DataBase.DTRO dtro, string baseUrl, List<DateTime> regulationStartDates, List<DateTime> regulationEndDates)
    {
        return new DtroSearchResult
        {
            TroName = dtro.Data.GetValueOrDefault<string>("source.troName"),
            TrafficAuthorityCreatorId = dtro.Data.GetExpando("source").HasField("traCreator")
                ? dtro.Data.GetValueOrDefault<int>("source.traCreator")
                : dtro.Data.GetValueOrDefault<int>("source.ha"),
            TrafficAuthorityOwnerId = dtro.Data.GetExpando("source").HasField("currentTraOwner")
                ? dtro.Data.GetValueOrDefault<int>("source.currentTraOwner")
                : dtro.Data.GetValueOrDefault<int>("source.ha"),
            PublicationTime = dtro.Created.Value,
            RegulationType = dtro.RegulationTypes,
            VehicleType = dtro.VehicleTypes,
            OrderReportingPoint = dtro.OrderReportingPoints,
            RegulationStart = regulationStartDates,
            RegulationEnd = regulationEndDates,
            Id = dtro.Id,
        };
    }

    private string Get(DTROHistory request, string key) =>
        request.Data.GetValueOrDefault<string>(key);

    private IList<object> GetProvision(DTROHistory request, string key) =>
        request.Data.GetValueOrDefault<IList<object>>(key);
}
