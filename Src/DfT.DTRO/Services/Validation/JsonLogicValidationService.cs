using System.Text.Json.Nodes;
using DfT.DTRO.Models.Validation;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DfT.DTRO.Services.Validation;

public class JsonLogicValidationService : IJsonLogicValidationService
{
    private readonly IRuleTemplateDal _ruleTemplateDal;

    public JsonLogicValidationService(IRuleTemplateDal ruleTemplateDal)
    {
        _ruleTemplateDal = ruleTemplateDal;
    }

    public async Task<IList<SemanticValidationError>> ValidateCreationRequest(DtroSubmit request, string schemaVersion)
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
                    Path = rule.Path
                };

                errors.Add(error);
            }
        }

        return errors;
    }
}
