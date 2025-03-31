namespace DfT.DTRO.Services.Validation.Implementation;

/// <inheritdoc cref="IRateLineValidationService" />
public class RateLineValidationService : IRateLineValidationService
{
    /// <inheritdoc cref="IRateLineValidationService" />
    public List<SemanticValidationError> Validate(DtroSubmit dtroSubmit)
    {
        List<SemanticValidationError> errors = new();

        List<ExpandoObject> regulations = dtroSubmit
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

        List<ExpandoObject> rateTables = regulations
            .Select(regulation => regulation
                .GetExpandoOrDefault("RateTable"
                    .ToBackwardCompatibility(dtroSubmit.SchemaVersion)))
            .Where(rateTable => rateTable != null)
            .ToList();

        if (!rateTables.Any())
        {
            return errors;
        }

        List<ExpandoObject> rateLineCollections = rateTables
            .SelectMany(expandoObject => expandoObject
                .GetValueOrDefault<IList<object>>(
                    "RateLineCollection".ToBackwardCompatibility(dtroSubmit.SchemaVersion)))
            .Cast<ExpandoObject>()
            .Where(rateLineCollection => rateLineCollection != null)
            .ToList();

        List<ExpandoObject> rateLines = rateLineCollections
            .SelectMany(expandoObject => expandoObject
                .GetValueOrDefault<IList<object>>("RateLine")
                .Cast<ExpandoObject>())
            .ToList();

        List<string> passedInDurations = rateLines
            .Where(rateLine => rateLine.HasField(Constants.Description))
            .Select(rateLine => rateLine.GetValueOrDefault<string>(Constants.Description))
            .ToList();

        if (passedInDurations.Any(string.IsNullOrEmpty))
        {
            SemanticValidationError error = new()
            {
                Name = "Invalid 'Description'",
                Message = "Free-text description associated with this rate line.",
                Path =
                    $"Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> RateLine -> {Constants.Description}",
                Rule = $"If present, {Constants.Description} must not be empty"
            };

            errors.Add(error);
        }

        if (dtroSubmit.SchemaVersion < new SchemaVersion("3.4.0"))
        {
            List<int> passedInDurationEnds = rateLines
                .Where(rateLine => rateLine.HasField(Constants.DurationEnd))
                .Select(rateLine => rateLine.GetValueOrDefault<int>(Constants.DurationEnd))
                .ToList();

            if (passedInDurationEnds.Any(passedInDuration => passedInDuration <= 0))
            {
                SemanticValidationError error = new()
                {
                    Name = "Invalid 'Duration end'",
                    Message =
                        "If used, indicates the end time for the applicability of the specific rate line, generally with respect to the start of the parking or other mobility session.",
                    Path =
                        $"Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> RateLine -> {Constants.DurationEnd}",
                    Rule = $"If present, {Constants.DurationEnd} must be of type 'integer' and greater than 0"
                };

                errors.Add(error);
            }

            List<int> passedInDurationStarts = rateLines
                .Where(rateLine => rateLine.HasField(Constants.DurationStart))
                .Select(rateLine => rateLine.GetValueOrDefault<int>(Constants.DurationStart))
                .ToList();

            if (passedInDurationStarts.Any(passedInDurationStart => passedInDurationStart <= 0))
            {
                SemanticValidationError error = new()
                {
                    Name = "Invalid 'Duration start'",
                    Message =
                        "Indicates the start time for the applicability of the specific rate line, generally with respect to the start of the parking or other mobility session.",
                    Path =
                        $"Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> RateLine -> {Constants.DurationStart}",
                    Rule = $"If present, {Constants.DurationStart} must be of type 'integer' and greater than 0"
                };

                errors.Add(error);
            }
        }
        else
        {
            List<string> passedInDurationEnds = rateLines
                .Where(rateLine => rateLine.HasField(Constants.DurationEnd))
                .Select(rateLine => rateLine.GetValueOrDefault<string>(Constants.DurationEnd))
                .ToList();

            List<bool> areValidDurationEnds = passedInDurationEnds
                .Where(passedInDurationEnd => !string.IsNullOrEmpty(passedInDurationEnd))
                .Select(passedInDurationEnd =>
                {
                    TimeSpan start = new DateTime(2024, 1, 1, 0, 0, 0).TimeOfDay;
                    TimeSpan end = new DateTime(2024, 1, 1, 23, 59, 59).TimeOfDay;
                    return DateTime.TryParse(passedInDurationEnd, out DateTime dateTime) &&
                           dateTime.TimeOfDay >= start && dateTime.TimeOfDay <= end;
                }).ToList();

            if (!areValidDurationEnds.Any())
            {
                SemanticValidationError error = new()
                {
                    Name = "Invalid 'Duration end'",
                    Message =
                        "If used, indicates the end time for the applicability of the specific rate line, generally with respect to the start of the parking or other mobility session.",
                    Path =
                        $"Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> RateLine -> {Constants.DurationEnd}",
                    Rule = $"If present, {Constants.DurationEnd} must be of type 'integer' and greater than 0"
                };

                errors.Add(error);
            }

            List<string> passedInDurationStarts = rateLines
                .Where(rateLine => rateLine.HasField(Constants.DurationStart))
                .Select(rateLine => rateLine.GetValueOrDefault<string>(Constants.DurationStart))
                .ToList();

            List<bool> areValidDurationStarts = passedInDurationStarts
                .Where(passedInDurationStart => !string.IsNullOrEmpty(passedInDurationStart))
                .Select(passedInDurationStart =>
                {
                    TimeSpan start = new DateTime(2024, 1, 1, 0, 0, 0).TimeOfDay;
                    TimeSpan end = new DateTime(2024, 1, 1, 23, 59, 59).TimeOfDay;
                    return DateTime.TryParse(passedInDurationStart, out DateTime dateTime) &&
                           dateTime.TimeOfDay >= start && dateTime.TimeOfDay <= end;
                }).ToList();

            if (!areValidDurationStarts.Any())
            {
                SemanticValidationError error = new()
                {
                    Name = "Invalid 'Duration start'",
                    Message =
                        "Indicates the start time for the applicability of the specific rate line, generally with respect to the start of the parking or other mobility session.",
                    Path =
                        $"Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> RateLine -> {Constants.DurationStart}",
                    Rule = $"If present, {Constants.DurationStart} must be of type 'integer' and greater than 0"
                };

                errors.Add(error);
            }
        }

        List<int> passedInIncrementPeriods = rateLines
            .Where(rateLine => rateLine.HasField(Constants.IncrementPeriod))
            .Select(rateLine => rateLine.GetValueOrDefault<int>(Constants.IncrementPeriod))
            .ToList();

        bool areValidIncrementPeriods = passedInIncrementPeriods
            .TrueForAll(incrementPeriod => incrementPeriod > 0);

        if (!areValidIncrementPeriods)
        {
            SemanticValidationError error = new()
            {
                Name = "Invalid 'Increment period'",
                Message =
                    $"The time period for incrementing the rate line charge. If set to the same as the duration of the period between the '{Constants.DurationStart}' and '{Constants.DurationEnd}' the increment will occur once per period.",
                Path =
                    $"Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> RateLine -> {Constants.IncrementPeriod}",
                Rule = $"If present, {Constants.IncrementPeriod} must be in integer and not 0."
            };

            errors.Add(error);
        }

        List<object> passedInMaxValues = rateLines
            .Where(rateLine => rateLine.HasField(Constants.MaxValue))
            .Select(rateLine => rateLine.GetValueOrDefault<object>(Constants.MaxValue))
            .ToList();

        if (passedInMaxValues.Any(it => it is string))
        {
            SemanticValidationError error = new()
            {
                Name = "Invalid 'Max value'",
                Message =
                    "The maximum monetary amount to be applied in conjunction with use of this rate line collection, regardless of the actual calculated value of the rate line. Defined in applicable currency with 2 decimal places",
                Path =
                    $"Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> RateLine -> {Constants.MaxValue}",
                Rule =
                    $"If present, {Constants.MaxValue} must be defined in applicable currency with 2 decimal places and not 0.0"
            };

            errors.Add(error);
        }

        List<object> passedInMinValues = rateLines
            .Where(rateLine => rateLine.HasField(Constants.MinValue))
            .Select(rateLine => rateLine.GetValueOrDefault<object>(Constants.MinValue))
            .ToList();

        if (passedInMinValues.Any(it => it is string))
        {
            SemanticValidationError error = new()
            {
                Name = "Invalid 'Min value'",
                Message =
                    "The minimum monetary amount to be applied in conjunction with use of this rate line collection, regardless of the actual calculated value of the rate line. Defined in applicable currency with 2 decimal places",
                Path =
                    $"Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> RateLine -> {Constants.MinValue}",
                Rule =
                    $"If present, {Constants.MinValue} must be defined in applicable currency with 2 decimal places and not 0.0"
            };

            errors.Add(error);
        }

        List<int> passedInSequences = rateLines
            .Select(rateLine => rateLine.GetValueOrDefault<int>(Constants.Sequence))
            .ToList();

        bool areValidSequences = passedInSequences
            .All(passedInSequence => passedInSequence > 0);

        if (!areValidSequences)
        {
            SemanticValidationError error = new()
            {
                Name = "Invalid 'Sequence'",
                Message = "An indicator giving the place in sequence of this rate line collection.",
                Path =
                    $"Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> {Constants.Sequence}",
                Rule = $"'{Constants.Sequence}' must be of type integer and not zero"
            };

            errors.Add(error);
        }

        List<string> passedInTypes = rateLines
            .Select(rateLine => rateLine.GetValueOrDefault<string>(Constants.Type))
            .ToList();

        bool areValidTypes = passedInTypes
            .TrueForAll(passedInType => Constants.RateLineTypes.Any(passedInType.Equals));

        if (!areValidTypes)
        {
            SemanticValidationError error = new()
            {
                Name = "Invalid 'Rate line type'",
                Message = "Indicates the nature of the rate line",
                Path =
                    $"Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> {Constants.Type}",
                Rule = $"'{Constants.Type}' must be one of '{string.Join(",", Constants.RateLineTypes)}'"
            };

            errors.Add(error);
        }

        List<string> passedInUsageConditions = rateLines
            .Where(rateLine => rateLine.HasField(Constants.UsageCondition))
            .Select(rateLine => rateLine.GetValueOrDefault<string>(Constants.UsageCondition))
            .ToList();

        bool areValidUsageConditions = passedInUsageConditions
            .TrueForAll(passedInUsageCondition =>
                Constants.RateUsageConditionsTypes.Any(passedInUsageCondition.Equals));

        if (!areValidUsageConditions)
        {
            SemanticValidationError error = new()
            {
                Name = "Invalid 'Rate usage condition type'",
                Message = "Indicates conditions on the use of this rate line.",
                Path =
                    $"Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> {Constants.UsageCondition}",
                Rule =
                    $"'{Constants.UsageCondition}' must be one of '{string.Join(",", Constants.RateUsageConditionsTypes)}'"
            };

            errors.Add(error);
        }

        List<object> values = rateLines
            .Where(rateLine => rateLine.HasField(Constants.Value))
            .Select(rateLine => rateLine.GetValueOrDefault<object>(Constants.Value))
            .ToList();

        if (values.Any(it => it is string))
        {
            SemanticValidationError error = new()
            {
                Name = "Invalid 'Value'",
                Message = "The value of the fee to be charged in respect of this rate line.",
                Path =
                    $"Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> {Constants.Value}",
                Rule =
                    $"'{Constants.Value}' must be defined in applicable currency with 2 decimal places and not 0.0"
            };

            errors.Add(error);
        }

        return errors;
    }
}