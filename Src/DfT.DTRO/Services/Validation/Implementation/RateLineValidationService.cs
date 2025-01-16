namespace DfT.DTRO.Services.Validation.Implementation;

/// <inheritdoc cref="IRateLineValidationService"/>
public class RateLineValidationService : IRateLineValidationService
{
    /// <inheritdoc cref="IRateLineValidationService"/>
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

        if (regulations.Any(it => !it.HasField("Condition") && !it.HasField("ConditionSet")))
        {
            return errors;
        }

        var rateTables = regulations
            .Select(regulation => regulation.GetValueOrDefault<ExpandoObject>("RateTable"))
            .Where(rateTable => rateTable != null)
            .ToList();

        if (!rateTables.Any())
        {
            return errors;
        }

        var rateLineCollections = rateTables
            .SelectMany(expandoObject => expandoObject
                .GetValueOrDefault<IList<object>>("RateLineCollection".ToBackwardCompatibility(dtroSubmit.SchemaVersion)))
            .Cast<ExpandoObject>()
            .Where(rateLineCollection => rateLineCollection != null)
            .ToList();

        var rateLines = rateLineCollections
            .SelectMany(expandoObject => expandoObject
                .GetValueOrDefault<IList<object>>("RateLine")
                .Cast<ExpandoObject>())
            .ToList();

        var passedInDurations = rateLines
            .Where(rateLine => rateLine.HasField(Constants.Description))
            .Select(rateLine => rateLine.GetValueOrDefault<string>(Constants.Description))
            .ToList();

        if (passedInDurations.Any(string.IsNullOrEmpty))
        {
            SemanticValidationError error = new()
            {
                Name = "Description",
                Message = "Free-text description associated with this rate line.",
                Path = $"Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> RateLine -> {Constants.Description}",
                Rule = $"If present, {Constants.Description} must not be empty"
            };

            errors.Add(error);
        }

        var passedInDurationEnds = rateLines
            .Where(rateLine => rateLine.HasField(Constants.DurationEnd))
            .Select(rateLine => rateLine.GetValueOrDefault<int>(Constants.DurationEnd))
            .ToList();

        if (passedInDurationEnds.Any(passedInDuration => passedInDuration <= 0))
        {
            SemanticValidationError error = new()
            {
                Name = "Duration end",
                Message = "If used, indicates the end time for the applicability of the specific rate line, generally with respect to the start of the parking or other mobility session.",
                Path = $"Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> RateLine -> {Constants.DurationEnd}",
                Rule = $"If present, {Constants.DurationEnd} must not be empty"
            };

            errors.Add(error);
        }

        var passedInDurationStarts = rateLines
            .Where(rateLine => rateLine.HasField(Constants.DurationStart))
            .Select(rateLine => rateLine.GetValueOrDefault<int>(Constants.DurationStart))
            .ToList();

        if (passedInDurationStarts.Any(passedInDurationStart => passedInDurationStart <= 0))
        {
            SemanticValidationError error = new()
            {
                Name = "Duration start",
                Message = "Indicates the start time for the applicability of the specific rate line, generally with respect to the start of the parking or other mobility session.",
                Path = $"Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> RateLine -> {Constants.DurationStart}",
                Rule = $"If present, {Constants.DurationStart} must not be empty"
            };

            errors.Add(error);
        }

        var passedInIncrementPeriods = rateLines
            .Where(rateLine => rateLine.HasField(Constants.IncrementPeriod))
            .Select(rateLine => rateLine.GetValueOrDefault<int>(Constants.IncrementPeriod))
            .ToList();

        var areValidIncrementPeriods = passedInIncrementPeriods
            .TrueForAll(incrementPeriod => incrementPeriod > 0);

        if (!areValidIncrementPeriods)
        {
            SemanticValidationError error = new()
            {
                Name = "Increment period",
                Message = $"The time period for incrementing the rate line charge. If set to the same as the duration of the period between the '{Constants.DurationStart}' and '{Constants.DurationEnd}' the increment will occur once per period.",
                Path = $"Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> RateLine -> {Constants.IncrementPeriod}",
                Rule = $"If present, {Constants.IncrementPeriod} must be in integer."
            };

            errors.Add(error);
        }

        var passedInMaxValues = rateLines
            .Where(rateLine => rateLine.HasField(Constants.MaxValue))
            .Select(rateLine => rateLine.GetValueOrDefault<object>(Constants.MaxValue))
            .Cast<double>()
            .ToList();

        var areValidMaxValues = passedInMaxValues
            .All(maxValue => maxValue > 0.0);

        if (!areValidMaxValues)
        {
            SemanticValidationError error = new()
            {
                Name = "Max value",
                Message = "The maximum monetary amount to be applied in conjunction with use of this rate line collection, regardless of the actual calculated value of the rate line. Defined in applicable currency with 2 decimal places",
                Path = $"Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> RateLine -> {Constants.MaxValue}",
                Rule = $"If present, {Constants.MaxValue} must be defined in applicable currency with 2 decimal places"
            };

            errors.Add(error);
        }

        var passedInMinValues = rateLines
            .Where(rateLine => rateLine.HasField(Constants.MinValue))
            .Select(rateLine => rateLine.GetValueOrDefault<object>(Constants.MinValue))
            .Cast<double>()
            .ToList();

        var areValidMinValues = passedInMinValues
            .All(maxValue => maxValue > 0.0);

        if (!areValidMinValues)
        {
            SemanticValidationError error = new()
            {
                Name = "Min value",
                Message = "The minimum monetary amount to be applied in conjunction with use of this rate line collection, regardless of the actual calculated value of the rate line. Defined in applicable currency with 2 decimal places",
                Path = $"Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> RateLine -> {Constants.MinValue}",
                Rule = $"If present, {Constants.MinValue} must be defined in applicable currency with 2 decimal places"
            };

            errors.Add(error);
        }

        var passedInSequences = rateLines
            .Select(rateLine => rateLine.GetValueOrDefault<int>(Constants.Sequence))
            .ToList();

        var areValidSequences = passedInSequences
            .All(passedInSequence => passedInSequence > 0);

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

        var passedInTypes = rateLines
            .Select(rateLine => rateLine.GetValueOrDefault<string>(Constants.Type))
            .ToList();

        var areValidTypes = passedInTypes
            .TrueForAll(passedInType => Constants.RateLineTypes.Any(passedInType.Equals));

        if (!areValidTypes)
        {
            SemanticValidationError error = new()
            {
                Name = "Rate line type",
                Message = "Indicates the nature of the rate line",
                Path = $"Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> {Constants.Type}",
                Rule = $"'{Constants.Type}' must be one of '{string.Join(",", Constants.RateLineTypes)}'",
            };

            errors.Add(error);
        }

        var passedInUsageConditions = rateLines
            .Where(rateLine => rateLine.HasField(Constants.UsageCondition))
            .Select(rateLine => rateLine.GetValueOrDefault<string>(Constants.UsageCondition))
            .ToList();

        var areValidUsageConditions = passedInUsageConditions
            .TrueForAll(passedInUsageCondition => Constants.RateUsageConditionsTypes.Any(passedInUsageCondition.Equals));

        if (!areValidUsageConditions)
        {
            SemanticValidationError error = new()
            {
                Name = "Rate usage condition type",
                Message = "Indicates conditions on the use of this rate line.",
                Path = $"Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> {Constants.UsageCondition}",
                Rule = $"'{Constants.UsageCondition}' must be one of '{string.Join(",", Constants.RateUsageConditionsTypes)}'",
            };

            errors.Add(error);
        }

        var values = rateLines
            .Where(rateLine => rateLine.HasField(Constants.Value))
            .Select(rateLine => rateLine.GetValueOrDefault<object>(Constants.Value))
            .Cast<double>()
            .ToList();

        var areValidValues = values.TrueForAll(value => value > 0.0);
        if (!areValidValues)
        {
            SemanticValidationError error = new()
            {
                Name = "Value",
                Message = "The value of the fee to be charged in respect of this rate line.",
                Path = $"Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> {Constants.Value}",
                Rule = $"'{Constants.Value}' must be defined in applicable currency with 2 decimal places.",
            };

            errors.Add(error);
        }

        return errors;
    }
}