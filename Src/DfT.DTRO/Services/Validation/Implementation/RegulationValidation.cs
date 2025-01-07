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
        .Where(regulation => Constants.ExistingRegulations.Any(regulation.HasField))
        .ToList();

        foreach (var existingRegulation in existingRegulations)
        {
            foreach (var regulationType in Constants.ExistingRegulations.Where(existingRegulation.HasField))
            {
                switch (regulationType)
                {
                    case "SpeedLimitValueBased":
                        var selectedRegulation = existingRegulation.GetValueOrDefault<ExpandoObject>(regulationType);
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

                        var passedInType = selectedRegulation.GetValueOrDefault<string>(Constants.Type);
                        var isValidType = Constants.SpeedLimitValueType.Any(passedInType.Equals);
                        if (!isValidType)
                        {
                            SemanticValidationError newError = new()
                            {
                                Name = "Speed Limit Value Based Regulation",
                                Message = "Speed limit type value indicated",
                                Path = "Source -> Provision -> Regulation -> SpeedLimitValueBased -> type",
                                Rule = $"'type' must be one of '{string.Join(",", Constants.SpeedLimitValueType)}'",
                            };

                            errors.Add(newError);
                        }

                        break;
                    case "SpeedLimitProfileBased":
                        break;
                    case "GeneralRegulation":
                        break;
                    case "OffListRegulation":
                        break;
                    default:
                        break;
                }
            }
        }


        //var regulationTypes = typeof(RegulationType)
        //    .GetDisplayNames<RegulationType>()
        //    .ToList();

        //var passedInRegulations = regulations
        //    .(regulation => regulation.Select(kv => kv.Key))
        //    .ToList();

        //var areMultipleRegulations = regulations.Count > 1;
        //if (areMultipleRegulations)
        //{
        //    var acceptedRegulations = passedInRegulations
        //        .Where(passedInRegulation =>
        //            regulationTypes.Any(passedInRegulation.Contains));

        //    var hasTemporaryRegulation = acceptedRegulations
        //        .Any(acceptedRegulation =>
        //            acceptedRegulation
        //                .Equals(RegulationType.TemporaryRegulation.GetDisplayName()));

        //    if (!hasTemporaryRegulation)
        //    {
        //        SemanticValidationError error = new()
        //        {
        //            Name = "Regulations",
        //            Message = "If more than one regulation is present, the temporary regulation must be one of. " +
        //                      "Otherwise, you have to have only one regulation in place.",
        //            Path = "Source -> Provision -> Regulation -> TemporaryRegulation",
        //            Rule = "One regulation must be present.",
        //        };
        //        errors.Add(error);
        //    }
        //}

        //var areAnyAcceptedRegulations = passedInRegulations
        //    .Any(passedInRegulation =>
        //        regulationTypes.Any(passedInRegulation.Contains));

        //if (areAnyAcceptedRegulations)
        //{
        //    return errors;
        //}

        //SemanticValidationError newError = new()
        //{
        //    Name = "Regulations",
        //    Message = "You have to have only one accepted regulation.",
        //    Path = "Source -> Provision -> Regulation",
        //    Rule = $"One of '{string.Join(", ", regulationTypes)}' regulation must be present.",
        //};

        //errors.Add(newError);

        return errors;
    }
}