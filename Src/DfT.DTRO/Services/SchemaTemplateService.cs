namespace DfT.DTRO.Services;

public class SchemaTemplateService : ISchemaTemplateService
{
    private readonly ISchemaTemplateDal _schemaTemplateDal;
    private readonly IRuleTemplateDal _ruleTemplateDal;
    private readonly ISchemaTemplateMappingService _schemaTemplateMappingService;

    public SchemaTemplateService(ISchemaTemplateDal schemaTemplateDal, IRuleTemplateDal ruleTemplateDal, ISchemaTemplateMappingService schemaTemplateMappingService)
    {
        _schemaTemplateDal = schemaTemplateDal;
        _schemaTemplateMappingService = schemaTemplateMappingService;
        _ruleTemplateDal = ruleTemplateDal;
    }

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

    public async Task<GuidResponse> DeActivateSchemaTemplateAsync(SchemaVersion schemaVersion)
    {
        var schemaTemplateExists = await _schemaTemplateDal.SchemaTemplateExistsAsync(schemaVersion);
        if (!schemaTemplateExists)
        {
            throw new NotFoundException();
        }

        return await _schemaTemplateDal.DeActivateSchemaTemplateAsync(schemaVersion);
    }

    public async Task<bool> SchemaTemplateExistsAsync(SchemaVersion schemaVersion)
    {
        return await _schemaTemplateDal.SchemaTemplateExistsAsync(schemaVersion);
    }

    ///<inheritdoc cref="ISchemaTemplateService"/>
    public async Task<bool> DeleteSchemaTemplateAsync(string version)
    {
        var schemaTemplate = await GetSchemaTemplateAsync(version);
        if (schemaTemplate.IsActive)
        {
            return false;
        }

        return await _schemaTemplateDal.DeleteSchemaTemplateByVersionAsync(version);
    }

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

    public async Task<List<SchemaTemplateResponse>> GetSchemaTemplatesAsync()
    {
        var templates = await _schemaTemplateDal.GetSchemaTemplatesAsync();
        var templatesResponse = _schemaTemplateMappingService.MapToSchemaTemplateResponse(templates);
        return templatesResponse;
    }

    public async Task<List<SchemaTemplateOverview>> GetSchemaTemplatesVersionsAsync()
    {
        return await _schemaTemplateDal.GetSchemaTemplatesVersionsAsync();
    }

    public async Task<GuidResponse> SaveSchemaTemplateAsJsonAsync(string version, ExpandoObject expandoObject, string correlationId)
    {
        var schemaTemplateExists = await _schemaTemplateDal.SchemaTemplateExistsAsync(version);
        if (schemaTemplateExists)
        {
            throw new InvalidOperationException("Schema Template already Exists");
        }

        return await _schemaTemplateDal.SaveSchemaTemplateAsJsonAsync(version, expandoObject, correlationId);
    }

    public async Task<GuidResponse> UpdateSchemaTemplateAsJsonAsync(string version, ExpandoObject expandoObject, string correlationId)
    {
        var schemaTemplateExists = await _schemaTemplateDal.SchemaTemplateExistsAsync(version);
        if (!schemaTemplateExists)
        {
            throw new NotFoundException();
        }

        return await _schemaTemplateDal.UpdateSchemaTemplateAsJsonAsync(version, expandoObject, correlationId);
    }
}