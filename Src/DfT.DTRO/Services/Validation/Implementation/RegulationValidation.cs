#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DfT.DTRO.Services.Validation.Implementation;

public class RegulationValidation : IRegulationValidation
{
    public List<SemanticValidationError> ValidateRegulation(DtroSubmit dtroSubmit, SchemaVersion schemaVersion)
    {
        var errors = new List<SemanticValidationError>();

        var regulations = dtroSubmit
            .Data
            .GetValueOrDefault<IList<object>>("source.provision".ToBackwardCompatibility(dtroSubmit.SchemaVersion))
            .OfType<ExpandoObject>()
            .SelectMany(expandoObject => expandoObject
                .GetValue<IList<object>>("regulation".ToBackwardCompatibility(dtroSubmit.SchemaVersion))
                .OfType<ExpandoObject>())
            .ToList();

        var areDynamics = regulations
        .Select(regulation => regulation.GetField(Constants.IsDynamic));
        if (areDynamics.Any(it => it.GetType() != typeof(bool)))
        {
            SemanticValidationError newError = new()
            {
                Name = "Regulation 'isDynamic'",
                Message = "Indicates if the regulation is dynamic in nature.",
                Path = "source -> provision -> regulation -> isDynamic",
                Rule = "Regulation 'isDynamic' must be present and has to be 'true' or 'false'.",
            };

            errors.Add(newError);
        }

        var timeZones = regulations.
        Select(regulation => regulation.GetValueOrDefault<string>(Constants.TimeZone));

        if (timeZones.Any(string.IsNullOrEmpty))
        {
            SemanticValidationError newError = new()
            {
                Name = "Regulation 'timeZone'",
                Message = "IANA time-zone (see http://www.iana.org/time-zones).",
                Path = "source -> provision -> regulation -> timeZone",
                Rule = "Regulation 'timeZone' must be of type 'string'.",
            };

            errors.Add(newError);
        }


        var existingRegulations = regulations
        .Where(regulation => Constants.RegulationInstances.Any(regulation.HasField))
        .ToList();

        existingRegulations = existingRegulations
            .Where(existingRegulation => !existingRegulation.HasField("temporaryProvision"))
            .ToList();

        foreach (var existingRegulation in existingRegulations)
        {
            var multipleRegulations = Constants.RegulationInstances.Where(existingRegulation.HasField).ToList();
            if (multipleRegulations.Count > 1)
            {
                SemanticValidationError newError = new()
                {
                    Name = "Invalid number of regulations",
                    Message = "Object indicating the characteristics of a regulation",
                    Path = "source -> provision -> regulation -> oneOf",
                    Rule = "Only one sub-type of Regulation can be associated with each Regulation instance",
                };

                errors.Add(newError);
                return errors;
            }

            foreach (var regulationType in Constants.RegulationInstances.Where(existingRegulation.HasField))
            {
                ExpandoObject selectedRegulation;
                string passedInType;
                bool isValidType;
                switch (regulationType)
                {
                    case "speedLimitValueBased":
                        selectedRegulation = existingRegulation.GetValueOrDefault<ExpandoObject>(regulationType);
                        var mphValue = selectedRegulation.GetValueOrDefault<int>(Constants.MphValue);
                        if (mphValue.GetType() != typeof(int) || !Constants.MphValues.Any(mphValue.Equals))
                        {
                            SemanticValidationError newError = new()
                            {
                                Name = "Invalid 'mphValue'",
                                Message = "Speed limit value in miles per hour",
                                Path = "source -> provision -> regulation -> SpeedLimitValueBased -> mphValue",
                                Rule = "'mphValue' must be an integer and one of these values: 10, 20, 30, 40, 50, 60, 70",
                            };

                            errors.Add(newError);
                        }

                        passedInType = selectedRegulation.GetValueOrDefault<string>(Constants.Type);
                        isValidType = Constants.SpeedLimitValueTypes.Any(passedInType.Equals);
                        if (!isValidType)
                        {
                            SemanticValidationError newError = new()
                            {
                                Name = "Invalid 'type'",
                                Message = "Speed limit value type value indicated",
                                Path = "source -> provision -> regulation -> SpeedLimitValueBased -> type",
                                Rule = $"'type' must be one of '{string.Join(",", Constants.SpeedLimitValueTypes)}'",
                            };

                            errors.Add(newError);
                        }
                        break;
                    case "speedLimitProfileBased":
                        selectedRegulation = existingRegulation.GetValueOrDefault<ExpandoObject>(regulationType);
                        passedInType = selectedRegulation.GetValueOrDefault<string>(Constants.Type);
                        isValidType = Constants.SpeedLimitProfileTypes.Any(passedInType.Equals);
                        if (!isValidType)
                        {
                            SemanticValidationError newError = new()
                            {
                                Name = "Invalid 'type'",
                                Message = "Speed limit based value indicated",
                                Path = "source -> provision -> regulation -> SpeedLimitProfileBased -> type",
                                Rule = $"'type' must be one of '{string.Join(",", Constants.SpeedLimitProfileTypes)}'",
                            };
                            errors.Add(newError);
                        }
                        break;
                    case "generalRegulation":
                        selectedRegulation = existingRegulation.GetValueOrDefault<ExpandoObject>(regulationType);
                        passedInType = selectedRegulation.GetValueOrDefault<string>(Constants.RegulationType);
                        isValidType = Constants.RegulationTypes.Any(passedInType.Equals);
                        if (!isValidType)
                        {
                            SemanticValidationError newError = new()
                            {
                                Name = "Invalid 'type'",
                                Message = "Object indicating a specific regulation (other than speed limit or user-defined off-list regulation)",
                                Path = "source -> provision -> regulation -> GeneralRegulation -> regulationType",
                                Rule = $"'type' must be one of '{string.Join(",", Constants.RegulationTypes)}'",
                            };
                            errors.Add(newError);
                        }
                        break;
                    case "offListRegulation":
                        selectedRegulation = existingRegulation.GetValueOrDefault<ExpandoObject>(regulationType);
                        var regulationFullText = selectedRegulation.GetValueOrDefault<string>(Constants.RegulationFullText);
                        if (string.IsNullOrEmpty(regulationFullText))
                        {
                            SemanticValidationError newError = new()
                            {
                                Name = "Invalid 'regulationFullText'",
                                Message = "Full description of the other type of regulation",
                                Path = "source -> provision -> regulation -> OffListRegulation -> regulationFullText",
                                Rule = "'regulationFullText' must be of type 'string' and cannot be null",
                            };

                            errors.Add(newError);
                        }

                        var regulationShortName = selectedRegulation.GetValueOrDefault<string>(Constants.RegulationShortName);
                        if (string.IsNullOrEmpty(regulationShortName))
                        {
                            SemanticValidationError newError = new()
                            {
                                Name = "Invalid 'regulationShortName'",
                                Message = "User-defined short name for other type of regulation",
                                Path = "source -> provision -> regulation -> OffListRegulation -> regulationShortName",
                                Rule = "'regulationShortName' must be of type 'string' and cannot be null",
                            };

                            errors.Add(newError);
                        }
                        break;
                }
            }
        }

        return errors;
    }
}