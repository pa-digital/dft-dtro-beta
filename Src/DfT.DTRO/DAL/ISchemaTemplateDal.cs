namespace DfT.DTRO.Services;
public interface ISchemaTemplateDal
{
    Task<GuidResponse> ActivateSchemaTemplateAsync(SchemaVersion schemaVersion);

    Task<GuidResponse> DeActivateSchemaTemplateAsync(SchemaVersion schemaVersion);

    Task<SchemaTemplate> GetSchemaTemplateAsync(SchemaVersion schemaVersion);

    Task<SchemaTemplate> GetSchemaTemplateByIdAsync(Guid id);

    Task<List<SchemaTemplate>> GetSchemaTemplatesAsync();

    Task<List<SchemaTemplateOverview>> GetSchemaTemplatesVersionsAsync();

    Task<GuidResponse> SaveSchemaTemplateAsJsonAsync(string version, ExpandoObject expandoObject, string correlationId);

    Task<GuidResponse> UpdateSchemaTemplateAsJsonAsync(string version, ExpandoObject expandoObject, string correlationId);

    Task<bool> SchemaTemplateExistsAsync(SchemaVersion schemaVersion);

    Task<bool> SchemaTemplateExistsByIdAsync(Guid id);
}