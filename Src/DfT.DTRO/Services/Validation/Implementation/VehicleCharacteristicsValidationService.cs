﻿namespace DfT.DTRO.Services.Validation.Implementation;

/// <inheritdoc cref="IVehicleCharacteristicsValidationService"/>
public class VehicleCharacteristicsValidationService : IVehicleCharacteristicsValidationService
{
    /// <inheritdoc cref="IVehicleCharacteristicsValidationService"/>
    public List<SemanticValidationError> Validate(DtroSubmit dtroSubmit)
    {
        List<SemanticValidationError> errors = new();

        var regulations = dtroSubmit
            .Data
            .GetValueOrDefault<IList<object>>($"{Constants.Source}.{Constants.Provision}".ToBackwardCompatibility(dtroSubmit.SchemaVersion))
            .OfType<ExpandoObject>()
            .SelectMany(provision => provision
                .GetValueOrDefault<IList<object>>($"{Constants.Regulation}".ToBackwardCompatibility(dtroSubmit.SchemaVersion))
                .OfType<ExpandoObject>())
            .ToList();

        foreach (var regulation in regulations)
        {
            var hasConditionSet = regulation.HasField($"{Constants.ConditionSet}".ToBackwardCompatibility(dtroSubmit.SchemaVersion));
            if (hasConditionSet)
            {
                var conditionSets = regulation
                    .GetValueOrDefault<IList<object>>(
                        $"{Constants.ConditionSet}".ToBackwardCompatibility(dtroSubmit.SchemaVersion))
                    .OfType<ExpandoObject>()
                    .ToList();

                var vechicleCharacteristics = conditionSets
                    .Select(conditionSet => conditionSet.GetExpandoOrDefault(Constants.VehicleCharacteristics))
                    .ToList();

                if (vechicleCharacteristics.Any())
                {
                    var vehicleUsages = vechicleCharacteristics
                        .Select(vehicleCharacteristic => 
                            vehicleCharacteristic.GetValueOrDefault<string>(Constants.VehicleUsage))
                        .ToList();

                    var areValidVehicleUsageTypes = vehicleUsages
                        .Any(Constants.VehicleUsageTypes.Equals);

                    if (!areValidVehicleUsageTypes)
                    {
                        SemanticValidationError newError = new()
                        {
                            Name = "Vehicle Usage Type",
                            Message = $"Invalid '{Constants.VehicleUsageTypes}'",
                            Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.Regulation} -> {Constants.ConditionSet} -> {Constants.VehicleCharacteristics}",
                            Rule = $"One of '{string.Join(", ", Constants.VehicleUsageTypes)}' vehicle usage type must be present",
                        };
                        errors.Add(newError);
                    }

                    foreach (string vehicleUsage in vehicleUsages)
                    {
                        if (vehicleUsage.Equals(VehicleUsageType.Other.GetDisplayName()))
                        {
                            var vehicleUsageTypeExtension = vechicleCharacteristics
                                .Select(vehicleCharacteristic => vehicleCharacteristic
                                        .GetExpandoOrDefault(Constants.VehicleUsageTypeExtension))
                                .FirstOrDefault();

                            if (vehicleUsageTypeExtension == null)
                            {
                                SemanticValidationError newError = new()
                                {
                                    Name = $"{Constants.VehicleUsageTypeExtension}",
                                    Message = $"Invalid '{Constants.VehicleUsageTypeExtension}'",
                                    Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.Regulation} -> {Constants.ConditionSet} -> {Constants.VehicleCharacteristics} -> {Constants.VehicleUsageTypeExtension}",
                                    Rule = $"If {VehicleUsageType.Other.GetDisplayName()} is present, the {Constants.VehicleUsageTypeExtension} must be present.",
                                };
                                errors.Add(newError);
                            }

                            var hasDefinition = vehicleUsageTypeExtension
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

                            var hasEnumeratedList = vehicleUsageTypeExtension
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

                            var hasValue = vehicleUsageTypeExtension
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
                    .GetValueOrDefault<IList<object>>(
                        $"{Constants.Condition}".ToBackwardCompatibility(dtroSubmit.SchemaVersion))
                    .OfType<ExpandoObject>()
                    .ToList();

                var vechicleCharacteristics = conditions
                    .Select(condition => condition.GetExpandoOrDefault(Constants.VehicleCharacteristics))
                    .ToList();

                if (vechicleCharacteristics.Any())
                {
                    var vehicleUsages = vechicleCharacteristics
                        .Select(vehicleCharacteristic => 
                            vehicleCharacteristic.GetValueOrDefault<string>(Constants.VehicleUsage))
                        .ToList();

                    var areValidVehicleUsageTypes = vehicleUsages
                        .Any(Constants.VehicleUsageTypes.Equals);

                    if (!areValidVehicleUsageTypes)
                    {
                        SemanticValidationError newError = new()
                        {
                            Name = "Vehicle Usage Type",
                            Message = $"Invalid '{Constants.VehicleUsageTypes}'",
                            Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.Regulation} -> {Constants.ConditionSet} -> {Constants.VehicleCharacteristics}",
                            Rule = $"One of '{string.Join(", ", Constants.VehicleUsageTypes)}' vehicle usage type must be present",
                        };
                        errors.Add(newError);
                    }

                    foreach (string vehicleUsage in vehicleUsages)
                    {
                        if (vehicleUsage.Equals(VehicleUsageType.Other.GetDisplayName()))
                        {
                            var vehicleUsageTypeExtension = vechicleCharacteristics
                                .Select(vehicleCharacteristic => vehicleCharacteristic
                                        .GetExpandoOrDefault(Constants.VehicleUsageTypeExtension))
                                .FirstOrDefault();

                            if (vehicleUsageTypeExtension == null)
                            {
                                SemanticValidationError newError = new()
                                {
                                    Name = $"{Constants.VehicleUsageTypeExtension}",
                                    Message = $"Invalid '{Constants.VehicleUsageTypeExtension}'",
                                    Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.Regulation} -> {Constants.ConditionSet} -> {Constants.VehicleCharacteristics} -> {Constants.VehicleUsageTypeExtension}",
                                    Rule = $"If {VehicleUsageType.Other.GetDisplayName()} is present, the {Constants.VehicleUsageTypeExtension} must be present.",
                                };
                                errors.Add(newError);
                            }

                            var hasDefinition = vehicleUsageTypeExtension
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

                            var hasEnumeratedList = vehicleUsageTypeExtension
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

                            var hasValue = vehicleUsageTypeExtension
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