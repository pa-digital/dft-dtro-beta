using Google.Type;
using DateTime = System.DateTime;

namespace DfT.DTRO.Services.Validation.Implementation;

/// <inheritdoc cref="IProvisionValidationService"/>
public class ProvisionValidationService : IProvisionValidationService
{
    private readonly SystemClock _clock = new();

    /// <inheritdoc cref="IProvisionValidationService"/>
    public List<SemanticValidationError> Validate(DtroSubmit dtroSubmit)
    {
        List<SemanticValidationError> validationErrors = new();

        var provisions = dtroSubmit.Data
            .GetValueOrDefault<IList<object>>($"{Constants.Source}.{Constants.Provision}"
                .ToBackwardCompatibility(dtroSubmit.SchemaVersion))
            .Cast<ExpandoObject>()
            .ToList();

        var actionTypes = provisions.Select(provision => provision.GetValue<string>(Constants.ActionType)).ToList();
        var areActionTypesValid = actionTypes.TrueForAll(actionType => Constants.ProvisionActionTypes.Any(actionType.Equals));
        if (!areActionTypesValid)
        {
            var error = new SemanticValidationError
            {
                Name = $"Invalid '{Constants.ActionType}'",
                Message = "Indicates the nature of update",
                Rule = $"Provision '{Constants.ActionType}' must contain one of the following accepted values: '{string.Join(",", Constants.ProvisionActionTypes)}'",
                Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.ActionType}"
            };

            validationErrors.Add(error);
        }

        DateOnly validDate = default;
        var hasComingIntoForceDates = dtroSubmit.SchemaVersion >= new SchemaVersion("3.4.0") &&
                                      provisions.Any(provision => provision.HasField(Constants.ComingIntoForceDate));
        if (hasComingIntoForceDates)
        {
            var comingIntoForceDates = provisions.
                Where(provision => provision.HasField(Constants.ComingIntoForceDate))
                .Select(provision =>
                provision.GetValueOrDefault<string>(Constants.ComingIntoForceDate))
                .ToList();
            var areValidDates = comingIntoForceDates
                .TrueForAll(comingIntoForceDate =>
                    DateOnly.TryParse(comingIntoForceDate, out validDate));
            if (!areValidDates)
            {
                var error = new SemanticValidationError
                {
                    Name = $"Invalid '{Constants.ComingIntoForceDate}'",
                    Message = "The date that the provision is coming into force",
                    Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.ComingIntoForceDate}",
                    Rule = $"{Constants.ComingIntoForceDate} if present, must be of type string and formatted as {typeof(DateOnly)}"
                };

                validationErrors.Add(error);
            }

            if (validDate > DateOnly.FromDateTime(_clock.UtcNow.DateTime))
            {
                var error = new SemanticValidationError
                {
                    Name = $"Invalid '{Constants.ComingIntoForceDate}'",
                    Message = "The date that the provision is coming into force",
                    Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.ComingIntoForceDate}",
                    Rule = $"{Constants.ComingIntoForceDate} can not be in the future"
                };

                validationErrors.Add(error);
            }
        }

        var hasExpectedOccupancyDurations = dtroSubmit.SchemaVersion >= new SchemaVersion("3.4.0") &&
                                            provisions.Any(provision => provision.HasField(Constants.ExpectedOccupancyDuration));
        if (hasExpectedOccupancyDurations)
        {
            var expectedOccupancyDurations = provisions
                .Select(provision => provision.GetValueOrDefault<int>(Constants.ExpectedOccupancyDuration))
                .ToList();

            if (expectedOccupancyDurations.Any(expectedOccupancyDuration => expectedOccupancyDuration <= 0))
            {
                var error = new SemanticValidationError
                {
                    Name = $"Invalid '{Constants.ExpectedOccupancyDuration}'",
                    Message = "Expected duration (in integer days) of the provision occupancy",
                    Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.ExpectedOccupancyDuration}",
                    Rule = $"{Constants.ExpectedOccupancyDuration} if present, must be of type 'int' and cannot be less or equal to 0"
                };

                validationErrors.Add(error);
            }
        }

        var orderReportingPoints = provisions
            .Select(provision => provision.GetValueOrDefault<string>(Constants.OrderReportingPoint))
            .ToList();

        var areValidOrderReportingPoints = orderReportingPoints
            .TrueForAll(orderReportingPoint => Constants.OrderReportingPointTypes.Any(orderReportingPoint.Equals));

        if (!areValidOrderReportingPoints)
        {
            var error = new SemanticValidationError
            {
                Name = $"Invalid '{Constants.OrderReportingPoint}'",
                Message = "Attribute identifying the lifecycle point and nature of a Provision",
                Rule = $"'{Constants.OrderReportingPoint}' must be one of '{string.Join(",", Constants.OrderReportingPointTypes)}'",
                Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.OrderReportingPoint}"
            };

            validationErrors.Add(error);
        }

        var anyTemporaryOrderReportingPoints = dtroSubmit.SchemaVersion >= new SchemaVersion("3.4.0") &&
                                               orderReportingPoints.Any(orderReportingPoint => orderReportingPoint.StartsWith("ttro"));
        if (anyTemporaryOrderReportingPoints)
        {
            var actualStartOrStops = provisions
                .Where(provision => ((IDictionary<string, object>)provision).ContainsKey(Constants.ActualStartOrStop))
                .SelectMany(provision => provision.GetValueOrDefault<IList<object>>(Constants.ActualStartOrStop))
                .Cast<ExpandoObject>()
                .ToList();

            // if (actualStartOrStops.Count == 0)
            // {
            //     var error = new SemanticValidationError
            //     {
            //         Name = $"Invalid '{Constants.ActualStartOrStop}'",
            //         Message = "Object supporting the recording of actual start and stop dates and times",
            //         Rule = $"'{Constants.ActualStartOrStop}' must be present if a temporary ordering reporting point exists",
            //         Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.ActualStartOrStop}"
            //     };

            //     validationErrors.Add(error);
            // }

            if (actualStartOrStops.Count > 0)
            {
                var eventAts = actualStartOrStops
                .Select(actualStartOrStop => ((IDictionary<string, object>)actualStartOrStop)[Constants.EventAt].ToString())
                .ToList();

                DateTime eventAtValidDate = default;
                var areValidEventAts = eventAts.TrueForAll(eventAt => DateTime.TryParse(eventAt, out eventAtValidDate));
                if (!areValidEventAts)
                {
                    var error = new SemanticValidationError
                    {
                        Name = $"Invalid '{Constants.EventAt}'",
                        Message = "Indicates the date / time of the related event",
                        Rule = $"'{Constants.EventAt}' must be of type 'string' and formatted as date-time",
                        Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.ActualStartOrStop} -> {Constants.EventAt}"
                    };

                    validationErrors.Add(error);
                }

                var isInTheFuture = eventAtValidDate > _clock.UtcNow;
                if (isInTheFuture)
                {
                    var error = new SemanticValidationError
                    {
                        Name = $"Invalid '{Constants.EventAt}'",
                        Message = "Indicates the date / time of the related event",
                        Rule = $"'{Constants.EventAt}' can not be in the future",
                        Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.ActualStartOrStop} -> {Constants.EventAt}"
                    };

                    validationErrors.Add(error);
                }

                var eventTypes = actualStartOrStops
                    .Select(actualStartOrStop => actualStartOrStop.GetValueOrDefault<string>(Constants.EventType))
                    .ToList();

                var areValidEventTypes = eventTypes.TrueForAll(eventType => Constants.EventTypes.Any(eventType.Equals));
                if (!areValidEventTypes)
                {
                    var error = new SemanticValidationError
                    {
                        Name = $"Invalid '{Constants.EventType}'",
                        Message = "Indicates that the event represents an actual start or stop time",
                        Rule = $"'{Constants.EventType}' must be one of '{string.Join(",", Constants.EventTypes)}'",
                        Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.ActualStartOrStop} -> {Constants.EventType}"
                    };

                    validationErrors.Add(error);
                }
            }
        }

        var anyExperimentalOrderReportingPoints = dtroSubmit.SchemaVersion >= new SchemaVersion("3.4.0") &&
                                                  orderReportingPoints.Any(orderReportingPoint => orderReportingPoint.StartsWith("experimental"));
        if (anyExperimentalOrderReportingPoints)
        {
            var experimentalVariations = provisions
                .Select(provision => provision.GetExpandoOrDefault(Constants.ExperimentalVariation))
                .ToList();
            if (experimentalVariations.Count == 0)
            {
                var error = new SemanticValidationError
                {
                    Name = $"Invalid '{Constants.ExperimentalVariation}'",
                    Message = "Object to enable the characteristic of an Experimental Order variation to be defined",
                    Rule = $"'{Constants.ExperimentalVariation}' must be present if a experimental ordering reporting point exists",
                    Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.ExperimentalVariation}"
                };

                validationErrors.Add(error);
            }

            var effectOfChanges = experimentalVariations
                .Select(experimentalVariation => experimentalVariation.GetValueOrDefault<string>(Constants.EffectOfChange))
                .ToList();

            if (effectOfChanges.Any(string.IsNullOrEmpty))
            {

                var error = new SemanticValidationError
                {
                    Name = $"Invalid '{Constants.EffectOfChange}'",
                    Message = "Description of the effect of the change of the Experimental Order",
                    Rule = $"'{Constants.EffectOfChange}' must be present and of type 'string'",
                    Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.ExperimentalVariation} -> {Constants.EffectOfChange}"
                };

                validationErrors.Add(error);
            }

            var expectedDurations = experimentalVariations
                .Select(experimentalVariation => experimentalVariation.GetValueOrDefault<int>(Constants.ExpectedDuration))
                .ToList();
            if (expectedDurations.Any(expectedDuration => expectedDuration <= 0))
            {
                var error = new SemanticValidationError
                {
                    Name = $"Invalid '{Constants.ExpectedDuration}'",
                    Message = "Duration (in integer days) of the change of effect of the Experimental Order",
                    Rule = $"'{Constants.ExpectedDuration}' must be present and of type 'int'",
                    Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.ExperimentalVariation} -> {Constants.ExpectedDuration}"
                };

                validationErrors.Add(error);
            }

            var experimentalCessations = provisions
                .Select(provision => provision.GetExpandoOrDefault(Constants.ExperimentalCessation))
                .ToList();
            if (experimentalCessations.Count == 0)
            {
                var error = new SemanticValidationError
                {
                    Name = $"Invalid '{Constants.ExperimentalCessation}'",
                    Message = "Object to enable the characteristic of an Experimental Order cessation to be defined",
                    Rule = $"'{Constants.ExperimentalCessation}' must be present if a experimental ordering reporting point exists",
                    Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.ExperimentalCessation}"
                };

                validationErrors.Add(error);
            }

            var actualDateOfCessations = experimentalCessations
                .Select(experimentalCessation => experimentalCessation.GetValueOrDefault<string>(Constants.ActualDateOfCessation))
                .ToList();

            if (actualDateOfCessations.Any(string.IsNullOrEmpty))
            {
                var error = new SemanticValidationError
                {
                    Name = $"Invalid '{Constants.ActualDateOfCessation}'",
                    Message = "Description of the effect of the change of the Experimental Order",
                    Rule = $"'{Constants.ActualDateOfCessation}' must be present and of type 'string' and formatted as date-time",
                    Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.ExperimentalCessation} -> {Constants.ActualDateOfCessation}"
                };

                validationErrors.Add(error);
            }

            DateOnly actualDateOfCessationValidDate = default;
            var isActualDateOfCessationValid = actualDateOfCessations.Any(actualDateOfCessation =>
                DateOnly.TryParse(actualDateOfCessation, out actualDateOfCessationValidDate));

            if (!isActualDateOfCessationValid)
            {
                var error = new SemanticValidationError
                {
                    Name = $"Invalid '{Constants.ActualDateOfCessation}'",
                    Message = "The date that the Experimental Order was ceased",
                    Rule = $"'{Constants.ActualDateOfCessation}' must be of type 'string' and formatted as date-time",
                    Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.ExperimentalCessation} -> {Constants.ActualDateOfCessation}"
                };

                validationErrors.Add(error);
            }

            var isInTheFuture = actualDateOfCessationValidDate > DateOnly.FromDateTime(_clock.UtcNow.DateTime);
            if (isInTheFuture)
            {
                var error = new SemanticValidationError
                {
                    Name = $"Invalid '{Constants.ActualDateOfCessation}'",
                    Message = "The date that the Experimental Order was ceased",
                    Rule = $"'{Constants.ActualDateOfCessation}' can not be in the future",
                    Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.ExperimentalCessation} -> {Constants.ActualDateOfCessation}"
                };

                validationErrors.Add(error);
            }

            var natureOfCessations = experimentalCessations
                .Select(experimentalCessation => experimentalCessation
                    .GetValueOrDefault<string>(Constants.NatureOfCessation))
                .ToList();

            if (natureOfCessations.Any(string.IsNullOrEmpty))
            {
                var error = new SemanticValidationError
                {
                    Name = $"Invalid '{Constants.NatureOfCessation}'",
                    Message = "Description of the reason for the cessation of the Experimental Order",
                    Rule = $"'{Constants.NatureOfCessation}' must be present and of type 'string'",
                    Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.ExperimentalCessation} -> {Constants.NatureOfCessation}"
                };

                validationErrors.Add(error);
            }
        }

        var provisionDescription = provisions
            .Select(it => it.GetValueOrDefault<string>(Constants.ProvisionDescription))
            .ToList();
        if (provisionDescription.Any(string.IsNullOrEmpty))
        {
            var error = new SemanticValidationError
            {
                Name = $"Invalid '{Constants.ProvisionDescription}'",
                Message = "Free text description of the referenced provision",
                Rule = $"Provision '{Constants.ProvisionDescription}' must be of type 'string'",
                Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.ProvisionDescription}"
            };

            validationErrors.Add(error);
        }

        var references = provisions.Select(provision => provision.GetValueOrDefault<string>(Constants.Reference)).ToList();
        if (references.Any(string.IsNullOrWhiteSpace))
        {
            var error = new SemanticValidationError
            {
                Name = $"Invalid '{Constants.Reference}'",
                Message = "Indicates a system reference to the relevant Provision of the TRO",
                Rule = $"{Constants.Provision} '{Constants.Reference}' must be of type 'string'",
                Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.Reference}"
            };

            validationErrors.Add(error);
        }
        if (references.Count > 1)
        {
            var dictionary = references
                .GroupBy(reference => reference)
                .Where(grouping => grouping.Count() > 1)
                .ToDictionary(grouping => grouping.Key, grouping => grouping.Count());


            IEnumerable<SemanticValidationError> errors = dictionary.Select(kv =>
            {
                var error = new SemanticValidationError
                {
                    Name = $"'{kv.Value}' duplication {Constants.Reference}",
                    Message = $"{Constants.Provision} {Constants.Reference} '{kv.Key}' is present {kv.Value} times.",
                    Rule = $"Each provision '{Constants.Reference}' must be unique and of type 'string'",
                    Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.Reference}"
                };

                return error;
            });

            validationErrors.AddRange(errors);
        }

        return validationErrors;
    }
}