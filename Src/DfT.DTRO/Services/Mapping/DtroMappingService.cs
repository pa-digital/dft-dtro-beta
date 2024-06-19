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

/// <inheritdoc cref="IDtroMappingService"/>
public class DtroMappingService : IDtroMappingService
{
    private readonly IConfiguration _configuration;
    private readonly ISpatialProjectionService _projectionService;

    /// <summary>
    /// Initializes a new instance of the <see cref="DtroMappingService"/> class.
    /// </summary>
    /// <param name="configuration">An <see cref="IConfiguration"/> instance.</param>
    /// <param name="projectionService">An <see cref="ISpatialProjectionService"/> instance.</param>
    public DtroMappingService(IConfiguration configuration, ISpatialProjectionService projectionService)
    {
        _configuration = configuration;
        _projectionService = projectionService;
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public DtroResponse MapToDtroResponse(Models.DataBase.DTRO dtro)
    {
        var result = new DtroResponse()
        {
            SchemaVersion = dtro.SchemaVersion,
            Data = dtro.Data
        };

        return result;
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public DTROHistory MapToDtroHistory(Models.DataBase.DTRO currentDtro) =>
        new()
        {
            Id = Guid.NewGuid(),
            Created = currentDtro.Created,
            Data = currentDtro.Data,
            Deleted = currentDtro.Deleted,
            DeletionTime = currentDtro.DeletionTime,
            LastUpdated = DateTime.UtcNow,
            SchemaVersion = currentDtro.SchemaVersion
        };

    /// <inheritdoc/>
    public void UpdateDetails(Models.DataBase.DTRO currentDtro, DtroSubmit dtroSubmit)
    {
        Models.DataBase.DTRO dtro = new()
        {
            SchemaVersion = dtroSubmit.SchemaVersion,
            Created = currentDtro.Created,
            RegulationStart = currentDtro.RegulationStart,
            RegulationEnd = currentDtro.RegulationEnd,
            TrafficAuthorityCreatorId = currentDtro.TrafficAuthorityCreatorId,
            TroName = currentDtro.TroName,
            CreatedCorrelationId = currentDtro.CreatedCorrelationId,
            Deleted = currentDtro.Deleted,
            DeletionTime = currentDtro.DeletionTime,
            Data = dtroSubmit.Data,
            RegulationTypes = currentDtro.RegulationTypes,
            VehicleTypes = currentDtro.VehicleTypes,
            OrderReportingPoints = currentDtro.OrderReportingPoints,
            Location = currentDtro.Location
        };

        dtro.LastUpdatedCorrelationId = dtro.CreatedCorrelationId;
    }

    /// <inheritdoc />
    public DtroHistoryResponse StripProvision(DtroHistoryRequest request)
    {
        DtroHistoryResponse history = new();

        var sourceActionType = Get(request, "source.actionType");
        var sourceReference = Get(request, "source.reference");
        var sourceSection = Get(request, "source.section");
        var sourceTa = Get(request, "source.ta");
        var sourceTroName = Get(request, "source.troName");

        ExpandoObject expando = new();
        IDictionary<string, object> dictionary = expando;
        if (sourceActionType == SourceActionType.NoChange.GetDisplayName())
        {
            return null;
        }

        dictionary.Add("actionType", sourceActionType);
        dictionary.Add("reference", sourceReference);
        dictionary.Add("section", sourceSection);
        dictionary.Add("ta", sourceTa);
        dictionary.Add("troName", sourceTroName);

        history.Created = request.Created;
        history.Data = (ExpandoObject)dictionary;
        history.Deleted = request.Deleted;
        history.DeletionTime = request.DeletionTime;
        history.Id = request.Id;
        history.LastUpdated = request.LastUpdated;
        history.SchemaVersion = request.SchemaVersion;
        history.TrafficAuthorityCreatorId = request.TrafficAuthorityCreatorId;
        history.TrafficAuthorityOwnerId = request.TrafficAuthorityOwnerId;

        return history;

    }

    public DtroHistoryRequest MapToDtroRequest(DTROHistory dtroHistory) =>
        new()
        {
            Id = dtroHistory.Id,
            Created = dtroHistory.Created,
            Data = dtroHistory.Data,
            Deleted = dtroHistory.Deleted,
            DeletionTime = dtroHistory.DeletionTime,
            LastUpdated = dtroHistory.LastUpdated,
            SchemaVersion = dtroHistory.SchemaVersion
        };

    /// <inheritdoc/>
    public void InferIndexFields(ref Models.DataBase.DTRO dtro)
    {
        var regulations = dtro.Data.GetValueOrDefault<IList<object>>("source.provision")
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
                "Polygon" => coordinates.OfType<IList<object>>().SelectMany(objects => objects).OfType<IList<object>>()
                    .Select(objects => new Coordinates((double)objects[0], (double)objects[1])),
                "LineString" => coordinates.OfType<IList<object>>()
                    .Select(objects => new Coordinates((double)objects[0], (double)objects[1])),
                "Point" => new List<Coordinates> { new((double)coordinates[0], (double)coordinates[1]) },
                _ => throw new InvalidOperationException($"Coordinate type '{type}' unsupported.")
            };

            return crs != "osgb36Epsg27700"
                ? result.Select(_projectionService.Wgs84ToOsgb36)
            : result;
        }

        var coordinates = dtro.Data.GetValueOrDefault<IList<object>>("source.provision").OfType<ExpandoObject>()
            .SelectMany(it => it.GetList("regulatedPlaces").OfType<ExpandoObject>())
            .SelectMany(it => FlattenAndConvertCoordinates(
                it.GetExpando("geometry").GetExpando("coordinates"),
                it.GetExpando("geometry").GetValue<string>("crs")));

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
            Id = dtro.Id
        };
    }

    private string Get(DtroHistoryRequest request, string key) => request.Data.GetValueOrDefault<string>(key);
}
