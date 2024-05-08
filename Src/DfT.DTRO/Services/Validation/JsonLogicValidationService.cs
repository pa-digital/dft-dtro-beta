using DfT.DTRO.Models.DtroDtos;
using DfT.DTRO.Models.Validation;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DfT.DTRO.Services.Validation;

/// <inheritdoc/>
public class JsonLogicValidationService : IJsonLogicValidationService
{
    private readonly IRuleTemplateDal _ruleTemplateDal;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="ruleTemplateDal">The <see cref="IRuleTemplateDal"/> used to retrieve the rules.</param>
    public JsonLogicValidationService(IRuleTemplateDal ruleTemplateDal)
    {
        _ruleTemplateDal = ruleTemplateDal;
    }

    /// <inheritdoc/>
    public async Task<IList<SemanticValidationError>> ValidateCreationRequest(DtroSubmit request)
    {
        if (request.SchemaVersion < "3.1.2")
        {
            return new List<SemanticValidationError>();
        }

        var rules = await _ruleTemplateDal.GetRuleTemplateDeserializeAsync(request.SchemaVersion);

        // var rules = await _ruleSource.GetRules($"dtro-{request.SchemaVersion}");
        var errors = new List<SemanticValidationError>();

        var json = JsonConvert.SerializeObject(request.Data, new ExpandoObjectConverter());
        var node = JsonNode.Parse(json);

        foreach (var rule in rules)
        {
            var result = rule.Rule.Apply(node);
            if (result.AsValue().TryGetValue(out bool value) && !value)
            {
                errors.Add(new SemanticValidationError()
                {
                    Message = rule.Message,
                    Path = rule.Path
                });
            }
        }

        return errors;
    }
}
