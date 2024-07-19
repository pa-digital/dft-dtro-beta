using DfT.DTRO.Models.RuleTemplate;

namespace DfT.DTRO.Services;

public interface IRuleTemplateService
{
    Task<RuleTemplateResponse> GetRuleTemplateAsync(SchemaVersion schemaVersion);

    Task<RuleTemplateResponse> GetRuleTemplateByIdAsync(Guid id);

    Task<List<RuleTemplateResponse>> GetRuleTemplatesAsync();

    Task<List<RuleTemplateOverview>> GetRuleTemplatesVersionsAsync();

    Task<GuidResponse> SaveRuleTemplateAsJsonAsync(string version, string rule, string correlationId);

    Task<GuidResponse> UpdateRuleTemplateAsJsonAsync(string version, string rule, string correlationId);

    Task<bool> RuleTemplateExistsAsync(SchemaVersion schemaVersion);
}