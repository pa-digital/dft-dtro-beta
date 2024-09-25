using System.Text.Json;
using DfT.DTRO.Models.Conditions;
using DfT.DTRO.Models.Conditions.Base;
using DfT.DTRO.Models.Conditions.ValueRules;
using DfT.DTRO.Models.Validation;
using Newtonsoft.Json.Linq;

namespace DfT.DTRO.Services.Validation;

public class SemanticValidationService : ISemanticValidationService
{
    private readonly ISystemClock _clock;
    private readonly IDtroDal _dtroDal;
    private readonly IConditionValidationService _conditionValidationService;
    private readonly IBoundingBoxService _boundingBoxService;

    public SemanticValidationService(
        ISystemClock clock,
        IDtroDal dtroDal,
        IConditionValidationService conditionValidationService,
        IBoundingBoxService boundingBoxService)
    {
        _clock = clock;
        _dtroDal = dtroDal;
        _conditionValidationService = conditionValidationService;
        _boundingBoxService = boundingBoxService;
    }

    public Task<Tuple<BoundingBox, List<SemanticValidationError>>> ValidateCreationRequest(DtroSubmit dtroSubmit) =>
        Validate(dtroSubmit.Data.ToIndentedJsonString(), dtroSubmit.SchemaVersion);

    private static IEnumerable<IValueRule> GetValueRules(Condition condition)
    {
        if (condition is ConditionSet conditionSet)
        {
            return conditionSet.SelectMany(GetValueRules);
        }

        if (condition is DriverCondition driverCondition)
        {
            return new List<IValueRule> { driverCondition.AgeOfDriver, driverCondition.TimeDriversLicenseHeld }.Where(
                it => it is not null);
        }

        if (condition is VehicleCondition vehicleCondition)
        {
            return new List<IValueRule>
            {
                vehicleCondition.VehicleCharacteristics?.GrossWeightCharacteristic,
                vehicleCondition.VehicleCharacteristics?.WidthCharacteristic,
                vehicleCondition.VehicleCharacteristics?.HeightCharacteristic,
                vehicleCondition.VehicleCharacteristics?.LengthCharacteristic,
                vehicleCondition.VehicleCharacteristics?.HeaviestAxleWeightCharacteristic,
                vehicleCondition.VehicleCharacteristics?.NumberOfAxlesCharacteristic
            }.Where(it => it is not null);
        }

        if (condition is OccupantCondition occupantCondition)
        {
            return new List<IValueRule> { occupantCondition.NumbersOfOccupants };
        }

        return new List<IValueRule>();
    }

    private static void ValidateRangeConditions(Condition condition, List<SemanticValidationError> errors, string path)
    {
        var rules = GetValueRules(condition);

        foreach (var andRule in rules.OfType<IAndRule>())
        {
            var lowerBound =
                andRule.First is ILessThanRule ltr1 ? ltr1
                : andRule.Second is ILessThanRule ltr2 ? ltr2
                : null;

            var upperBound =
                andRule.First is IMoreThanRule mtr1 ? mtr1
                : andRule.Second is IMoreThanRule mtr2 ? mtr2
                : null;

            if (lowerBound is null || upperBound is null)
            {
                errors.Add(new SemanticValidationError()
                {
                    Message = "The value condition must represent a single value or a valid range.",
                    Path = path
                });

                continue;
            }

            if (andRule.First.Contradicts(andRule.Second))
            {
                errors.Add(new SemanticValidationError { Message = "The condition contradicts itself", Path = path });
            }
        }
    }

    public BoundingBox ValidateCoordinatesAgainstBoundingBoxes(JObject data, List<SemanticValidationError> errors)
    {
        JProperty geometry = data
            .DescendantsAndSelf()
            .OfType<JProperty>()
            .FirstOrDefault(property => property.Name == "geometry");

        BoundingBox boundingBox = new();
        if (geometry?.Value is not JObject)
        {
            errors.Add(new SemanticValidationError
            {
                Message = $"'geometry' was {geometry?.Value.Type}, it must be an object."
            });
        }

        JObject obj1 = geometry?.Value as JObject;

        if (!obj1.TryGetValue("version", out JToken _))
        {
            errors.Add(new SemanticValidationError
            {
                Message = "'version' was missing, it must be a number."
            });
        }

        return _boundingBoxService.SetBoundingBox(errors, obj1, boundingBox);
    }

    #region Remove the code
    //private static BoundingBox ValidateLinearGeometryAgainstBoundingBox(IEnumerable<JToken> values, GeometryType geometryType)
    //{
    //    List<Coordinates> coordinates = new();

    //    List<string> points = values
    //        .Value<string>()
    //        .Split(" ")
    //        .Select(it => it
    //            .Replace("(", "")
    //            .Replace(")", "")
    //            .Replace(",", ""))
    //        .ToList();

    //    for (int index = 0; index < points.Count; index++)
    //    {
    //        Coordinates coordinate = new();
    //        coordinate.Longitude = points[index].AsInt();
    //        index++;
    //        coordinate.Latitude = points[index].AsInt();

    //        coordinates.Add(coordinate);
    //    }

    //    return BoundingBox.Wrapping(coordinates);
    //}

    //private static BoundingBox ValidatePolygonAgainstBoundingBox(IEnumerable<JToken> values, GeometryType geometryType)
    //{
    //    List<Coordinates> coordinates = new();

    //    List<string> points = values
    //        .Value<string>()
    //        .Split(" ")
    //        .Select(it => it
    //            .Replace("(", "")
    //            .Replace(")", "")
    //            .Replace(",", ""))
    //        .ToList();

    //    for (int index = 0; index < points.Count; index++)
    //    {
    //        Coordinates coordinate = new();
    //        coordinate.Longitude = points[index].AsInt();
    //        index++;
    //        coordinate.Latitude = points[index].AsInt();

    //        coordinates.Add(coordinate);
    //    }

    //    return BoundingBox.Wrapping(coordinates);
    //}

    //private static BoundingBox ValidateDirectedLinearAgainstBoundingBox(IEnumerable<JToken> values, GeometryType geometryType)
    //{
    //    List<Coordinates> coordinates = new();

    //    List<string> points = values
    //        .Value<string>()
    //        .Split(" ")
    //        .Select(it => it
    //            .Replace("(", "")
    //            .Replace(")", "")
    //            .Replace(",", ""))
    //        .ToList();

    //    for (int index = 0; index < points.Count; index++)
    //    {
    //        Coordinates coordinate = new();
    //        coordinate.Longitude = points[index].AsInt();
    //        index++;
    //        coordinate.Latitude = points[index].AsInt();

    //        coordinates.Add(coordinate);
    //    }

    //    return BoundingBox.Wrapping(coordinates);
    //}
    #endregion

    private void ValidateConditions(JObject data, List<SemanticValidationError> errors)
    {
        var conditionArrays =
            data.DescendantsAndSelf()
                .OfType<JProperty>()
                .Where(it => it.Name == "conditions")
                .Select(it => it.Value)
                .Cast<JArray>();

        foreach (var conditionArray in conditionArrays)
        {
            var mappedConditions =
                JsonSerializer.Deserialize<List<Condition>>(
                    conditionArray.ToString(),
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            var rangeConditionErrors = new List<SemanticValidationError>();

            foreach (var condition in mappedConditions)
            {
                ValidateRangeConditions(condition, rangeConditionErrors, conditionArray.Path);
            }

            errors.AddRange(rangeConditionErrors);

            if (rangeConditionErrors.Any())
            {
                return;
            }

            if (mappedConditions.Count < 2)
            {
                continue;
            }

            var conditionSet = ConditionSet.And(mappedConditions);

            var conditionErrors = _conditionValidationService.Validate(conditionSet);

            foreach (var error in conditionErrors)
            {
                error.Path = conditionArray.Path;
            }

            errors.AddRange(
                conditionErrors);
        }
    }

    private async Task<Tuple<BoundingBox, List<SemanticValidationError>>> Validate(
        string dtroDataString,
        SchemaVersion dtroSchemaVersion)
    {
        List<SemanticValidationError> validationErrors = new();
        JObject parsedBody = JObject.Parse(dtroDataString);

        ValidateLastUpdatedDate(parsedBody, validationErrors);
        BoundingBox boundingBox = ValidateCoordinatesAgainstBoundingBoxes(parsedBody, validationErrors);
        await ValidateReferencedTroId(parsedBody, dtroSchemaVersion, validationErrors);
        ValidateConditions(parsedBody, validationErrors);

        return new Tuple<BoundingBox, List<SemanticValidationError>>(boundingBox, validationErrors);
    }

    private void ValidateLastUpdatedDate(JObject data, List<SemanticValidationError> errors)
    {
        IEnumerable<JProperty> lastUpdatedDateNodes = data.DescendantsAndSelf().OfType<JProperty>()
            .Where(p => p.Name == "lastUpdateDate")
            .Select(p => p);

        foreach (var lastUpdatedDateNode in lastUpdatedDateNodes)
        {
            var dateTime = DateTime.Parse(lastUpdatedDateNode.Value.ToString()).ToUniversalTime();
            if (dateTime > _clock.UtcNow)
            {
                errors.Add(
                    new SemanticValidationError
                    {
                        Message = "lastUpdateDate cannot be in the future",
                        Path = lastUpdatedDateNode.Path
                    });
            }
        }
    }

    private async Task ValidateReferencedTroId(
        JObject data,
        SchemaVersion dtroSchemaVersion,
        List<SemanticValidationError> errors)
    {
        if (dtroSchemaVersion < "3.1.2")
        {
            return;
        }

        var referencedDtroIds =
            (data["source"]?["crossRefTro"] as JArray ?? new JArray())
            .Select(id => new Guid((string)id));

        foreach (Guid dtroId in referencedDtroIds)
        {
            if (!await _dtroDal.DtroExistsAsync(dtroId))
            {
                errors.Add(
                    new SemanticValidationError
                    {
                        Message = $"Referenced D-TRO with id {dtroId} does not exist.",
                        Path = "source.crossRefTro"
                    });
            }
        }
    }
}