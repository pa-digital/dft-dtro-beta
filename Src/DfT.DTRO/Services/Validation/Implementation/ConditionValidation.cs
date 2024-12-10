#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DfT.DTRO.Services.Validation.Implementation;

public class ConditionValidation : IConditionValidation
{
    public IList<SemanticValidationError> ValidateCondition(DtroSubmit dtroSubmit, SchemaVersion schemaVersion)
    {
        List<SemanticValidationError> errors = new();

        var regulations = dtroSubmit
            .Data
            .GetValueOrDefault<IList<object>>("Source.Provision".ToBackwardCompatibility(dtroSubmit.SchemaVersion))
            .OfType<ExpandoObject>()
            .SelectMany(provision => provision
                .GetValueOrDefault<IList<object>>("Regulation".ToBackwardCompatibility(dtroSubmit.SchemaVersion))
                .OfType<ExpandoObject>())
            .ToList();

        var hasConditionSet = regulations
            .Select(regulation => regulation.HasField("ConditionSet".ToBackwardCompatibility(dtroSubmit.SchemaVersion)))
            .FirstOrDefault();

        if (hasConditionSet)
        {
            var conditionSets = regulations
                .SelectMany(regulation => regulation
                    .GetValueOrDefault<IList<object>>("ConditionSet".ToBackwardCompatibility(dtroSubmit.SchemaVersion))
                    .OfType<ExpandoObject>())
                .ToList();

            var passedInOperator = conditionSets
                .Select(conditionSet => conditionSet.GetValueOrDefault<string>("operator"))
                .FirstOrDefault();

            var operatorTypes = typeof(OperatorType).GetDisplayNames<OperatorType>().ToList();
            var hasValidOperator = passedInOperator != null &&
                                   operatorTypes.Any(passedInOperator.Equals);

            if (!hasValidOperator)
            {
                SemanticValidationError error = new()
                {
                    Name = "Operator",
                    Message = "Operator is not present or incorrect",
                    Path = "Source -> Provision -> Regulation -> ConditionSet -> operator",
                    Rule = $"One of '{string.Join(", ", operatorTypes)}' operators must be present",
                };
                errors.Add(error);
            }

            List<KeyValuePair<string, object>> passedInConditions = new();

            foreach (var possibleCondition in PossibleConditions)
            {
                var hasCondition = conditionSets
                    .Select(conditionSet => conditionSet.HasField(possibleCondition))
                    .FirstOrDefault();

                if (!hasCondition)
                {
                    continue;
                }
                passedInConditions.AddRange(conditionSets
                    .Select(conditionSet =>
                        conditionSet
                            .GetValueOrDefault<IList<object>>(possibleCondition
                                .ToBackwardCompatibility(dtroSubmit.SchemaVersion))
                            .OfType<ExpandoObject>())
                    .SelectMany(expandoObjects => expandoObjects)
                    .SelectMany(expandoObject => expandoObject)
                    .Select(kv => kv)
                    .ToList());
            }

            passedInConditions = passedInConditions
                .Where(passedInCondition => passedInCondition.Key != "operator")
                .ToList();


            var conditionTypes = typeof(ConditionType).GetDisplayNames<ConditionType>().ToList();
            var areAllValidConditions = passedInConditions.All(passedInCondition => conditionTypes.Contains(passedInCondition.Key));

            if (!areAllValidConditions)
            {
                SemanticValidationError error = new()
                {
                    Name = "Invalid conditions",
                    Message = "One or more conditions are invalid",
                    Path = "Source -> Provision -> Regulation -> ConditionSet -> conditions",
                    Rule = $"One or more types of '{string.Join(", ", conditionTypes)}' conditions must be present",
                };
                errors.Add(error);
            }
        }
        else
        {
            var passedInConditions = regulations
                .Select(regulation => regulation
                    .GetValueOrDefault<IList<object>>("Condition".ToBackwardCompatibility(dtroSubmit.SchemaVersion))
                .OfType<ExpandoObject>())
                .SelectMany(expandoObjects => expandoObjects)
                .SelectMany(expandoObject => expandoObject)
                .Where(kv => kv.Key.IsPascalCase())
                .ToList();

            if (passedInConditions.Count > 1)
            {
                SemanticValidationError error = new()
                {
                    Name = "Condition",
                    Message = "Incorrect condition",
                    Path = "Source -> Provision -> Regulation -> Condition",
                    Rule = "Only one condition must be present",
                };
                errors.Add(error);
            }

            var conditionTypes = typeof(ConditionType).GetDisplayNames<ConditionType>().ToList();
            var areAllValidConditions = passedInConditions.All(passedInCondition => conditionTypes.Contains(passedInCondition.Key));

            if (!areAllValidConditions)
            {
                SemanticValidationError error = new()
                {
                    Name = "Condition",
                    Message = "Invalid condition",
                    Path = "Source -> Provision -> Regulation -> Condition",
                    Rule = $"One of '{string.Join(", ", conditionTypes)}' condition must be present",
                };
                errors.Add(error);
            }
        }

        return errors;
    }

    private static List<string> PossibleConditions => new() { "conditions", "Condition", "ConditionSet" };
}