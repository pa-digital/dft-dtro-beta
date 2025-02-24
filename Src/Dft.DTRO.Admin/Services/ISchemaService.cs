
namespace Dft.DTRO.Admin.Services;
public interface ISchemaService
{
    Task ActivateSchemaAsync(string version);

    Task CreateSchemaAsync(string version, IFormFile file);

    Task DeactivateSchemaAsync(string version);

    Task<List<SchemaTemplateOverview>> GetSchemaVersionsAsync();

    Task UpdateSchemaAsync(string version, IFormFile file);

    Task DeleteSchemaAsync(string version);
}