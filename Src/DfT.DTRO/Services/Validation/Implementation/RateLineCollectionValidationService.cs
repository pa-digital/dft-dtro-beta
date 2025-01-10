namespace DfT.DTRO.Services.Validation.Implementation;

/// <inheritdoc cref="IRateLineCollectionValidationService"/>
public class RateLineCollectionValidationService : IRateLineCollectionValidationService
{
    /// <inheritdoc cref="IRateLineCollectionValidationService"/>
    public List<SemanticValidationError> Validate(DtroSubmit dtroSubmit)
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

        List<ExpandoObject> rateTables = new();
        foreach (var regulation in regulations)
        {
            var hasConditionSet = regulation.HasField("ConditionSet".ToBackwardCompatibility(dtroSubmit.SchemaVersion));
            if (hasConditionSet)
            {
                var conditionsSets = regulation
                    .GetValueOrDefault<IList<object>>("ConditionSet")
                    .Cast<ExpandoObject>()
                    .ToList();

                rateTables.AddRange(conditionsSets
                    .Select(conditionSet => conditionSet.GetValueOrDefault<ExpandoObject>("RateTable")));
            }
            else
            {
                var conditions = regulation
                    .GetValueOrDefault<IList<object>>("Condition")
                    .Cast<ExpandoObject>()
                    .ToList();

                rateTables.AddRange(conditions
                    .Select(condition => condition.GetValueOrDefault<ExpandoObject>("RateTable")));
            }

            rateTables = rateTables.Where(rateTable => rateTable != null).ToList();

        }

        var rateLineCollections = rateTables
            .SelectMany(expandoObject => expandoObject
                .GetValueOrDefault<IList<object>>("RateLineCollection"))
            .Cast<ExpandoObject>()
            .ToList();

        rateLineCollections = rateLineCollections.Where(rateLineCollection => rateLineCollection != null).ToList();

        var passedInCurrencies = rateLineCollections
            .Select(rateLineCollection => rateLineCollection.GetValueOrDefault<string>(Constants.ApplicableCurrency))
            .ToList();

        var areValidCurrencies = passedInCurrencies
            .TrueForAll(passedInCurrency => Constants.CurrencyTypes.Any(passedInCurrency.Equals));

        if (!areValidCurrencies)
        {
            SemanticValidationError error = new()
            {
                Name = "Applicable currency",
                Message = "The monetary currency that rates are specified in this rate line collection.",
                Path = $"Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> {Constants.ApplicableCurrency}",
                Rule = $"'{Constants.ApplicableCurrency}' must be one of '{string.Join(",", Constants.CurrencyTypes)}'",
            };

            errors.Add(error);
        }

        var passedInEndValidUsagePeriods = rateLineCollections
            .Where(rateLineCollection => rateLineCollection.HasField(Constants.EndValidUsagePeriod))
            .Select(rateLineCollection => rateLineCollection.GetValueOrDefault<string>(Constants.EndValidUsagePeriod))
            .ToList();

        var areValidEndValidUsagePeriods = passedInEndValidUsagePeriods
            .Where(passedInEndValidUsagePeriod => !string.IsNullOrEmpty(passedInEndValidUsagePeriod))
            .Select(passedInEndValidUsagePeriod =>
            {
                var start = new DateTime(2024, 1, 1, 0, 0, 0).TimeOfDay;
                var end = new DateTime(2024, 1, 1, 23, 59, 59).TimeOfDay;
                return DateTime.TryParse(passedInEndValidUsagePeriod, out var endValidUsagePeriod) &&
                       (endValidUsagePeriod.TimeOfDay >= start ||
                        endValidUsagePeriod.TimeOfDay <= end);
            }).ToList();

        if (areValidEndValidUsagePeriods.All(isValidEndUsagePeriod => isValidEndUsagePeriod == false))
        {
            SemanticValidationError error = new()
            {
                Name = "End usage valid period",
                Message = "The end time for the validity of this rate line collection.",
                Path = $"Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> {Constants.EndValidUsagePeriod}",
                Rule = $"If present '{Constants.EndValidUsagePeriod}' must be between '00:00:00' and '23:59:59'",
            };

            errors.Add(error);
        }

        var passedInMaxTimes = rateLineCollections
            .Where(rateLineCollection => rateLineCollection.HasField(Constants.MaxTime))
            .Select(rateLineCollection => rateLineCollection.GetValueOrDefault<int>(Constants.MaxTime))
            .ToList();

        var areValidMaxTimes = passedInMaxTimes
            .Select(passedInMaxTime => passedInMaxTime != 0)
            .ToList();

        if (areValidMaxTimes.All(isValidMaxTime => isValidMaxTime == false))
        {

            SemanticValidationError error = new()
            {
                Name = "Max time",
                Message = "A maximum session duration to be applied to this rate line collection, specified in integer minutes.",
                Path = $"Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> {Constants.MaxTime}",
                Rule = $"If present '{Constants.MaxTime}' must be of type integer and not 0.",
            };

            errors.Add(error);
        }

        var passedInMaxValueCollections = rateLineCollections
            .Where(rateLineCollection => rateLineCollection.HasField(Constants.MaxValueCollection))
            .Select(maxValueCollection => maxValueCollection.GetValueOrDefault<object>(Constants.MaxValueCollection))
            .Cast<double>()
            .ToList();

        var areValidMaxValueCollections = passedInMaxValueCollections
            .TrueForAll(passedInMaxValueCollection => passedInMaxValueCollection > 0.0);

        if (!areValidMaxValueCollections)
        {
            SemanticValidationError error = new()
            {
                Name = "Max Value Collection",
                Message = "The maximum monetary amount to be applied in conjunction with use of this rate line collection. Defined in applicable currency with 2 decimal places.",
                Path = $"Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> {Constants.MaxValueCollection}",
                Rule = $"If present '{Constants.MaxValueCollection}' must be of type decimal and not 0.0 or negative"
            };

            errors.Add(error);
        }

        var passedInMinTimes = rateLineCollections
            .Where(rateLineCollection => rateLineCollection.HasField(Constants.MinTime))
            .Select(rateLineCollection => rateLineCollection.GetValueOrDefault<int>(Constants.MinTime))
            .ToList();

        var areValidMinTimes = passedInMinTimes.TrueForAll(passedInMinTime => passedInMinTime != 0);

        if (!areValidMinTimes)
        {
            SemanticValidationError error = new()
            {
                Name = "Min time",
                Message = "A minimum session duration to be applied to this rate line collection, specified in integer minutes.",
                Path = $"Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> {Constants.MinTime}",
                Rule = $"If present '{Constants.MinTime}' must be of type integer and not 0.",
            };

            errors.Add(error);
        }

        var passedInMinValueCollections = rateLineCollections
            .Where(rateLineCollection => rateLineCollection.HasField(Constants.MinValueCollection))
            .Select(minValueCollection => minValueCollection.GetValueOrDefault<object>(Constants.MinValueCollection))
            .Cast<double>()
            .ToList();

        var areValidMinValueCollections = passedInMinValueCollections
            .TrueForAll(passedInMinValueCollection => passedInMinValueCollection > 0.0);

        if (!areValidMinValueCollections)
        {
            SemanticValidationError error = new()
            {
                Name = "Min Value Collection",
                Message = "The minimum monetary amount to be applied in conjunction with use of this rate line collection. Defined in applicable currency with 2 decimal places.",
                Path = $"Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> {Constants.MinValueCollection}",
                Rule = $"If present '{Constants.MinValueCollection}' must be of type decimal and not 0.0 or negative"
            };

            errors.Add(error);
        }

        var passedInResetTimes = rateLineCollections
            .Where(rateLineCollection => rateLineCollection.HasField(Constants.ResetTime))
            .Select(rateLineCollection => rateLineCollection.GetValueOrDefault<string>(Constants.ResetTime))
            .ToList();

        var areValidResetTimes = passedInResetTimes
            .Where(passedInResetTime => !string.IsNullOrEmpty(passedInResetTime))
            .Select(passedInResetTime =>
            {
                var start = new DateTime(2024, 1, 1, 0, 0, 0).TimeOfDay;
                var end = new DateTime(2024, 1, 1, 23, 59, 59).TimeOfDay;
                return DateTime.TryParse(passedInResetTime, out var dateTime) &&
                       (dateTime.TimeOfDay >= start || dateTime.TimeOfDay <= end);
            }).ToList();

        if (areValidResetTimes.All(isValidResetTime => isValidResetTime == false))
        {
            SemanticValidationError error = new()
            {
                Name = "Reset Time",
                Message = "Time that rate resets.",
                Path = $"Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> {Constants.ResetTime}",
                Rule = $"If present '{Constants.ResetTime}' must be between '00:00:00' and '23:59:59'",
            };

            errors.Add(error);
        }

        var passedInSequences = rateLineCollections
            .Select(rateLineCollection => rateLineCollection.GetValueOrDefault<int>(Constants.Sequence))
            .ToList();

        var areValidSequences = passedInSequences.TrueForAll(passedInSequence => passedInSequence >= 0);

        if (!areValidSequences)
        {
            SemanticValidationError error = new()
            {
                Name = "Sequence",
                Message = "An indicator giving the place in sequence of this rate line collection.",
                Path = $"Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> {Constants.Sequence}",
                Rule = $"'{Constants.Sequence}' must be of type integer and not a negative number",
            };

            errors.Add(error);
        }

        var passedInStartValidUsagePeriods = rateLineCollections
            .Select(rateLineCollection => rateLineCollection.GetValueOrDefault<string>(Constants.StartValidUsagePeriod))
            .ToList();

        var areValidStartValidUsagePeriods = passedInStartValidUsagePeriods
            .Select(passedInStartValidUsagePeriod =>
            {
                var start = new DateTime(2024, 1, 1, 0, 0, 0).TimeOfDay;
                var end = new DateTime(2024, 1, 1, 23, 59, 59).TimeOfDay;
                return DateTime.TryParse(passedInStartValidUsagePeriod, out var startValidUsagePeriod) &&
                       (startValidUsagePeriod.TimeOfDay >= start ||
                        startValidUsagePeriod.TimeOfDay <= end);
            }).ToList();

        if (areValidStartValidUsagePeriods.All(isValidStartUsagePeriod => isValidStartUsagePeriod == false))
        {
            SemanticValidationError error = new()
            {
                Name = "Start usage valid period",
                Message = "The start time for the validity of this rate line collection.",
                Path = $"Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> {Constants.StartValidUsagePeriod}",
                Rule = $"'{Constants.StartValidUsagePeriod}' must be between '00:00:00' and '23:59:59'",
            };

            errors.Add(error);
        }

        return errors;
    }
}