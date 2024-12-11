﻿#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

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
                .Select(conditionSet => conditionSet.GetValueOrDefault<string>(Constants.Operator))
                .FirstOrDefault();


            var hasValidOperator = passedInOperator != null &&
                                   Constants.OperatorTypes.Any(passedInOperator.Equals);

            if (!hasValidOperator)
            {
                SemanticValidationError error = new()
                {
                    Name = "Operator",
                    Message = "Operator is not present or incorrect",
                    Path = "Source -> Provision -> Regulation -> ConditionSet -> operator",
                    Rule = $"One of '{string.Join(", ", Constants.OperatorTypes)}' operators must be present",
                };
                errors.Add(error);
            }

            List<KeyValuePair<string, object>> passedInConditions = new();

            foreach (var possibleCondition in Constants.PossibleConditions
                         .Select(possibleCondition => new
                         {
                             possibleCondition,
                             hasCondition =
                                 conditionSets
                                     .Select(conditionSet => conditionSet.HasField(possibleCondition))
                                     .FirstOrDefault()
                         })
                         .Where(both => both.hasCondition)
                         .Select(both => both.possibleCondition))
            {
                var conditions = conditionSets
                     .Select(conditionSet =>
                         conditionSet
                             .GetValueOrDefault<IList<object>>(possibleCondition
                                 .ToBackwardCompatibility(dtroSubmit.SchemaVersion))
                             .OfType<ExpandoObject>())
                     .SelectMany(expandoObjects => expandoObjects)
                     .SelectMany(expandoObject => expandoObject)
                     .Select(kv => kv)
                     .ToList();

                passedInConditions.AddRange(conditions);
            }

            var areCorrectNegateValues = passedInConditions
                .Where(passedInCondition => passedInCondition.Key == Constants.Negate)
                .Select(passedInCondition => passedInCondition.Value);

            if (areCorrectNegateValues.Any(it => it is not bool))
            {
                SemanticValidationError error = new()
                {
                    Name = "Negate",
                    Message = "One or more 'negate' values are incorrect",
                    Path = "Source -> Provision -> Regulation -> ConditionSet -> conditions -> negate",
                    Rule = "Negate property must be boolean, 'true' or 'false'",
                };
                errors.Add(error);
            }

            passedInConditions = passedInConditions
                .Where(passedInCondition => passedInCondition.Key != Constants.Operator)
                .Where(passedInCondition => passedInCondition.Key != Constants.Negate)
                .ToList();


            var areAllValidConditions = passedInConditions.All(passedInCondition => Constants.ConditionTypes.Contains(passedInCondition.Key));

            if (!areAllValidConditions)
            {
                SemanticValidationError error = new()
                {
                    Name = "Invalid conditions",
                    Message = "One or more conditions are invalid",
                    Path = "Source -> Provision -> Regulation -> ConditionSet -> conditions",
                    Rule = $"One or more types of '{string.Join(", ", Constants.ConditionTypes)}' conditions must be present",
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
                .Select(kv => kv)
                .ToList();

            var areCorrectNegateValues = passedInConditions
                .Where(passedInCondition => passedInCondition.Key == Constants.Negate)
                .Select(passedInCondition => passedInCondition.Value);

            if (areCorrectNegateValues.Any(it => it is not bool))
            {
                SemanticValidationError error = new()
                {
                    Name = "Negate",
                    Message = "One or more 'negate' values are incorrect",
                    Path = "Source -> Provision -> Regulation -> ConditionSet -> conditions -> negate",
                    Rule = "Negate property must be boolean, 'true' or 'false'",
                };
                errors.Add(error);
            }

            passedInConditions = passedInConditions
                .Where(passedInCondition => passedInCondition.Key != Constants.Negate)
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

            var areAllValidConditions = passedInConditions
                .All(passedInCondition => Constants.ConditionTypes
                    .Contains(passedInCondition.Key));

            if (areAllValidConditions)
            {
                return errors;
            }

            SemanticValidationError newError = new()
            {
                Name = "Condition",
                Message = "Invalid condition",
                Path = "Source -> Provision -> Regulation -> Condition",
                Rule = $"One of '{string.Join(", ", Constants.ConditionTypes)}' condition must be present",
            };
            errors.Add(newError);
        }

        return errors;
    }
}