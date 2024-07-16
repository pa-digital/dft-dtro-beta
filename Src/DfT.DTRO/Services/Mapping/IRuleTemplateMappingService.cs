using System.Collections.Generic;
using DfT.DTRO.Models.DataBase;
using DfT.DTRO.Models.RuleTemplate;

namespace DfT.DTRO.Services.Mapping;

public interface IRuleTemplateMappingService
{
    RuleTemplateResponse MapToRuleTemplateResponse(RuleTemplate ruleTemplate);

    List<RuleTemplateResponse> MapToRuleTemplateResponse(List<RuleTemplate> ruleTemplates);
}
