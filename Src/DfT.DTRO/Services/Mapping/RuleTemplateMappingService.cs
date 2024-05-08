using DfT.DTRO.Models.DataBase;
using DfT.DTRO.Models.RuleTemplate;
using System.Collections.Generic;

namespace DfT.DTRO.Services.Mapping;

/// <inheritdoc cref="IRuleTemplateMappingService"/>
public class RuleTemplateMappingService : IRuleTemplateMappingService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RuleTemplateMappingService"/> class.
    /// </summary>
    public RuleTemplateMappingService()
    {
    }

    /// <inheritdoc/>
    public RuleTemplateResponse MapToRuleTemplateResponse(RuleTemplate ruleTemplate)
    {
        var result = new RuleTemplateResponse()
        {
            SchemaVersion = ruleTemplate.SchemaVersion,
            Template = ruleTemplate.Template
        };

        return result;
    }

    public List<RuleTemplateResponse> MapToRuleTemplateResponse(List<RuleTemplate> ruleTemplates)
    {
        var list = new List<RuleTemplateResponse>();
        foreach (var ruleTemplate in ruleTemplates)
        {
            list.Add(MapToRuleTemplateResponse(ruleTemplate));
        }

        return list;
    }
}
