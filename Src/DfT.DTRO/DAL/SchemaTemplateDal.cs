namespace DfT.DTRO.DAL;

/// <summary>
/// Implementation of the <see cref="ISchemaTemplateDal"/> service.
/// </summary>
[ExcludeFromCodeCoverage]
public class SchemaTemplateDal : ISchemaTemplateDal
{
    private readonly DtroContext _dtroContext;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="dtroContext"><see cref="DtroContext"/> database context.</param>
    public SchemaTemplateDal(DtroContext dtroContext) => _dtroContext = dtroContext;

    ///<inheritdoc cref="ISchemaTemplateDal"/>
    public async Task<GuidResponse> ActivateSchemaTemplateAsync(SchemaVersion schemaVersion)
    {
        if (!await SchemaTemplateExistsAsync(schemaVersion))
        {
            throw new InvalidOperationException($"There is no Schema Template with Schema Version {schemaVersion}");
        }

        var existing = await GetSchemaTemplateAsync(schemaVersion);
        existing.IsActive = true;
        await _dtroContext.SaveChangesAsync();
        return new GuidResponse() { Id = existing.Id };
    }

    ///<inheritdoc cref="ISchemaTemplateDal"/>
    public async Task<GuidResponse> DeActivateSchemaTemplateAsync(SchemaVersion schemaVersion)
    {
        if (!await SchemaTemplateExistsAsync(schemaVersion))
        {
            throw new InvalidOperationException($"There is no Schema Template with Schema Version {schemaVersion}");
        }

        var existing = await GetSchemaTemplateAsync(schemaVersion);
        existing.IsActive = false;
        await _dtroContext.SaveChangesAsync();
        return new GuidResponse() { Id = existing.Id };
    }

    ///<inheritdoc cref="ISchemaTemplateDal"/>
    public async Task<bool> SchemaTemplateExistsAsync(SchemaVersion schemaVersion)
    {
        var exists = await _dtroContext.SchemaTemplate.AnyAsync(it => it.SchemaVersion == schemaVersion);
        return exists;
    }

    ///<inheritdoc cref="ISchemaTemplateDal"/>
    public async Task<bool> SchemaTemplateExistsByIdAsync(Guid id)
    {
        var exists = await _dtroContext.SchemaTemplate.AnyAsync(it => it.Id == id);
        return exists;
    }

    /// <inheritdoc cref="ISchemaTemplateDal"/>
    public async Task<bool> DeleteSchemaTemplateByVersionAsync(string version)
    {
        var schemaTemplate = await _dtroContext.SchemaTemplate.FirstOrDefaultAsync(it => it.SchemaVersion == version);
        _dtroContext.SchemaTemplate.Remove(schemaTemplate);
        await _dtroContext.SaveChangesAsync();
        return true;
    }

    ///<inheritdoc cref="ISchemaTemplateDal"/>
    public async Task<SchemaTemplate> GetSchemaTemplateByIdAsync(Guid id)
    {
        return await _dtroContext.SchemaTemplate.FindAsync(id);
    }

    ///<inheritdoc cref="ISchemaTemplateDal"/>
    public async Task<SchemaTemplate> GetSchemaTemplateAsync(SchemaVersion schemaVersion)
    {
        var ret = await _dtroContext.SchemaTemplate.FirstOrDefaultAsync(b => b.SchemaVersion == schemaVersion);
        return ret;
    }

    ///<inheritdoc cref="ISchemaTemplateDal"/>
    public async Task<List<SchemaTemplate>> GetSchemaTemplatesAsync()
    {
        var templates = await _dtroContext.SchemaTemplate.ToListAsync();
        return templates;
    }

    ///<inheritdoc cref="ISchemaTemplateDal"/>
    public async Task<List<SchemaTemplateOverview>> GetSchemaTemplatesVersionsAsync()
    {
        var versions = await (from schema in _dtroContext.SchemaTemplate
                              join rule in _dtroContext.RuleTemplate on schema.SchemaVersion equals rule.SchemaVersion into rules
                              from ruleGroup in rules.DefaultIfEmpty()
                              select new SchemaTemplateOverview
                              {
                                  SchemaVersion = schema.SchemaVersion.ToString(),
                                  IsActive = schema.IsActive,
                                  RulesExist = ruleGroup != null
                              }).ToListAsync();

        return versions;
    }

    ///<inheritdoc cref="ISchemaTemplateDal"/>
    public async Task<GuidResponse> SaveSchemaTemplateAsJsonAsync(string version, ExpandoObject expandoObject, string correlationId)
    {
        var schemaTemplate = new SchemaTemplate();
        var response = new GuidResponse();

        schemaTemplate.Id = response.Id;
        schemaTemplate.Template = expandoObject;
        schemaTemplate.SchemaVersion = version;
        schemaTemplate.LastUpdated = DateTime.UtcNow;
        schemaTemplate.Created = schemaTemplate.LastUpdated;

        schemaTemplate.LastUpdatedCorrelationId = correlationId;
        schemaTemplate.CreatedCorrelationId = schemaTemplate.LastUpdatedCorrelationId;
        if (await SchemaTemplateExistsAsync(version))
        {
            throw new InvalidOperationException($"There is an existing Schema Template with Schema Version {version}");
        }

        await _dtroContext.SchemaTemplate.AddAsync(schemaTemplate);

        await _dtroContext.SaveChangesAsync();
        return response;
    }

    ///<inheritdoc cref="ISchemaTemplateDal"/>
    public async Task<GuidResponse> UpdateSchemaTemplateAsJsonAsync(string version, ExpandoObject expandoObject, string correlationId)
    {
        if (!await SchemaTemplateExistsAsync(version))
        {
            throw new InvalidOperationException($"There is no Schema Template with Schema Version {version}");
        }

        var existing = await GetSchemaTemplateAsync(version);
        existing.Template = expandoObject;
        existing.LastUpdated = DateTime.UtcNow;
        existing.LastUpdatedCorrelationId = correlationId;

        await _dtroContext.SaveChangesAsync();
        return new GuidResponse() { Id = existing.Id };
    }
}