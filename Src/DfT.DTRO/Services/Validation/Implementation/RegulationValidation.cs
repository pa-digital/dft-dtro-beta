#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DfT.DTRO.Services.Validation.Implementation;

public class RegulationValidation : IRegulationValidation
{
    public List<SemanticValidationError> ValidateRegulation(DtroSubmit dtroSubmit, SchemaVersion schemaVersion)
    {
        var errors = new List<SemanticValidationError>();

        var regulations = dtroSubmit
            .Data
            .GetValueOrDefault<IList<object>>("Source.Provision".ToBackwardCompatibility(dtroSubmit.SchemaVersion))
            .OfType<ExpandoObject>()
            .SelectMany(expandoObject => expandoObject
                .GetValue<IList<object>>("Regulation".ToBackwardCompatibility(dtroSubmit.SchemaVersion))
                .OfType<ExpandoObject>())
            .ToList();

        var areDynamics = regulations
        .Select(regulation => regulation.GetValueOrDefault<bool>(Constants.IsDynamic));
        if (areDynamics.All(it => it.GetType() != typeof(bool)))
        {
            SemanticValidationError newError = new()
            {
                Name = "Regulations",
                Message = "Indicates if the regulation is dynamic in nature.",
                Path = "Source -> Provision -> Regulation -> isDynamic",
                Rule = "'isDynamic' must be present and has to be 'true' or 'false'.",
            };

            errors.Add(newError);
        }

        var timeZones = regulations.
        Select(regulation => regulation.GetValueOrDefault<string>(Constants.TimeZone));

        if (timeZones.Any(string.IsNullOrEmpty))
        {
            SemanticValidationError newError = new()
            {
                Name = "Regulations",
                Message = "IANA time-zone (see http://www.iana.org/time-zones).",
                Path = "Source -> Provision -> Regulation -> timeZone",
                Rule = "'timeZone' must be present.",
            };

            errors.Add(newError);
        }


        var existingRegulations = regulations
        .Where(regulation => Constants.RegulationInstances.Any(regulation.HasField))
        .ToList();

        foreach (var existingRegulation in existingRegulations)
        {
            foreach (var regulationType in Constants.RegulationInstances.Where(existingRegulation.HasField))
            {
                ExpandoObject selectedRegulation;
                string passedInType;
                bool isValidType;
                switch (regulationType)
                {
                    case "SpeedLimitValueBased":
                        selectedRegulation = existingRegulation.GetValueOrDefault<ExpandoObject>(regulationType);
                        var mphValue = selectedRegulation.GetValueOrDefault<int>(Constants.MphValue);
                        if (mphValue == 0)
                        {
                            SemanticValidationError newError = new()
                            {
                                Name = "Speed Limit Value Based Regulation",
                                Message = "Speed limit value in miles per hour",
                                Path = "Source -> Provision -> Regulation -> SpeedLimitValueBased -> mphValue",
                                Rule = "'mphValue' cannot be zero",
                            };

                            errors.Add(newError);
                        }

                        passedInType = selectedRegulation.GetValueOrDefault<string>(Constants.Type);
                        isValidType = Constants.SpeedLimitValueTypes.Any(passedInType.Equals);
                        if (!isValidType)
                        {
                            SemanticValidationError newError = new()
                            {
                                Name = "Speed Limit Value Type Regulation",
                                Message = "Speed limit value type value indicated",
                                Path = "Source -> Provision -> Regulation -> SpeedLimitValueBased -> type",
                                Rule = $"'type' must be one of '{string.Join(",", Constants.SpeedLimitValueTypes)}'",
                            };

                            errors.Add(newError);
                        }
                        break;
                    case "SpeedLimitProfileBased":
                        selectedRegulation = existingRegulation.GetValueOrDefault<ExpandoObject>(regulationType);
                        passedInType = selectedRegulation.GetValueOrDefault<string>(Constants.Type);
                        isValidType = Constants.SpeedLimitProfileTypes.Any(passedInType.Equals);
                        if (!isValidType)
                        {
                            SemanticValidationError newError = new()
                            {
                                Name = "Speed Limit Value Based Regulation",
                                Message = "Speed limit based value indicated",
                                Path = "Source -> Provision -> Regulation -> SpeedLimitProfileBased -> type",
                                Rule = $"'type' must be one of '{string.Join(",", Constants.SpeedLimitProfileTypes)}'",
                            };
                            errors.Add(newError);
                        }
                        break;
                    case "GeneralRegulation":
                        selectedRegulation = existingRegulation.GetValueOrDefault<ExpandoObject>(regulationType);
                        passedInType = selectedRegulation.GetValueOrDefault<string>(Constants.RegulationType);
                        isValidType = Constants.RegulationTypes.Any(passedInType.Equals);
                        if (!isValidType)
                        {
                            SemanticValidationError newError = new()
                            {
                                Name = "General Regulation",
                                Message = "Object indicating a specific regulation (other than speed limit or user-defined off-list regulation)",
                                Path = "Source -> Provision -> Regulation -> GeneralRegulation -> regulationType",
                                Rule = $"'type' must be one of '{string.Join(",", Constants.RegulationTypes)}'",
                            };
                            errors.Add(newError);
                        }
                        break;
                    case "OffListRegulation":
                        selectedRegulation = existingRegulation.GetValueOrDefault<ExpandoObject>(regulationType);

                        var regulationFullText = selectedRegulation.GetValueOrDefault<string>(Constants.RegulationFullText);
                        if (string.IsNullOrEmpty(regulationFullText))
                        {
                            SemanticValidationError newError = new()
                            {
                                Name = "Off List Regulation",
                                Message = "Full description of the other type of regulation",
                                Path = "Source -> Provision -> Regulation -> OffListRegulation -> regulationFullText",
                                Rule = "'regulationFullText' must be present",
                            };

                            errors.Add(newError);
                        }

                        var regulationShortName = selectedRegulation.GetValueOrDefault<string>(Constants.RegulationShortName);
                        if (string.IsNullOrEmpty(regulationShortName))
                        {
                            SemanticValidationError newError = new()
                            {
                                Name = "Off List Regulation",
                                Message = "User-defined short name for other type of regulation",
                                Path = "Source -> Provision -> Regulation -> OffListRegulation -> regulationShortName",
                                Rule = "'regulationShortName' must be present",
                            };

                            errors.Add(newError);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        return errors;
    }
}