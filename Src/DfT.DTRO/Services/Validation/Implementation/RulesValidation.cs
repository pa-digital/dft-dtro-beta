using System.Text.Json.Nodes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DfT.DTRO.Services.Validation.Implementation;

public class RulesValidation : IRulesValidation
{
    private readonly IRuleTemplateDal _ruleTemplateDal;

    public RulesValidation(IRuleTemplateDal ruleTemplateDal)
    {
        _ruleTemplateDal = ruleTemplateDal;
    }

    public async Task<List<SemanticValidationError>> ValidateRules(DtroSubmit request, string schemaVersion)
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
}
