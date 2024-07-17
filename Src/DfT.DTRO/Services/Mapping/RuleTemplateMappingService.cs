using DfT.DTRO.Models.RuleTemplate;

namespace DfT.DTRO.Services.Mapping;

public class RuleTemplateMappingService : IRuleTemplateMappingService
{
    public RuleTemplateMappingService()
    {
    }

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
