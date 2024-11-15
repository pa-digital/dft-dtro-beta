using DfT.DTRO.Models.Validation;
using DfT.DTRO.Services.Validation.Contracts;

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

        //TODO: check to make sure conditionSet exists.
        var hasConditionSet = regulations.Select(it => it.GetField("conditionSet")).FirstOrDefault();

        if (hasConditionSet != null)
        {
            List<ExpandoObject> conditionSets = regulations
                .SelectMany(expandoObject => expandoObject
                    .GetValue<IList<object>>("conditionSet")
                    .OfType<ExpandoObject>())
                .ToList();

            var operatorTypes = typeof(OperatorType)
                .GetDisplayNames<OperatorType>()
                .ToList();
            var passedInOperators = conditionSets
                .Select(passedInOperator => passedInOperator
                    .GetValueOrDefault<string>("operator"))
                .ToList();
            var hasOperator = passedInOperators
                .All(it => operatorTypes.Any(it.Contains));

            if (!hasOperator)
            {
                SemanticValidationError error = new()
                {
                    Name = "Condition set",
                    Message = "You have to have at least one operator for the condition set.",
                    Path = "Source -> Provision -> Regulation -> ConditionSet",
                    Rule = $"One of '{string.Join(", ", operatorTypes)}' operators must be present.",
                };
                errors.Add(error);
            }

            List<ExpandoObject> conditions =
                conditionSets
                    .Select(conditionSet => conditionSet
                        .GetValueOrDefault<ExpandoObject>("condition"))
                    .ToList();

            var conditionTypes = typeof(ConditionType)
                .GetDisplayNames<ConditionType>()
                .ToList();
            var passedInConditions = conditions
                .SelectMany(condition => condition
                    .Select(kv => kv.Key))
                .ToList();
            var areEachConditionsAccepted = passedInConditions
                .Select(passedInCondition => conditionTypes
                    .Any(passedInCondition.Contains))
                .ToList();


            if (!areEachConditionsAccepted.All(it => it))
            {
                SemanticValidationError error = new()
                {
                    Name = "Condition",
                    Message = "One or more conditions are not accepted.",
                    Path = "Source -> Provision -> Regulation -> Condition",
                    Rule = $"One or more of '{string.Join(", ", conditionTypes)}' conditions must be present.",
                };
                errors.Add(error);
            }
        }
        else
        {

            List<ExpandoObject> conditions = dtroSubmit
                .Data
                .GetValueOrDefault<IList<object>>("Source.provision")
                .OfType<ExpandoObject>()
                .SelectMany(expandoObject => expandoObject
                    .GetValue<IList<object>>("regulation")
                    .OfType<ExpandoObject>())
                .ToList();

            var conditionTypes = typeof(ConditionType)
                .GetDisplayNames<ConditionType>()
                .ToList();

            var passedInCondition = conditions
                .SelectMany(condition => condition.GetValueOrDefault<IList<object>>("condition"))
                .Cast<ExpandoObject>()
                .FirstOrDefault();

            var isValidCondition = conditionTypes
                .Select(conditionType => passedInCondition
                    .GetField(conditionType)
                    .ToString())
                .Any();


            if (!isValidCondition)
            {
                SemanticValidationError error = new()
                {
                    Name = "Condition",
                    Message = "Condition is not accepted.",
                    Path = "Source -> Provision -> Regulation -> Condition",
                    Rule = $"One or more of '{string.Join(", ", conditionTypes)}' conditions must be present.",
                };
                errors.Add(error);
            }
        }

        return errors;
    }
}