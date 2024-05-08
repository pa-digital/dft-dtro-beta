using DfT.DTRO.Models.DataBase;
using DfT.DTRO.Models.RuleTemplate;
using System.Collections.Generic;

namespace DfT.DTRO.Services.Mapping;

/// <summary>
/// Provides methods used for mapping <see cref="RuleTemplate"/> to other types.
/// </summary>
public interface IRuleTemplateMappingService
{
    /// <summary>
    /// A mapping service for a db object to a external contract    /// </summary>
    /// <param name="ruleTemplate">The <see cref="RuleTemplate"/> to infer index fields for.</param>
    RuleTemplateResponse MapToRuleTemplateResponse(RuleTemplate ruleTemplate);

    /// <summary>
    /// A mapping service for a list of db objects to a external contract    /// </summary>
    /// <param name="ruleTemplates">The <see cref="RuleTemplate"/> to infer index fields for.</param>
    List<RuleTemplateResponse> MapToRuleTemplateResponse(List<RuleTemplate> ruleTemplates);
}
