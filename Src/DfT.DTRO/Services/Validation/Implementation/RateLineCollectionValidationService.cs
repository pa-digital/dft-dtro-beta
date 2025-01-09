namespace DfT.DTRO.Services.Validation.Implementation;

/// <inheritdoc cref="IRateLineCollectionValidationService"/>
public class RateLineCollectionValidationService : IRateLineCollectionValidationService
{
    /// <inheritdoc cref="IRateLineCollectionValidationService"/>
    public List<SemanticValidationError> Validate(DtroSubmit dtroSubmit)
    {
        List<SemanticValidationError> errors = new();

        var rateTables = dtroSubmit
            .Data
            .GetValueOrDefault<IList<object>>("Source.Provision".ToBackwardCompatibility(dtroSubmit.SchemaVersion))
            .OfType<ExpandoObject>()
            .SelectMany(expandoObject => expandoObject
                .GetValueOrDefault<IList<object>>("Regulation".ToBackwardCompatibility(dtroSubmit.SchemaVersion))
                .OfType<ExpandoObject>())
            .SelectMany(expandoObject => expandoObject
                .GetValueOrDefault<IList<object>>("Condition".ToBackwardCompatibility(dtroSubmit.SchemaVersion))
                .OfType<ExpandoObject>())
            .Select(expandoObject => expandoObject.GetValueOrDefault<ExpandoObject>("RateTable"))
            .ToList();

        var rateLineCollections = rateTables
            .SelectMany(expandoObject => expandoObject
                .GetValueOrDefault<IList<object>>("RateLineCollection")
                .OfType<ExpandoObject>())
            .ToList();

        var passedInCurrencies = rateLineCollections
            .Select(rateLineCollection => rateLineCollection.GetValueOrDefault<string>(Constants.ApplicableCurrency))
            .ToList();

        var areValidCurrencies = passedInCurrencies
            .Select(passedInCurrency => Constants.CurrencyTypes.Any(passedInCurrency.Equals))
            .ToList();

        if (areValidCurrencies.Any(isValidCurrency => isValidCurrency == false))
        {
            SemanticValidationError error = new()
            {
                Name = "Applicable currency",
                Message = "The monetary currency that rates are specified in this rate line collection. [Defined by APDS - ISO/TS 5206-1]",
                Path = "Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> applicableCurrency",
                Rule = $"Applicable currency must be one of '{string.Join(",", Constants.CurrencyTypes)}'",
            };

            errors.Add(error);
        }

        var passedInEndValidUsagePeriods = rateLineCollections
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

        if (areValidEndValidUsagePeriods.Any(isValidEndUsagePeriod => isValidEndUsagePeriod == false))
        {
            SemanticValidationError error = new()
            {
                Name = "End usage valid period",
                Message = "The end time for the validity of this rate line collection.",
                Path = "Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> endValidUsagePeriod",
                Rule = $"If present '{Constants.EndValidUsagePeriod}' must be between '00:00:00' and '23:59:59' and later than '{Constants.StartValidUsagePeriod}'",
            };

            errors.Add(error);
        }

        var passedInMaxTimes = rateLineCollections
            .Select(rateLineCollection => rateLineCollection.GetValueOrDefault<int>(Constants.MaxTime))
            .ToList();

        var areValidMaxTimes = passedInMaxTimes
            .Select(passedInMaxTime => passedInMaxTime != 0)
            .ToList();

        if (areValidMaxTimes.Any(isValidMaxTime => isValidMaxTime == false))
        {

            SemanticValidationError error = new()
            {
                Name = "Max time",
                Message = "A maximum session duration to be applied to this rate line collection, specified in integer minutes.",
                Path = "Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> maxTime",
                Rule = "If present 'maxTime' must be of type integer and not 0.",
            };

            errors.Add(error);
        }

        var passedInMaxValueCollections = rateLineCollections
            .Select(maxValueCollection => maxValueCollection.GetValueOrDefault<object>(Constants.MaxValueCollection))
            .Cast<double>()
            .ToList();

        var areValidMaxValueCollections = passedInMaxValueCollections
            .Select(passedInMaxValueCollection => passedInMaxValueCollection > 0.0)
            .ToList();

        if (areValidMaxValueCollections.Any(isValidMaxValueCollection => isValidMaxValueCollection == false))
        {

            SemanticValidationError error = new()
            {
                Name = "Max Value Collection",
                Message = "The maximum monetary amount to be applied in conjunction with use of this rate line collection. Defined in applicable currency with 2 decimal places.",
                Path = "Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> maxValueCollection",
                Rule = "If present 'maxValueCollection' must be of type decimal and not 0.0 or negative"
            };

            errors.Add(error);
        }

        var passedInMinTimes = rateLineCollections
            .Select(rateLineCollection => rateLineCollection.GetValueOrDefault<int>(Constants.MinTime))
            .ToList();

        var areValidMinTimes = passedInMinTimes
            .Select(passedInMinTime => passedInMinTime != 0)
            .ToList();

        if (areValidMinTimes.Any(isValidMinTime => isValidMinTime == false))
        {

            SemanticValidationError error = new()
            {
                Name = "Min time",
                Message = "A minimum session duration to be applied to this rate line collection, specified in integer minutes.",
                Path = "Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> minTime",
                Rule = "If present 'minTime' must be of type integer and not 0.",
            };

            errors.Add(error);
        }


        var passedInMinValueCollections = rateLineCollections
            .Select(minValueCollection => minValueCollection.GetValueOrDefault<object>(Constants.MinValueCollection))
            .Cast<double>()
            .ToList();

        var areValidMinValueCollections = passedInMinValueCollections
            .Select(passedInMinValueCollection => passedInMinValueCollection > 0.0)
            .ToList();

        if (areValidMinValueCollections.Any(isValidMinValueCollection => isValidMinValueCollection == false))
        {

            SemanticValidationError error = new()
            {
                Name = "Min Value Collection",
                Message = "The minimum monetary amount to be applied in conjunction with use of this rate line collection. Defined in applicable currency with 2 decimal places.",
                Path = "Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> minValueCollection",
                Rule = "If present 'minValueCollection' must be of type decimal and not 0.0 or negative"
            };

            errors.Add(error);
        }


        var passedInResetTimes = rateLineCollections
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

        if (areValidResetTimes.Any(isValidResetTime => isValidResetTime == false))
        {
            SemanticValidationError error = new()
            {
                Name = "Reset Time",
                Message = "Time that rate resets.",
                Path = "Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> resetTime",
                Rule = "If present 'resetTime' must be between '00:00:00' and '23:59:59'",
            };

            errors.Add(error);
        }

        var passedInSequences = rateLineCollections
            .Select(rateLineCollection => rateLineCollection.GetValueOrDefault<int>(Constants.Sequence))
            .ToList();

        var areValidSequences = passedInSequences
            .All(passedInSequence => passedInSequence >= 0);

        if (!areValidSequences)
        {
            SemanticValidationError error = new()
            {
                Name = "Sequence",
                Message = "An indicator giving the place in sequence of this rate line collection.",
                Path = "Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> sequence",
                Rule = "'Sequence' must be of type integer and not a negative number",
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

        if (areValidStartValidUsagePeriods.Any(isValidStartUsagePeriod => isValidStartUsagePeriod == false))
        {
            SemanticValidationError error = new()
            {
                Name = "Start usage valid period",
                Message = "The start time for the validity of this rate line collection.",
                Path = "Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> startValidUsagePeriod",
                Rule = $"'{Constants.StartValidUsagePeriod}' must be between '00:00:00' and '23:59:59' and earlier than '{Constants.EndValidUsagePeriod}'",
            };

            errors.Add(error);
        }

        return errors;
    }
}