using DfT.DTRO.Models.Errors;
using DfT.DTRO.Models.RuleTemplate;
using DfT.DTRO.Models.SharedResponse;
using DfT.DTRO.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SchemaVersion = DfT.DTRO.Models.SchemaTemplate.SchemaVersion;

namespace DfT.DTRO.Services;

/// <summary>
/// An implementation of <see cref="IRuleTemplateService"/>
/// </summary>
public class RuleTemplateService : IRuleTemplateService
{
    private readonly IRuleTemplateDal _ruleTemplateDal;
    private readonly IDtroDal _dtroDal;
    private readonly IRuleTemplateMappingService _ruleTemplateMappingService;

    /// <summary>
    /// The default constructor.
    /// </summary>
    /// <param name="ruleTemplateDal">An <see cref="IRuleTemplateDal"/> instance.</param>
    /// <param name="dtroDal">An <see cref="IDtroDal"/> instance.</param>
    /// <param name="ruleTemplateMappingService">An <see cref="IRuleTemplateMappingService"/> instance.</param>
    public RuleTemplateService(IRuleTemplateDal ruleTemplateDal, IDtroDal dtroDal, IRuleTemplateMappingService ruleTemplateMappingService)
    {
        _ruleTemplateDal = ruleTemplateDal;
        _dtroDal = dtroDal;
        _ruleTemplateMappingService = ruleTemplateMappingService;
    }

    /// <inheritdoc/>
    public async Task<bool> RuleTemplateExistsAsync(SchemaVersion schemaVersion)
    {
        return await _ruleTemplateDal.RuleTemplateExistsAsync(schemaVersion);
    }

    /// <inheritdoc/>
    public async Task<RuleTemplateResponse> GetRuleTemplateByIdAsync(Guid id)
    {
        var ruleTemplateExists = await _ruleTemplateDal.RuleTemplateExistsByIdAsync(id);
        if (!ruleTemplateExists)
        {
            throw new NotFoundException("Rule Template not found");
        }

        var dbrec = await _ruleTemplateDal.GetRuleTemplateByIdAsync(id);
        var res = _ruleTemplateMappingService.MapToRuleTemplateResponse(dbrec);
        return res;
    }

    /// <inheritdoc/>
    public async Task<RuleTemplateResponse> GetRuleTemplateAsync(SchemaVersion schemaVersion)
    {
        var ruleTemplateExists = await _ruleTemplateDal.RuleTemplateExistsAsync(schemaVersion);
        if (!ruleTemplateExists)
        {
            throw new NotFoundException("Rule Template not found");
        }

        var dbrec = await _ruleTemplateDal.GetRuleTemplateAsync(schemaVersion);
        var res = _ruleTemplateMappingService.MapToRuleTemplateResponse(dbrec);
        return res;
    }

    /// <inheritdoc/>
    public async Task<List<RuleTemplateResponse>> GetRuleTemplatesAsync()
    {
        var templates = await _ruleTemplateDal.GetRuleTemplatesAsync();
        var templatesResponse = _ruleTemplateMappingService.MapToRuleTemplateResponse(templates);
        return templatesResponse;
    }

    /// <inheritdoc/>
    public async Task<List<RuleTemplateOverview>> GetRuleTemplatesVersionsAsync()
    {
        return await _ruleTemplateDal.GetRuleTemplatesVersionsAsync();
    }

    /// <inheritdoc/>
    public async Task<GuidResponse> SaveRuleTemplateAsJsonAsync(string version, string rule, string correlationId)
    {
        var ruleTemplateExists = await _ruleTemplateDal.RuleTemplateExistsAsync(version);
        if (ruleTemplateExists)
        {
            throw new InvalidOperationException("Rule Template already Exists");
        }

        return await _ruleTemplateDal.SaveRuleTemplateAsJsonAsync(version, rule, correlationId);
    }

    /// <inheritdoc/>
    public async Task<GuidResponse> UpdateRuleTemplateAsJsonAsync(string version, string rule, string correlationId)
    {
        var ruleTemplateExists = await _ruleTemplateDal.RuleTemplateExistsAsync(version);
        if (!ruleTemplateExists)
        {
            throw new NotFoundException();
        }

        var countDtros = await _dtroDal.DtroCountForSchemaAsync(version);

        if (countDtros > 0)
        {
            throw new InvalidOperationException($"Cannot update the schema as in use by exising DTRO's");
        }

        return await _ruleTemplateDal.UpdateRuleTemplateAsJsonAsync(version, rule, correlationId);
    }
}