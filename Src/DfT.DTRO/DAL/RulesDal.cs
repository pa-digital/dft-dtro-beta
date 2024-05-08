using DfT.DTRO.DAL;
using DfT.DTRO.JsonLogic;
using DfT.DTRO.Models.DataBase;
using DfT.DTRO.Models.RuleTemplate;
using DfT.DTRO.Models.SharedResponse;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using SchemaVersion = DfT.DTRO.Models.SchemaTemplate.SchemaVersion;

namespace DfT.DTRO.Services;

/// <summary>
/// An implementation of <see cref="IRuleTemplateDal"/>
/// that uses an SQL database as its store.
/// </summary>
///
[ExcludeFromCodeCoverage]
public class RuleTemplateDal : IRuleTemplateDal
{
    private readonly DtroContext _dtroContext;

    /// <summary>
    /// The default constructor.
    /// </summary>
    /// <param name="dtroContext">
    /// An instance of <see cref="DtroContext"/>
    /// representing the current database session.
    /// </param>
    public RuleTemplateDal(DtroContext dtroContext)
    {
        _dtroContext = dtroContext;
    }

    public async Task<bool> RuleTemplateExistsAsync(SchemaVersion schemaVersion)
    {
        var exists = await _dtroContext.RuleTemplate.AnyAsync(it => it.SchemaVersion == schemaVersion);
        return exists;
    }

    public async Task<bool> RuleTemplateExistsByIdAsync(Guid id)
    {
        var exists = await _dtroContext.RuleTemplate.AnyAsync(it => it.Id == id);
        return exists;
    }

    public async Task<RuleTemplate> GetRuleTemplateByIdAsync(Guid id)
    {
        return await _dtroContext.RuleTemplate.FindAsync(id);
    }

    public async Task<RuleTemplate> GetRuleTemplateAsync(SchemaVersion schemaVersion)
    {
        var ret = await _dtroContext.RuleTemplate.FirstOrDefaultAsync(b => b.SchemaVersion == schemaVersion);
        return ret;
    }

    public async Task<IEnumerable<JsonLogicValidationRule>> GetRuleTemplateDeserializeAsync(SchemaVersion schemaVersion)
    {
        var dalRule = await _dtroContext.RuleTemplate.FirstOrDefaultAsync(b => b.SchemaVersion == schemaVersion);
        var rules = System.Text.Json.JsonSerializer.Deserialize<JsonLogicValidationRule[]>(dalRule.Template);

        return rules.ToList();
    }

    public async Task<List<RuleTemplate>> GetRuleTemplatesAsync()
    {
        var templates = await _dtroContext.RuleTemplate.ToListAsync();
        return templates;
    }

    public async Task<List<RuleTemplateOverview>> GetRuleTemplatesVersionsAsync()
    {
        var versions = await _dtroContext.RuleTemplate
         .Select(e => new RuleTemplateOverview
         {
             SchemaVersion = e.SchemaVersion
         })
         .ToListAsync();

        return versions;
    }

    public async Task<GuidResponse> SaveRuleTemplateAsJsonAsync(string version, string rule, string correlationId)
    {
        var ruleTemplate = new RuleTemplate();
        var response = new GuidResponse();

        ruleTemplate.Id = response.Id;
        ruleTemplate.Template = rule;
        ruleTemplate.SchemaVersion = version;
        ruleTemplate.LastUpdated = DateTime.UtcNow;
        ruleTemplate.Created = ruleTemplate.LastUpdated;

        ruleTemplate.LastUpdatedCorrelationId = correlationId;
        ruleTemplate.CreatedCorrelationId = ruleTemplate.LastUpdatedCorrelationId;
        if (await RuleTemplateExistsAsync(version))
        {
            throw new InvalidOperationException($"There is an existing Schema Template with Schema Version {version}");
        }

        await _dtroContext.RuleTemplate.AddAsync(ruleTemplate);

        await _dtroContext.SaveChangesAsync();
        return response;
    }

    public async Task<GuidResponse> UpdateRuleTemplateAsJsonAsync(string version, string rule, string correlationId)
    {
        if (!await RuleTemplateExistsAsync(version))
        {
            throw new InvalidOperationException($"There is no Schema Template with Schema Version {version}");
        }

        var existing = await GetRuleTemplateAsync(version);
        existing.Template = rule;
        existing.LastUpdated = DateTime.UtcNow;
        existing.LastUpdatedCorrelationId = correlationId;

        await _dtroContext.SaveChangesAsync();
        return new GuidResponse() { Id = existing.Id };
    }
}