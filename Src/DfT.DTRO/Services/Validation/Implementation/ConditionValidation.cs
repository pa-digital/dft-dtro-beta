using DfT.DTRO.Services.Validation.Contracts;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DfT.DTRO.Services.Validation.Implementation;

public class ConditionValidation : IConditionValidation
{
    public IList<SemanticValidationError> ValidateCondition(DtroSubmit dtroSubmit, SchemaVersion schemaVersion)
    {
        List<SemanticValidationError> errors = new();

        if (schemaVersion < new SchemaVersion("3.3.0"))
        {
            return errors;
        }

        var regulations = dtroSubmit
            .Data
            .GetValueOrDefault<IList<object>>("Source.provision")
            .OfType<ExpandoObject>()
            .SelectMany(provision => provision
                .GetValueOrDefault<IList<object>>("regulation")
                .OfType<ExpandoObject>())
            .ToList();

        var hasConditionSet = regulations
            .Select(regulation => regulation.HasField("conditionSet"))
            .FirstOrDefault();

        if (hasConditionSet)
        {
            var conditionSets = regulations
                .SelectMany(regulation => regulation
                    .GetValueOrDefault<IList<object>>("conditionSet")
                    .OfType<ExpandoObject>())
                .ToList();

            var passedInOperator = conditionSets
                .Select(conditionSet => conditionSet.GetValueOrDefault<string>("operator"))
                .FirstOrDefault();

            var operatorTypes = typeof(OperatorType).GetDisplayNames<OperatorType>().ToList();
            var hasValidOperator = passedInOperator != null &&
                                   operatorTypes
                                       .Select(passedInOperator.Contains)
                                       .FirstOrDefault();

            if (!hasValidOperator)
            {
                SemanticValidationError error = new()
                {
                    Name = "Operator",
                    Message = "Operator is not present or incorrect.",
                    Path = "Source -> Provision -> Regulation -> ConditionSet -> operator",
                    Rule = $"One or more of '{string.Join(", ", operatorTypes)}' operators must be present.",
                };
                errors.Add(error);
            }

            var passedInConditions = conditionSets
                .Select(conditionSet => conditionSet
                    .GetValueOrDefault<object>("condition"))
                .OfType<ExpandoObject>()
                .SelectMany(it => it)
                .Select(kv => kv.Key)
                .ToList();

            var conditionTypes = typeof(ConditionType).GetDisplayNames<ConditionType>().ToList();
            var areAllValidConditions = passedInConditions.All(conditionTypes.Contains);

            if (!areAllValidConditions)
            {
                SemanticValidationError error = new()
                {
                    Name = "Invalid conditions",
                    Message = "One or more conditions are invalid",
                    Path = "Source -> Provision -> Regulation -> ConditionSet -> Condition",
                    Rule = $"One or more type of '{string.Join(", ", typeof(ConditionType))}' must be present",
                };
                errors.Add(error);
            }
        }
        else
        {
            var passedInConditions = regulations
                .Select(conditionSet => conditionSet
                    .GetValueOrDefault<object>("condition"))
                .OfType<ExpandoObject>()
                .SelectMany(it => it)
                .Select(kv => kv.Key)
                .ToList();

            if (passedInConditions.Count > 1)
            {
                SemanticValidationError error = new()
                {
                    Name = "Condition",
                    Message = "Maximum number of conditions",
                    Path = "Source -> Provision -> Regulation -> Condition",
                    Rule = "Only one condition must be present",
                };
                errors.Add(error);
            }

            var conditionTypes = typeof(ConditionType).GetDisplayNames<ConditionType>().ToList();
            var areAllValidConditions = passedInConditions.All(conditionTypes.Contains);

            if (!areAllValidConditions)
            {
                SemanticValidationError error = new()
                {
                    Name = "Invalid conditions",
                    Message = "One or more conditions are invalid",
                    Path = "Source -> Provision -> Regulation -> ConditionSet -> Condition",
                    Rule = $"One or more type of '{string.Join(", ", typeof(ConditionType))}' must be present",
                };
                errors.Add(error);
            }
        }

        return errors;
    }
}