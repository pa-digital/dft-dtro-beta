namespace DfT.DTRO.Services.Validation.Implementation;

/// <summary>
/// <inheritdoc cref="IPermitConditionValidationService"/>
/// </summary>
public class PermitConditionValidationService : IPermitConditionValidationService
{
    /// <summary>
    /// <inheritdoc cref="IPermitConditionValidationService"/>
    /// </summary>
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

                    var permitConditions = conditions
                        .Select(condition => condition.GetExpandoOrDefault(Constants.PermitCondition))
                        .Where(condition => condition != null)
                        .ToList();

                    if (permitConditions.Any())
                    {
                        var permitTypes = permitConditions
                            .Select(permitCondition => 
                                permitCondition.GetValueOrDefault<string>(Constants.Type))
                            .Where(permitCondition => !string.IsNullOrEmpty(permitCondition))
                            .ToList();

                        if (!permitTypes.Any())
                            continue;

                        var areValidPermitTypes =
                            permitTypes.Any(permitType => Constants.PermitTypes.Contains(permitType));

                        if (!areValidPermitTypes)
                        {
                            SemanticValidationError newError = new()
                            {
                                Name = "Permit Type",
                                Message = $"Invalid 'Permit {Constants.Type}'",
                                Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.Regulation} -> {Constants.ConditionSet} -> {Constants.PermitCondition} -> {Constants.Type}",
                                Rule = $"One of '{string.Join(", ", Constants.PermitTypes)}' permit type must be present",
                            };
                            errors.Add(newError);
                        }

                        foreach (string permitType in permitTypes)
                        {
                            if (permitType.Equals(VehicleUsageType.Other.GetDisplayName()))
                            {
                                var permitTypeExtension = permitConditions
                                    .Select(permitType => permitType
                                            .GetExpandoOrDefault(Constants.PermitTypeExtension))
                                    .FirstOrDefault();

                                if (permitTypeExtension == null)
                                {
                                    SemanticValidationError newError = new()
                                    {
                                        Name = $"{Constants.PermitTypeExtension}",
                                        Message = $"Invalid '{Constants.PermitTypeExtension}'",
                                        Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.Regulation} -> {Constants.ConditionSet} -> {Constants.PermitCondition} -> {Constants.PermitTypeExtension}",
                                        Rule = $"If {PermitType.Other.GetDisplayName()} is present, the {Constants.PermitTypeExtension} must be present.",
                                    };
                                    errors.Add(newError);
                                }

                                var hasDefinition = permitTypeExtension.HasField(Constants.Definition);
                                if (!hasDefinition)
                                {
                                    SemanticValidationError newError = new()
                                    {
                                        Name = $"{Constants.Definition}",
                                        Message = $"Invalid '{Constants.Definition}'",
                                        Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.Regulation} -> {Constants.ConditionSet} -> {Constants.PermitCondition} -> {Constants.PermitTypeExtension} -> {Constants.Definition}",
                                        Rule = $"{Constants.Definition} must be present",
                                    };
                                    errors.Add(newError);
                                }

                                var hasEnumeratedList = permitTypeExtension
                                    .HasField(Constants.EnumeratedList);
                                if (!hasEnumeratedList)
                                {
                                    SemanticValidationError newError = new()
                                    {
                                        Name = $"{Constants.EnumeratedList}",
                                        Message = $"Invalid '{Constants.EnumeratedList}'",
                                        Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.Regulation} -> {Constants.ConditionSet} -> {Constants.PermitCondition} -> {Constants.PermitTypeExtension} -> {Constants.EnumeratedList}",
                                        Rule = $"{Constants.EnumeratedList} must be present",
                                    };
                                    errors.Add(newError);
                                }

                                var hasValue = permitTypeExtension
                                    .HasField(Constants.Value);
                                if (!hasValue)
                                {
                                    SemanticValidationError newError = new()
                                    {
                                        Name = $"{Constants.Value}",
                                        Message = $"Invalid '{Constants.Value}'",
                                        Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.Regulation} -> {Constants.ConditionSet} -> {Constants.PermitCondition} -> {Constants.PermitTypeExtension} -> {Constants.Value}",
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

                    var permitConditions = conditions
                        .Select(condition => condition.GetExpandoOrDefault(Constants.PermitCondition))
                        .Where(condition => condition != null)
                        .ToList();

                    if (permitConditions.Any())
                    {
                        var permitTypes = permitConditions
                            .Select(permitCondition => 
                                permitCondition.GetValueOrDefault<string>(Constants.Type))
                            .Where(condition => !string.IsNullOrEmpty(condition))
                            .ToList();

                        if (!permitTypes.Any())
                            continue;

                        var areValidPermitTypes = permitTypes
                            .Any(Constants.PermitTypes.Contains);

                        if (!areValidPermitTypes)
                        {
                            SemanticValidationError newError = new()
                            {
                                Name = "Permit Type",
                                Message = $"Invalid 'Permit {Constants.Type}'",
                                Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.Regulation} -> {Constants.ConditionSet} -> {Constants.PermitCondition}",
                                Rule = $"One of '{string.Join(", ", Constants.PermitTypes)}' permit type must be present",
                            };
                            errors.Add(newError);
                        }

                        foreach (string permitType in permitTypes)
                        {
                            if (permitType.Equals(PermitType.Other.GetDisplayName()))
                            {
                                var permitTypeDefinitions = permitConditions
                                    .Select(permitTypeDefinition => permitTypeDefinition
                                            .GetExpandoOrDefault(Constants.PermitTypeExtension))
                                    .FirstOrDefault();

                                if (permitTypeDefinitions == null)
                                {
                                    SemanticValidationError newError = new()
                                    {
                                        Name = $"{Constants.PermitTypeExtension}",
                                        Message = $"Invalid '{Constants.PermitTypeExtension}'",
                                        Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.Regulation} -> {Constants.ConditionSet} -> {Constants.PermitCondition} -> {Constants.PermitTypeExtension}",
                                        Rule = $"If {PermitType.Other.GetDisplayName()} is present, the {Constants.PermitTypeExtension} must be present.",
                                    };
                                    errors.Add(newError);
                                }

                                var hasDefinition = permitTypeDefinitions.HasField(Constants.Definition);
                                if (!hasDefinition)
                                {
                                    SemanticValidationError newError = new()
                                    {
                                        Name = $"{Constants.Definition}",
                                        Message = $"Invalid '{Constants.Definition}'",
                                        Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.Regulation} -> {Constants.ConditionSet} -> {Constants.PermitCondition} -> {Constants.PermitTypeExtension} -> {Constants.Definition}",
                                        Rule = $"{Constants.Definition} must be present",
                                    };
                                    errors.Add(newError);
                                }

                                var hasEnumeratedList = permitTypeDefinitions
                                    .HasField(Constants.EnumeratedList);
                                if (!hasEnumeratedList)
                                {
                                    SemanticValidationError newError = new()
                                    {
                                        Name = $"{Constants.EnumeratedList}",
                                        Message = $"Invalid '{Constants.EnumeratedList}'",
                                        Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.Regulation} -> {Constants.ConditionSet} -> {Constants.PermitCondition} -> {Constants.PermitTypeExtension} -> {Constants.EnumeratedList}",
                                        Rule = $"{Constants.EnumeratedList} must be present",
                                    };
                                    errors.Add(newError);
                                }

                                var hasValue = permitTypeDefinitions
                                    .HasField(Constants.Value);
                                if (!hasValue)
                                {
                                    SemanticValidationError newError = new()
                                    {
                                        Name = $"{Constants.Value}",
                                        Message = $"Invalid '{Constants.Value}'",
                                        Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.Regulation} -> {Constants.ConditionSet} -> {Constants.PermitCondition} -> {Constants.PermitTypeExtension} -> {Constants.Value}",
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