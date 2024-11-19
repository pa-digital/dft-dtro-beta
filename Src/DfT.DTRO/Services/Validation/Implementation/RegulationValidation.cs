using DfT.DTRO.Models.Validation;
using DfT.DTRO.Services.Validation.Contracts;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DfT.DTRO.Services.Validation.Implementation;

public class RegulationValidation : IRegulationValidation
{
    public IList<SemanticValidationError> ValidateRegulation(DtroSubmit dtroSubmit, SchemaVersion schemaVersion)
    {
        var errors = new List<SemanticValidationError>();

        if (schemaVersion < new SchemaVersion("3.2.5"))
        {
            return errors;
        }

        List<ExpandoObject> regulations = dtroSubmit
            .Data
            .GetValueOrDefault<IList<object>>("Source.provision")
            .OfType<ExpandoObject>()
            .SelectMany(expandoObject => expandoObject
                .GetValue<IList<object>>("regulation")
                .OfType<ExpandoObject>())
            .ToList();

        var regulationTypes = typeof(RegulationType)
            .GetDisplayNames<RegulationType>()
            .ToList();

        var passedInRegulations = regulations
            .SelectMany(regulation => regulation.Select(kv => kv.Key))
            .ToList();

        var areMultipleRegulations = regulations.Count > 1;
        if (areMultipleRegulations)
        {
            var acceptedRegulations = passedInRegulations
                .Where(passedInRegulation =>
                    regulationTypes.Any(passedInRegulation.Contains));

            var hasTemporaryRegulation = acceptedRegulations
                .Any(acceptedRegulation =>
                    acceptedRegulation
                        .Equals(RegulationType.TemporaryRegulation.GetDisplayName()));

            if (!hasTemporaryRegulation)
            {
                SemanticValidationError error = new()
                {
                    Name = "Regulations",
                    Message = "If more than one regulation is present, the temporary regulation must be one of. " +
                              "Otherwise, you have to have only one regulation in place.",
                    Path = "Source -> Provision -> Regulation -> TemporaryRegulation",
                    Rule = "One regulation must be present.",
                };
                errors.Add(error);
            }
        }

        var areAnyAcceptedRegulations = passedInRegulations
            .Any(passedInRegulation =>
                regulationTypes.Any(passedInRegulation.Contains));

        if (!areAnyAcceptedRegulations)
        {
            SemanticValidationError error = new()
            {
                Name = "Regulations",
                Message = "You have to have only one accepted regulation.",
                Path = "Source -> Provision -> Regulation",
                Rule = $"One of '{string.Join(", ", regulationTypes)}' regulation must be present.",
            };
            errors.Add(error);
        }
        return errors;
    }
}