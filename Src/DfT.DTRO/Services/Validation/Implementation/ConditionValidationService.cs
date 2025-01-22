namespace DfT.DTRO.Services.Validation.Implementation;

/// <inheritdoc cref="IConditionValidationService"/>
public class ConditionValidationService : IConditionValidationService
{
    /// <inheritdoc cref="IConditionValidationService"/>
    public List<SemanticValidationError> ValidateCondition(DtroSubmit dtroSubmit, SchemaVersion schemaVersion)
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

        foreach (var regulation in regulations)
        {
            var hasConditionSet = regulation.HasField("ConditionSet".ToBackwardCompatibility(dtroSubmit.SchemaVersion));
            if (hasConditionSet)
            {
                var conditionSets = regulation
                        .GetValueOrDefault<IList<object>>(
                            "ConditionSet".ToBackwardCompatibility(dtroSubmit.SchemaVersion))
                        .OfType<ExpandoObject>()
                    .ToList();

                var passedInOperator = conditionSets
                    .Select(conditionSet => conditionSet.GetValueOrDefault<string>(Constants.Operator))
                    .FirstOrDefault();


                var hasValidOperator = passedInOperator != null && Constants.OperatorTypes.Any(passedInOperator.Equals);
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
                        Rule =
                            $"One or more types of '{string.Join(", ", Constants.ConditionTypes)}' conditions must be present",
                    };
                    errors.Add(error);
                }
            }
            else
            {
                var passedInConditions = regulation
                        .GetValueOrDefault<IList<object>>("Condition".ToBackwardCompatibility(dtroSubmit.SchemaVersion))
                        .OfType<ExpandoObject>()
                    .SelectMany(expandoObjects => expandoObjects)
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

                //TODO: This should be refactored once schema version 3.2.4 will be decommissioned.
                bool areAllValidConditions;
                if (dtroSubmit.SchemaVersion >= new SchemaVersion("3.3.0"))
                {
                    areAllValidConditions = passedInConditions
                        .All(passedInCondition => Constants.ConditionTypes
                            .Contains(passedInCondition.Key));

                }
                else
                {
                    areAllValidConditions = passedInConditions
                        .All(passedInCondition => Constants.PreviousConditionTypes
                            .Contains(passedInCondition.Key));
                }


                if (!areAllValidConditions)
                {
                    SemanticValidationError newError = new()
                    {
                        Name = "Condition",
                        Message = "Invalid condition",
                        Path = "Source -> Provision -> Regulation -> Condition",
                        Rule = $"One of '{string.Join(", ", Constants.ConditionTypes)}' condition must be present",
                    };
                    errors.Add(newError);
                }
            }

        }

        return errors;
    }
}