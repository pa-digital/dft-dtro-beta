namespace DfT.DTRO.DAL;

/// <summary>
/// Service interface providing methods that storage Schema Templates.
/// </summary>
public interface ISchemaTemplateDal
{
    /// <summary>
    /// Activate schema template by <paramref name="schemaVersion"/>
    /// </summary>
    /// <param name="schemaVersion">Schema version to activate by.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous activate operation.</returns>
    Task<GuidResponse> ActivateSchemaTemplateAsync(SchemaVersion schemaVersion);

    /// <summary>
    /// Deactivate schema template by <paramref name="schemaVersion"/>
    /// </summary>
    /// <param name="schemaVersion">Schema version to deactivate by.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous deactivate operation.</returns>
    Task<GuidResponse> DeActivateSchemaTemplateAsync(SchemaVersion schemaVersion);

    /// <summary>
    /// Get schema template by <paramref name="schemaVersion"/>
    /// </summary>
    /// <param name="schemaVersion">Schema version to get schema template by.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous get operation.</returns>
    Task<SchemaTemplate> GetSchemaTemplateAsync(SchemaVersion schemaVersion);

    /// <summary>
    /// Get schema template by <paramref name="id"/>
    /// </summary>
    /// <param name="id">Schema template ID to get schema template by.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous get operation.</returns>
    Task<SchemaTemplate> GetSchemaTemplateByIdAsync(Guid id);

    /// <summary>
    /// Get schema templates
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous get operation.</returns>
    Task<List<SchemaTemplate>> GetSchemaTemplatesAsync();

    /// <summary>
    /// Get schema template versions.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous get operation.</returns>
    Task<List<SchemaTemplateOverview>> GetSchemaTemplatesVersionsAsync();

    /// <summary>
    /// Save a schema template by its <paramref name="version"/>
    /// </summary>
    /// <param name="version">Schema version to get schema template by.</param>
    /// <param name="expandoObject">Expando object representing a schema template.</param>
    /// <param name="correlationId">Correlation ID passed when submitting the schema template.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous save operation.</returns>
    Task<GuidResponse> SaveSchemaTemplateAsJsonAsync(string version, ExpandoObject expandoObject, string correlationId);

    /// <summary>
    /// Update an existing schema template by its <paramref name="version"/>
    /// </summary>
    /// <param name="version">Schema version to get schema template by.</param>
    /// <param name="expandoObject">Expando object representing a schema template.</param>
    /// <param name="correlationId">Correlation ID passed when submitting the schema template.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous save operation.</returns>
    Task<GuidResponse> UpdateSchemaTemplateAsJsonAsync(string version, ExpandoObject expandoObject, string correlationId);

    /// <summary>
    /// Check if schema template exists by <paramref name="schemaVersion"/>
    /// </summary>
    /// <param name="schemaVersion">Schema version by which schema template check done by.</param>
    /// <returns>
    /// A <see cref="Task"/> whose result is <see langword="true" /> if schema template
    /// with specified schema version exists. Otherwise <see langword="false" />
    /// </returns>
    Task<bool> SchemaTemplateExistsAsync(SchemaVersion schemaVersion);

    /// <summary>
    /// Check if schema template exists by <paramref name="id"/>
    /// </summary>
    /// <param name="id">Schema template ID by which schema template check done by.</param>
    /// <returns>
    /// A <see cref="Task"/> whose result is <see langword="true" /> if schema template
    /// with specified schema ID exists. Otherwise <see langword="false" />
    /// </returns>
    Task<bool> SchemaTemplateExistsByIdAsync(Guid id);


    /// <summary>
    /// Delete schema template by <paramref name="version"/>
    /// </summary>
    /// <param name="version">Schema template version by which schema template deletion is done by.</param>
    /// <returns>
    /// A <see cref="Task"/> whose result is <see langword="true"/> if schema template
    /// with specified schema version is deleted. Otherwise <see langword="false"/>
    /// </returns>
    Task<bool> DeleteSchemaTemplateByVersionAsync(string version);
}