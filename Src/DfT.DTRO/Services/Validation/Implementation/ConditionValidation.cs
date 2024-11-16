using DfT.DTRO.Models.Validation;
using DfT.DTRO.Services.Validation.Contracts;
using Microsoft.CodeAnalysis;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DfT.DTRO.Services.Validation.Implementation;

public class ConditionValidation : IConditionValidation
{
    public IList<SemanticValidationError> ValidateCondition(DtroSubmit dtroSubmit, SchemaVersion schemaVersion)
    {
        var errors = new List<SemanticValidationError>();

        if (schemaVersion < new SchemaVersion("3.2.5"))
        {
            return errors;
        }

        List<ExpandoObject> regulations = dtroSubmit
            .Data
            .GetValueOrDefault<IList<object>>("Source.provision")
            .OfType<ExpandoObject>()
            .SelectMany(expandoObject => expandoObject
                .GetValue<IList<object>>("regulation")
                .OfType<ExpandoObject>())
            .ToList();

        var hasConditionSet = regulations
            .Select(regulation => regulation.HasField("conditionSet"))
            .FirstOrDefault();

        if (hasConditionSet)
        {

            List<ExpandoObject> conditionSets = regulations
                .SelectMany(expandoObject => expandoObject
                    .GetValueOrDefault<IList<object>>("conditionSet")
                    .OfType<ExpandoObject>())
                .ToList();


            List<string> passedInOperator = conditionSets
                .Select(conditionSet => conditionSet.GetValueOrDefault<string>("operator"))
                .ToList();

            List<string> operatorTypes = typeof(OperatorType).GetDisplayNames<OperatorType>().ToList();
            bool hasOperator = operatorTypes.Select(passedInOperator.Contains).FirstOrDefault();
            if (!hasOperator)
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

            var conditionTypes = typeof(ConditionType)
                .GetDisplayNames<ConditionType>()
                .ToList();

            var areAllValidConditions = passedInConditions.All(it => conditionTypes.Contains(it));

            if (!areAllValidConditions)
            {
                SemanticValidationError error = new()
                {
                    Name = "Condition",
                    Message = "One or more condition are incorrect.",
                    Path = "Source -> Provision -> Regulation -> ConditionSet -> Condition",
                    Rule = $"One or more of '{string.Join(", ", conditionTypes)}' operators must be present.",
                };
                errors.Add(error);
            }
        }

        var hasCondition = regulations
            .Select(regulation => regulation.HasField("condition"))
            .FirstOrDefault();

        if (hasCondition)
        {
            var passedInConditions = regulations
                .SelectMany(expandoObject => expandoObject
                    .GetValueOrDefault<IList<object>>("condition"))
                .OfType<ExpandoObject>()
                .ToList();

            var conditionTypes = typeof(ConditionType)
                .GetDisplayNames<ConditionType>()
                .ToList();

            if (passedInConditions.Count > 1)
            {
                SemanticValidationError error = new()
                {
                    Name = "Condition",
                    Message = "Condition is not accepted.",
                    Path = "Source -> Provision -> Regulation -> Condition",
                    Rule = $"One of '{string.Join(", ", conditionTypes)}' conditions must be present.",
                };
                errors.Add(error);
            }

            var hasValidCondition = passedInConditions
                .Select(passedInCondition => conditionTypes.Any(passedInCondition.HasField))
                .FirstOrDefault();

            if (!hasValidCondition)
            {
                SemanticValidationError error = new()
                {
                    Name = "Condition",
                    Message = "Condition is not valid.",
                    Path = "Source -> Provision -> Regulation -> Condition",
                    Rule = $"One of '{string.Join(", ", conditionTypes)}' conditions must be present.",
                };
                errors.Add(error);
            }
        }

        return errors;
    }
}