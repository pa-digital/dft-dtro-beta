using DfT.DTRO.Extensions;
using DfT.DTRO.Models.Conditions;
using DfT.DTRO.Models.Conditions.Base;
using DfT.DTRO.Models.Conditions.ValueRules;
using DfT.DTRO.Models.DtroDtos;
using DfT.DTRO.Models.DtroJson;
using DfT.DTRO.Models.SchemaTemplate;
using DfT.DTRO.Models.Validation;
using Microsoft.Extensions.Internal;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DfT.DTRO.Services.Validation;

/// <inheritdoc />
public class SemanticValidationService : ISemanticValidationService
{
    private readonly ISystemClock _clock;
    private readonly IDtroDal _dtroDal;
    private readonly IConditionValidationService _conditionValidationService;

    /// <summary>
    /// The default constructor.
    /// </summary>
    /// <param name="clock">An <see cref="ISystemClock"/> instance to facilitate testing.</param>
    /// <param name="dtroDal">An <see cref="IDtroService" /> instance.</param>
    /// <param name="conditionValidationService">An <see cref="IConditionValidationService"/> instance.</param>
    public SemanticValidationService(
        ISystemClock clock,
        IDtroDal dtroDal,
        IConditionValidationService conditionValidationService)
    {
        _clock = clock;
        _dtroDal = dtroDal;
        _conditionValidationService = conditionValidationService;
    }

    /// <inheritdoc />
    public Task<List<SemanticValidationError>> ValidateCreationRequest(DtroSubmit dtroSubmit)
    {
        return Validate(dtroSubmit.Data.ToIndentedJsonString(), dtroSubmit.SchemaVersion);
    }

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

    private static void ValidateCoordinatesAgainstBoundingBoxes(JObject data, List<SemanticValidationError> errors)
    {
        var geometries = data.DescendantsAndSelf().OfType<JProperty>().Where(p => p.Name == "geometry");

        foreach (var geometry in geometries)
        {
            if (geometry.Value is not JObject obj)
            {
                errors.Add(new SemanticValidationError
                {
                    Message = $"'geometry' was {geometry.Value.Type}, it must be an object."
                });

                continue;
            }

            if (!obj.TryGetValue("crs", out JToken token))
            {
                errors.Add(new SemanticValidationError { Message = "'crs' was missing, it must be a string." });

                continue;
            }

            if (token.Type == JTokenType.String && token.Value<string>() is string value)
            {
                if (value.ToLower() == "osgb36epsg27700")
                {
                    var coordinates = obj.DescendantsAndSelf().OfType<JProperty>()
                        .Where(p => p.Name == "coordinates").FirstOrDefault().Value as JObject;
                    ValidateGeometryAgainstBoundingBox(coordinates, errors, BoundingBox.ForOsgb36Epsg27700);
                }
                else if (value.ToLower() == "wgs84epsg4326")
                {
                    var coordinates = obj.DescendantsAndSelf().OfType<JProperty>()
                        .Where(p => p.Name == "coordinates").FirstOrDefault().Value as JObject;
                    ValidateGeometryAgainstBoundingBox(coordinates, errors, BoundingBox.ForWgs84Epsg4326);
                }
                else
                {
                    errors.Add(new SemanticValidationError
                    {
                        Message = $"'crs' was '{value}', it must be one of 'osgb36Epsg27700' or 'wgs84Epsg4326'.",
                        Path = token.Path
                    });
                }
            }
        }
    }

    private static void ValidateGeometryAgainstBoundingBox(
        JObject outerCoordinates,
        List<SemanticValidationError> errors,
        BoundingBox bbox)
    {
        JProperty coordinateSetFields = outerCoordinates.DescendantsAndSelf().OfType<JProperty>()
            .Where(p => p.Name == "coordinates").First();

        if (!outerCoordinates.TryGetValue("type", out JToken typeToken) || typeToken.Type != JTokenType.String)
        {
            errors.Add(
                new SemanticValidationError
                {
                    Message = $"'coordinates' must define a 'type' field of type {JTokenType.String}",
                    Path = outerCoordinates.Path
                });

            return;
        }

        var type = (string)typeToken;

        if (coordinateSetFields.Value is not JArray coordsArray)
        {
            errors.Add(
                new SemanticValidationError
                {
                    Message = $"'coordinates' must define a 'coordinates' field of type {JTokenType.Array}",
                    Path = coordinateSetFields.Path
                });

            return;
        }

        var coordinates = new List<JArray> { coordsArray }.AsEnumerable();

        if (type == "LineString")
        {
            coordinates = UnwrapArray(coordinates, errors);
        }
        else if (type == "Polygon")
        {
            coordinates = UnwrapArray(UnwrapArray(coordinates, errors), errors);
        }

        foreach (var array in coordinates)
        {
            var invalid = false;

            if (array.Count != 2)
            {
                errors.Add(
                    new SemanticValidationError
                    {
                        Message = $"Array contains {array.Count} elements - it must contain exactly 2.",
                        Path = array.Path,
                    });

                invalid = true;
            }

            foreach (var token in array)
            {
                if (token.Type != JTokenType.Integer && token.Type != JTokenType.Float)
                {
                    errors.Add(
                        new SemanticValidationError
                        {
                            Message = $"Coordinate arrays contains a token of type {token.Type}. " +
                                      $"Coordinate arrays must only contain numbers.",
                            Path = token.Path,
                        });

                    invalid = true;
                }
            }

            if (invalid)
            {
                continue;
            }

            var x = array[0].Value<double>();
            var y = array[1].Value<double>();

            if (!bbox.Contains(x, y, out var bboxErrors))
            {
                if (bboxErrors.LongitudeError is string longitudeError)
                {
                    errors.Add(
                        new SemanticValidationError { Message = longitudeError, Path = array.First.Path });
                }

                if (bboxErrors.LatitudeError is string latitudeError)
                {
                    errors.Add(
                        new SemanticValidationError { Message = latitudeError, Path = array.Last.Path });
                }
            }
        }
    }

    private static IEnumerable<JArray> UnwrapArray(
        IEnumerable<JArray> coordinateSetProp,
        List<SemanticValidationError> errors)
    {
        var nonArrayCoordinateSets = coordinateSetProp
            .SelectMany(it => it)
            .Where(it => it is not JArray);

        foreach (var nonArray in nonArrayCoordinateSets)
        {
            errors.Add(
                new SemanticValidationError
                {
                    Message = $"'coordinates' was {nonArray.Type} - it must be a {JTokenType.Array}",
                    Path = nonArray.Path,
                });
        }

        return coordinateSetProp
            .SelectMany(it => it)
            .OfType<JArray>();
    }

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

    private async Task<List<SemanticValidationError>> Validate(
        string dtroDataString,
        SchemaVersion dtroSchemaVersion)
    {
        var validationErrors = new List<SemanticValidationError>();
        var parsedBody = JObject.Parse(dtroDataString);

        ValidateLastUpdatedDate(parsedBody, validationErrors);
        ValidateCoordinatesAgainstBoundingBoxes(parsedBody, validationErrors);
        await ValidateReferencedTroId(parsedBody, dtroSchemaVersion, validationErrors);
        ValidateConditions(parsedBody, validationErrors);

        return validationErrors;
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