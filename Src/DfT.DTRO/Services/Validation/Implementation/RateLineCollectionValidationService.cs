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
                return DateTime.TryParse(passedInEndValidUsagePeriod, out var dateTime) &&
                       (dateTime.TimeOfDay >= start || dateTime.TimeOfDay <= end);
            }).ToList();

        if (areValidEndValidUsagePeriods.Any(isValidEndUsagePeriod => isValidEndUsagePeriod == false))
        {
            SemanticValidationError error = new()
            {
                Name = "End usage valid period",
                Message = "The end time for the validity of this rate line collection.",
                Path = "Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> endValidUsagePeriod",
                Rule = "If present 'endValidUsagePeriod' must be between '00:00:00' and '23:59:59'",
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

        return errors;
    }
}