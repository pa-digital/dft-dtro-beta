namespace DfT.DTRO.Services;

public interface ISchemaTemplateService
{
    Task<GuidResponse> ActivateSchemaTemplateAsync(SchemaVersion schemaVersion);

    Task<GuidResponse> DeActivateSchemaTemplateAsync(SchemaVersion schemaVersion);

    Task<SchemaTemplateResponse> GetSchemaTemplateAsync(SchemaVersion schemaVersion);

    Task<SchemaTemplateResponse> GetSchemaTemplateByIdAsync(Guid id);

    Task<List<SchemaTemplateResponse>> GetSchemaTemplatesAsync();

    Task<List<SchemaTemplateOverview>> GetSchemaTemplatesVersionsAsync();

    Task<GuidResponse> SaveSchemaTemplateAsJsonAsync(string version, ExpandoObject expandoObject, string correlationId);

    Task<GuidResponse> UpdateSchemaTemplateAsJsonAsync(string version, ExpandoObject expandoObject, string correlationId);

    Task<bool> SchemaTemplateExistsAsync(SchemaVersion schemaVersion);

    Task<bool> DeleteSchemaTemplateAsync(string version);
}