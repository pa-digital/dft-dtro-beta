using DfT.DTRO.JsonLogic;
using DfT.DTRO.Models.DataBase;
using DfT.DTRO.Models.RuleTemplate;
using DfT.DTRO.Models.SchemaTemplate;
using DfT.DTRO.Models.SharedResponse;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DfT.DTRO.Services;
public interface IRuleTemplateDal
{
    Task<IEnumerable<JsonLogicValidationRule>> GetRuleTemplateDeserializeAsync(SchemaVersion schemaVersion);

    Task<RuleTemplate> GetRuleTemplateAsync(SchemaVersion schemaVersion);

    Task<RuleTemplate> GetRuleTemplateByIdAsync(Guid id);

    Task<List<RuleTemplate>> GetRuleTemplatesAsync();

    Task<List<RuleTemplateOverview>> GetRuleTemplatesVersionsAsync();

    Task<bool> RuleTemplateExistsAsync(SchemaVersion schemaVersion);

    Task<bool> RuleTemplateExistsByIdAsync(Guid id);

    Task<GuidResponse> SaveRuleTemplateAsJsonAsync(string version, string rule, string correlationId);

    Task<GuidResponse> UpdateRuleTemplateAsJsonAsync(string version, string rule, string correlationId);
}