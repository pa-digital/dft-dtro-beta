using System.Text.Json;
using DfT.DTRO.Models.Conditions;
using DfT.DTRO.Models.Conditions.Base;
using DfT.DTRO.Models.Conditions.ValueRules;
using DfT.DTRO.Services.Validation.Contracts;
using Newtonsoft.Json.Linq;

namespace DfT.DTRO.Services.Validation.Implementation;

public class SemanticValidationService : ISemanticValidationService
{
    private readonly ISystemClock _clock;
    private readonly IDtroDal _dtroDal;
    private readonly IConditionValidationService _conditionValidationService;
    private readonly IGeometryValidation _geometryValidation;
    private readonly LoggingExtension _loggingExtension;

    public SemanticValidationService(
        ISystemClock clock,
        IDtroDal dtroDal,
        IConditionValidationService conditionValidationService,
        IGeometryValidation geometryValidation,
        LoggingExtension loggingExtension)
    {
        _clock = clock;
        _dtroDal = dtroDal;
        _conditionValidationService = conditionValidationService;
        _geometryValidation = geometryValidation;
        _loggingExtension = loggingExtension;
    }

    public Task<Tuple<BoundingBox, List<SemanticValidationError>>> ValidateCreationRequest(DtroSubmit dtroSubmit) =>
        Validate(dtroSubmit.Data.ToIndentedJsonString(), dtroSubmit.SchemaVersion);

    private async Task<Tuple<BoundingBox, List<SemanticValidationError>>> Validate(
        string dtroDataString, SchemaVersion dtroSchemaVersion)
    {
        List<SemanticValidationError> validationErrors = new();
        JObject parsedBody = JObject.Parse(dtroDataString);

        ValidateLastUpdatedDate(parsedBody, dtroSchemaVersion, validationErrors);
        BoundingBox boundingBox = dtroSchemaVersion >= new SchemaVersion("3.3.0")
            ? _geometryValidation
                .ValidateGeometryAgainstCurrentSchemaVersion(parsedBody, validationErrors)
            : _geometryValidation
                .ValidateGeometryAgainstPreviousSchemaVersions(parsedBody, dtroSchemaVersion, validationErrors);

        await ValidateReferencedTroId(parsedBody, dtroSchemaVersion, validationErrors);
        ValidateConditions(parsedBody, validationErrors);

        return new Tuple<BoundingBox, List<SemanticValidationError>>(boundingBox, validationErrors);
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

        if (condition is VehicleCharacteristic vehicleCondition)
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

    private void ValidateConditions(JObject data, List<SemanticValidationError> errors)
    {
        var conditionSetArrays =
            data.DescendantsAndSelf()
                .OfType<JObject>()
                .Where(it => it.ContainsKey("conditionSet"))
                .Select(it => it.Value<JObject>());

        if (conditionSetArrays.Any())
        {
            return;
        }

        //foreach (var conditionSetArray in conditionSetArrays)
        //{
        //    var mappedConditionSet = JsonSerializer.Deserialize<List<ConditionSet>>(
        //        conditionSetArray.ToString(),
        //        new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        //    var conditionSet = ConditionSet.And(mappedConditionSet);

        //    var conditionErrors = _conditionValidationService.Validate(conditionSet);

        //    foreach (var error in conditionErrors)
        //    {
        //        error.Path = conditionSetArray.Path;
        //    }

        //    errors.AddRange(conditionErrors);
        //}

        var conditionArrays =
            data.DescendantsAndSelf()
                .OfType<JProperty>()
                .Where(it => it.Name == "condition")
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
        }
    }


    private void ValidateLastUpdatedDate(JObject data, SchemaVersion schemaVersion, List<SemanticValidationError> errors)
    {
        IEnumerable<JProperty> externalReferences = data
            .DescendantsAndSelf()
            .OfType<JProperty>()
            .Where(it => it.Name == "externalReference".ToBackwardCompatibility(schemaVersion))
            .Select(it => it);

        if (!externalReferences.Any())
        {
            errors.Add(new SemanticValidationError
            {
                Message = "value 'ExternalReference' cannot be found",
                Path = "Source -> Provision -> RegulatedPlace -> Geometry -> External Reference"
            });

            _loggingExtension.LogError(nameof(ValidateLastUpdatedDate), "", "ExternalReference error", string.Join(",", errors));
        }

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
                        Message = "value 'lastUpdateDate' cannot be in the future",
                        Path = lastUpdatedDateNode.Path
                    });
                _loggingExtension.LogError(nameof(ValidateLastUpdatedDate), "", "LastUpdatedDate error", string.Join(",", errors));
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
                        Message = $"Referenced TRO with id '{dtroId}' does not exist.",
                        Path = "source.reference"
                    });
            }
        }
    }
}