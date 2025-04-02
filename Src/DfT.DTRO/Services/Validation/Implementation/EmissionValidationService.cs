namespace DfT.DTRO.Services.Validation.Implementation;

/// <inheritdoc cref="IEmissionValidationService"/>
public class EmissionValidationService : IEmissionValidationService
{
    /// <inheritdoc cref="IEmissionValidationService"/>
    public List<SemanticValidationError> Validate(DtroSubmit dtroSubmit)
    {
         List<SemanticValidationError> errors = new();

        var regulations = dtroSubmit
            .Data
            .GetValueOrDefault<IList<object>>($"{Constants.Source}.{Constants.Provision}"
                .ToBackwardCompatibility(dtroSubmit.SchemaVersion))
            .OfType<ExpandoObject>()
            .SelectMany(provision => provision
                .GetValueOrDefault<IList<object>>(Constants.Regulation
                    .ToBackwardCompatibility(dtroSubmit.SchemaVersion))
                .OfType<ExpandoObject>())
            .ToList();

        foreach (var regulation in regulations)
        {
            var hasConditionSet = regulation.HasField(Constants.ConditionSet
                .ToBackwardCompatibility(dtroSubmit.SchemaVersion));
            if (hasConditionSet)
            {
                var conditionSets = regulation
                    .GetValueOrDefault<IList<object>>(Constants.ConditionSet
                        .ToBackwardCompatibility(dtroSubmit.SchemaVersion))
                    .OfType<ExpandoObject>()
                    .ToList();

                List<ExpandoObject> conditions = [];
                foreach (var conditionSet in conditionSets)
                {
                    foreach (var possibleCondition in Constants.PossibleConditions)
                    {
                        List<ExpandoObject> result = [];
                        var hasPossibleCondition = conditionSet.HasField(possibleCondition);
                        if (hasPossibleCondition)
                        {
                            result = conditionSet
                                .GetValueOrDefault<IList<object>>(possibleCondition
                                        .ToBackwardCompatibility(dtroSubmit.SchemaVersion))
                                .Cast<ExpandoObject>()
                                .ToList();
                        }

                        conditions.AddRange(result);
                    }
                }

                var vehicleCharacteristics = conditions
                    .Select(condition => condition.GetExpandoOrDefault(Constants.VehicleCharacteristics))
                    .Where(condition => condition != null)
                    .ToList();

                if (vehicleCharacteristics.Any())
                {
                    var emissions = vehicleCharacteristics
                        .Select(vehicleCharacteristic => vehicleCharacteristic.GetExpandoOrDefault(Constants.Emissions))
                        .Where(vehicleCharacteristic => vehicleCharacteristic != null)
                        .ToList();

                    var emissionClassificationEuros = emissions
                        .Select(emission => emission.GetValueOrDefault<string>(Constants.EmissionClassificationEuro))
                        .Where(emission => !string.IsNullOrEmpty(emission))
                        .ToList();

                    if (!emissionClassificationEuros.Any())
                        continue;

                    var areValidEmissionClassificationEuroType =
                        emissionClassificationEuros.Any(vehicleUsage => Constants.EmissionClassificationEuroTypes.Contains(vehicleUsage));

                    if (!areValidEmissionClassificationEuroType)
                    {
                        SemanticValidationError newError = new()
                        {
                            Name = "Emission Classification Euro",
                            Message = $"Invalid '{Constants.EmissionClassificationEuro}'",
                            Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.Regulation} -> {Constants.ConditionSet} -> {Constants.VehicleCharacteristics} -> {Constants.Emissions}",
                            Rule = $"One of '{string.Join(", ", Constants.EmissionClassificationEuroTypes)}' emission clasification euro type must be present",
                        };
                        errors.Add(newError);
                    }

                    foreach (string emissionClassificationEuro in emissionClassificationEuros)
                    {
                        if (emissionClassificationEuro.Equals(EmissionClassificationEuroType.Other.GetDisplayName()))
                        {
                            var emissionClassificationEuroTypeExtension = vehicleCharacteristics
                                .Select(vehicleCharacteristic => vehicleCharacteristic.GetExpandoOrDefault(Constants.Emissions))
                                .Select(emission => emission.GetExpandoOrDefault(Constants.EmissionClasificationEuroTypeExtension))
                                .FirstOrDefault();

                            if (emissionClassificationEuroTypeExtension == null)
                            {
                                SemanticValidationError newError = new()
                                {
                                    Name = $"{Constants.EmissionClasificationEuroTypeExtension}",
                                    Message = $"Invalid '{Constants.EmissionClasificationEuroTypeExtension}'",
                                    Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.Regulation} -> {Constants.ConditionSet} -> {Constants.VehicleCharacteristics} -> {Constants.Emissions} -> {Constants.EmissionClasificationEuroTypeExtension}",
                                    Rule = $"If {EmissionClassificationEuroType.Other.GetDisplayName()} is present, the {Constants.EmissionClasificationEuroTypeExtension} must be present.",
                                };
                                errors.Add(newError);
                                return errors;
                            }

                            var hasDefinition = emissionClassificationEuroTypeExtension
                                .HasField(Constants.Definition);
                            if (!hasDefinition)
                            {
                                SemanticValidationError newError = new()
                                {
                                    Name = $"{Constants.Definition}",
                                    Message = $"Invalid '{Constants.Definition}'",
                                    Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.Regulation} -> {Constants.ConditionSet} -> {Constants.VehicleCharacteristics} -> {Constants.VehicleUsageTypeExtension} -> {Constants.Definition}",
                                    Rule = $"{Constants.Definition} must be present",
                                };
                                errors.Add(newError);
                            }

                            var hasEnumeratedList = emissionClassificationEuroTypeExtension
                                .HasField(Constants.EnumeratedList);
                            if (!hasEnumeratedList)
                            {
                                SemanticValidationError newError = new()
                                {
                                    Name = $"{Constants.EnumeratedList}",
                                    Message = $"Invalid '{Constants.EnumeratedList}'",
                                    Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.Regulation} -> {Constants.ConditionSet} -> {Constants.VehicleCharacteristics} -> {Constants.VehicleUsageTypeExtension} -> {Constants.EnumeratedList}",
                                    Rule = $"{Constants.EnumeratedList} must be present",
                                };
                                errors.Add(newError);
                            }

                            var hasValue = emissionClassificationEuroTypeExtension
                                .HasField(Constants.Value);
                            if (!hasValue)
                            {
                                SemanticValidationError newError = new()
                                {
                                    Name = $"{Constants.Value}",
                                    Message = $"Invalid '{Constants.Value}'",
                                    Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.Regulation} -> {Constants.ConditionSet} -> {Constants.VehicleCharacteristics} -> {Constants.VehicleUsageTypeExtension} -> {Constants.Value}",
                                    Rule = $"{Constants.Value} must be present",
                                };
                                errors.Add(newError);
                            }
                        }
                    }
                }
            }
            else
            {
                var conditions = regulation
                    .GetValueOrDefault<IList<object>>(Constants.Condition
                        .ToBackwardCompatibility(dtroSubmit.SchemaVersion))
                    .OfType<ExpandoObject>()
                    .ToList();


                var vehicleCharacteristics = conditions
                    .Select(condition => condition.GetExpandoOrDefault(Constants.VehicleCharacteristics))
                    .Where(condition => condition != null)
                    .ToList();

                if (vehicleCharacteristics.Any())
                {
                    var emissions = vehicleCharacteristics
                        .Select(vehicleCharacteristic => vehicleCharacteristic.GetExpandoOrDefault(Constants.Emissions))
                        .Where(vehicleCharacteristic => vehicleCharacteristic != null)
                        .ToList();

                    var emissionClassificationEuros = emissions
                        .Select(emission => emission.GetValueOrDefault<string>(Constants.EmissionClassificationEuro))
                        .Where(emission => !string.IsNullOrEmpty(emission))
                        .ToList();

                    if (!emissionClassificationEuros.Any())
                        continue;

                    var areValidEmissionClassificationEuroType =
                        emissionClassificationEuros.Any(vehicleUsage => Constants.EmissionClassificationEuroTypes.Contains(vehicleUsage));

                    if (!areValidEmissionClassificationEuroType)
                    {
                        SemanticValidationError newError = new()
                        {
                            Name = "Emission Classification Euro",
                            Message = $"Invalid '{Constants.EmissionClassificationEuro}'",
                            Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.Regulation} -> {Constants.ConditionSet} -> {Constants.VehicleCharacteristics} -> {Constants.Emissions}",
                            Rule = $"One of '{string.Join(", ", Constants.EmissionClassificationEuroTypes)}' emission clasification euro type must be present",
                        };
                        errors.Add(newError);
                    }

                    foreach (string emissionClassificationEuro in emissionClassificationEuros)
                    {
                        if (emissionClassificationEuro.Equals(EmissionClassificationEuroType.Other.GetDisplayName()))
                        {
                            var emissionClassificationEuroTypeExtension = vehicleCharacteristics
                                .Select(vehicleCharacteristic => vehicleCharacteristic.GetExpandoOrDefault(Constants.Emissions))
                                .Select(emission => emission.GetExpandoOrDefault(Constants.EmissionClasificationEuroTypeExtension))
                                .FirstOrDefault();

                            if (emissionClassificationEuroTypeExtension == null)
                            {
                                SemanticValidationError newError = new()
                                {
                                    Name = $"{Constants.EmissionClasificationEuroTypeExtension}",
                                    Message = $"Invalid '{Constants.EmissionClasificationEuroTypeExtension}'",
                                    Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.Regulation} -> {Constants.ConditionSet} -> {Constants.VehicleCharacteristics} -> {Constants.Emissions} -> {Constants.EmissionClasificationEuroTypeExtension}",
                                    Rule = $"If {EmissionClassificationEuroType.Other.GetDisplayName()} is present, the {Constants.EmissionClasificationEuroTypeExtension} must be present.",
                                };
                                errors.Add(newError);
                            }

                            var hasDefinition = emissionClassificationEuroTypeExtension
                                .HasField(Constants.Definition);
                            if (!hasDefinition)
                            {
                                SemanticValidationError newError = new()
                                {
                                    Name = $"{Constants.Definition}",
                                    Message = $"Invalid '{Constants.Definition}'",
                                    Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.Regulation} -> {Constants.ConditionSet} -> {Constants.VehicleCharacteristics} -> {Constants.VehicleUsageTypeExtension} -> {Constants.Definition}",
                                    Rule = $"{Constants.Definition} must be present",
                                };
                                errors.Add(newError);
                            }

                            var hasEnumeratedList = emissionClassificationEuroTypeExtension
                                .HasField(Constants.EnumeratedList);
                            if (!hasEnumeratedList)
                            {
                                SemanticValidationError newError = new()
                                {
                                    Name = $"{Constants.EnumeratedList}",
                                    Message = $"Invalid '{Constants.EnumeratedList}'",
                                    Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.Regulation} -> {Constants.ConditionSet} -> {Constants.VehicleCharacteristics} -> {Constants.VehicleUsageTypeExtension} -> {Constants.EnumeratedList}",
                                    Rule = $"{Constants.EnumeratedList} must be present",
                                };
                                errors.Add(newError);
                            }

                            var hasValue = emissionClassificationEuroTypeExtension
                                .HasField(Constants.Value);
                            if (!hasValue)
                            {
                                SemanticValidationError newError = new()
                                {
                                    Name = $"{Constants.Value}",
                                    Message = $"Invalid '{Constants.Value}'",
                                    Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.Regulation} -> {Constants.ConditionSet} -> {Constants.VehicleCharacteristics} -> {Constants.VehicleUsageTypeExtension} -> {Constants.Value}",
                                    Rule = $"{Constants.Value} must be present",
                                };
                                errors.Add(newError);
                            }
                        }
                    }
                }
             }

        }

        return errors;
    }
}