using DfT.DTRO.Models.Errors;
using DfT.DTRO.Models.SchemaTemplate;
using DfT.DTRO.Models.SharedResponse;
using DfT.DTRO.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using SchemaVersion = DfT.DTRO.Models.SchemaTemplate.SchemaVersion;

namespace DfT.DTRO.Services;

/// <summary>
/// An implementation of <see cref="ISchemaTemplateService"/>
/// </summary>
public class SchemaTemplateService : ISchemaTemplateService
{
    private readonly ISchemaTemplateDal _schemaTemplateDal;
    private readonly IRuleTemplateDal _ruleTemplateDal;
    private readonly IDtroDal _dtroDal;
    private readonly ISchemaTemplateMappingService _schemaTemplateMappingService;

    /// <summary>
    /// The default constructor.
    /// </summary>
    /// <param name="schemaTemplateDal">An <see cref="ISchemaTemplateDal"/> instance.</param>
    /// <param name="ruleTemplateDal">An <see cref="IRuleTemplateDal"/> instance.</param>
    /// <param name="dtroDal">An <see cref="IDtroDal"/> instance.</param>
    /// <param name="schemaTemplateMappingService">An <see cref="ISchemaTemplateMappingService"/> instance.</param>
    public SchemaTemplateService(ISchemaTemplateDal schemaTemplateDal, IRuleTemplateDal ruleTemplateDal, IDtroDal dtroDal, ISchemaTemplateMappingService schemaTemplateMappingService)
    {
        _schemaTemplateDal = schemaTemplateDal;
        _dtroDal = dtroDal;
        _schemaTemplateMappingService = schemaTemplateMappingService;
        _ruleTemplateDal = ruleTemplateDal;
    }

    /// <inheritdoc/>
    public async Task<GuidResponse> ActivateSchemaTemplateAsync(SchemaVersion schemaVersion)
    {
        var schemaTemplateExists = await _schemaTemplateDal.SchemaTemplateExistsAsync(schemaVersion);
        if (!schemaTemplateExists)
        {
            throw new NotFoundException("Schema Template not found");
        }

        var ruleTemplateExists = await _ruleTemplateDal.RuleTemplateExistsAsync(schemaVersion);
        if (!ruleTemplateExists)
        {
            throw new NotFoundException("Rule Template not found");
        }

        return await _schemaTemplateDal.ActivateSchemaTemplateAsync(schemaVersion);
    }

    /// <inheritdoc/>
    public async Task<GuidResponse> DeActivateSchemaTemplateAsync(SchemaVersion schemaVersion)
    {
        var schemaTemplateExists = await _schemaTemplateDal.SchemaTemplateExistsAsync(schemaVersion);
        if (!schemaTemplateExists)
        {
            throw new NotFoundException();
        }

        return await _schemaTemplateDal.DeActivateSchemaTemplateAsync(schemaVersion);
    }

    /// <inheritdoc/>
    public async Task<bool> SchemaTemplateExistsAsync(SchemaVersion schemaVersion)
    {
        return await _schemaTemplateDal.SchemaTemplateExistsAsync(schemaVersion);
    }

    /// <inheritdoc/>
    public async Task<SchemaTemplateResponse> GetSchemaTemplateByIdAsync(Guid id)
    {
        var schemaTemplateExists = await _schemaTemplateDal.SchemaTemplateExistsByIdAsync(id);
        if (!schemaTemplateExists)
        {
            throw new NotFoundException("Schema Template not found");
        }

        var dbrec = await _schemaTemplateDal.GetSchemaTemplateByIdAsync(id);
        var res = _schemaTemplateMappingService.MapToSchemaTemplateResponse(dbrec);
        return res;
    }

    /// <inheritdoc/>
    public async Task<SchemaTemplateResponse> GetSchemaTemplateAsync(SchemaVersion schemaVersion)
    {
        var schemaTemplateExists = await _schemaTemplateDal.SchemaTemplateExistsAsync(schemaVersion);
        if (!schemaTemplateExists)
        {
            throw new NotFoundException("Schema Template not found");
        }

        var dbrec = await _schemaTemplateDal.GetSchemaTemplateAsync(schemaVersion);
        var res = _schemaTemplateMappingService.MapToSchemaTemplateResponse(dbrec);
        return res;
    }

    /// <inheritdoc/>
    public async Task<List<SchemaTemplateResponse>> GetSchemaTemplatesAsync()
    {
        var templates = await _schemaTemplateDal.GetSchemaTemplatesAsync();
        var templatesResponse = _schemaTemplateMappingService.MapToSchemaTemplateResponse(templates);
        return templatesResponse;
    }

    /// <inheritdoc/>
    public async Task<List<SchemaTemplateOverview>> GetSchemaTemplatesVersionsAsync()
    {
        return await _schemaTemplateDal.GetSchemaTemplatesVersionsAsync();
    }

    /// <inheritdoc/>
    public async Task<GuidResponse> SaveSchemaTemplateAsJsonAsync(string version, ExpandoObject expandoObject, string correlationId)
    {
        var schemaTemplateExists = await _schemaTemplateDal.SchemaTemplateExistsAsync(version);
        if (schemaTemplateExists)
        {
            throw new InvalidOperationException("Schema Template already Exists");
        }

        return await _schemaTemplateDal.SaveSchemaTemplateAsJsonAsync(version, expandoObject, correlationId);
    }

    /// <inheritdoc/>
    public async Task<GuidResponse> UpdateSchemaTemplateAsJsonAsync(string version, ExpandoObject expandoObject, string correlationId)
    {
        var schemaTemplateExists = await _schemaTemplateDal.SchemaTemplateExistsAsync(version);
        if (!schemaTemplateExists)
        {
            throw new NotFoundException();
        }

        var countDtros = await _dtroDal.DtroCountForSchemaAsync(version);

        if (countDtros > 0)
        {
            throw new InvalidOperationException($"Cannot update the schema as in use by exising DTRO's");
        }

        return await _schemaTemplateDal.UpdateSchemaTemplateAsJsonAsync(version, expandoObject, correlationId);
    }
}