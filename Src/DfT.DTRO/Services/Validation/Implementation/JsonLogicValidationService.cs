using System.Text.Json.Nodes;
using DfT.DTRO.Models.Validation;
using DfT.DTRO.Services.Validation.Contracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DfT.DTRO.Services.Validation.Implementation;

public class JsonLogicValidationService : IJsonLogicValidationService
{
    private readonly IRuleTemplateDal _ruleTemplateDal;

    public JsonLogicValidationService(IRuleTemplateDal ruleTemplateDal)
    {
        _ruleTemplateDal = ruleTemplateDal;
    }

    public async Task<IList<SemanticValidationError>> ValidateRules(DtroSubmit request, string schemaVersion)
    {
        if (request.SchemaVersion < schemaVersion)
        {
            return new List<SemanticValidationError>();
        }

        var rules = await _ruleTemplateDal.GetRuleTemplateDeserializeAsync(request.SchemaVersion);

        var errors = new List<SemanticValidationError>();

        var json = JsonConvert.SerializeObject(request.Data, new ExpandoObjectConverter());
        var node = JsonNode.Parse(json);

        foreach (var rule in rules)
        {
            var result = rule.Rule.Apply(node);
            if (result != null && result.AsValue().TryGetValue(out bool value) && !value)
            {
                SemanticValidationError error = new()
                {
                    Message = rule.Message,
                    Path = rule.Path,
                    Name = rule.Name,
                    Rule = rule.Rule.ToIndentedJsonString()
                };

                errors.Add(error);
            }
        }

        return errors;
    }

    public IList<SemanticValidationError> ValidateRegulatedPlacesType(DtroSubmit request, SchemaVersion schemaVersion)
    {
        var errors = new List<SemanticValidationError>();

        if (schemaVersion < new SchemaVersion("3.2.5"))
        {
            return errors;
        }

        List<ExpandoObject> regulatedPlaces = request
            .Data
            .GetValueOrDefault<IList<object>>("Source.provision")
            .OfType<ExpandoObject>()
            .SelectMany(expandoObject => expandoObject
                .GetValue<IList<object>>("regulatedPlace")
                .OfType<ExpandoObject>())
            .ToList();

        var existingTypes = regulatedPlaces.Select(it => it.GetValueOrDefault<string>("type")).ToList();
        var regulatedPlaceType = RegulatedPlaceType.RegulationLocation.GetDisplayName();
        if (existingTypes.Contains(regulatedPlaceType))
        {
            return errors;
        }

        SemanticValidationError error = new()
        {
            Name = "Regulate place type",
            Message = "Regulated place type missing or incorrect.",
            Path = "Source -> Provision -> RegulatedPlace",
            Rule = $"'{RegulatedPlaceType.RegulationLocation}' type must be present.",
        };
        errors.Add(error);

        return errors;
    }

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

        var areMultipleRegulations = regulations.Count > 1;
        if (areMultipleRegulations)
        {
            SemanticValidationError error = new()
            {
                Name = "Regulations",
                Message = "You have to have only one regulation in place.",
                Path = "Source -> Provision -> Regulation",
                Rule = "One regulation must be present.",
            };
            errors.Add(error);
        }

        var regulationTypes = typeof(RegulationType).GetDisplayNames<RegulationType>().ToList();
        var passedInRegulations = regulations.SelectMany(regulation => regulation.Select(kv => kv.Key)).ToList();
        var areAnyAcceptedRegulations = passedInRegulations.Any(passedInRegulation => regulationTypes.Any(passedInRegulation.Contains));


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

    public IList<SemanticValidationError> ValidateCondition(DtroSubmit dtroSubmit, SchemaVersion schemaVersion)
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

        List<ExpandoObject> conditionSets = regulations
            .SelectMany(expandoObject => expandoObject
                .GetValue<IList<object>>("conditionSet")
                .OfType<ExpandoObject>())
            .ToList();

        var operatorTypes = typeof(OperatorType)
            .GetDisplayNames<OperatorType>()
            .ToList();
        var passedInOperators = conditionSets
            .Select(passedInOperator => passedInOperator
                .GetValueOrDefault<string>("operator"))
            .ToList();
        var hasOperator = passedInOperators
            .All(it => operatorTypes.Any(it.Contains));

        if (!hasOperator)
        {
            SemanticValidationError error = new()
            {
                Name = "Condition set",
                Message = "You have to have at least one operator for the condition set.",
                Path = "Source -> Provision -> Regulation -> ConditionSet",
                Rule = $"One of '{string.Join(", ", operatorTypes)}' operators must be present.",
            };
            errors.Add(error);
        }

        List<ExpandoObject> conditions =
            conditionSets
                .Select(conditionSet => conditionSet
                    .GetValueOrDefault<ExpandoObject>("condition"))
                .ToList();

        var conditionTypes = typeof(ConditionType)
            .GetDisplayNames<ConditionType>()
            .ToList();
        var passedInConditions = conditions
            .SelectMany(condition => condition
                .Select(kv => kv.Key))
            .ToList();
        var areEachConditionsAccepted = passedInConditions
            .Select(passedInCondition => conditionTypes
                .Any(passedInCondition.Contains))
            .ToList();


        if (!areEachConditionsAccepted.All(it => it))
        {
            SemanticValidationError error = new()
            {
                Name = "Condition",
                Message = "You have to have at least one accepted condition.",
                Path = "Source -> Provision -> Regulation -> Condition",
                Rule = $"One or more of '{string.Join(", ", conditionTypes)}' conditions must be present.",
            };
            errors.Add(error);
        }
        return errors;
    }
}
