namespace DfT.DTRO.DAL;

/// <summary>
/// Service interface providing methods that storage Rule Templates.
/// </summary>
public interface IRuleTemplateDal
{
    /// <summary>
    /// Get rule templates by <paramref name="schemaVersion"/>
    /// </summary>
    /// <param name="schemaVersion">Schema version to get rule template by.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous get operation.</returns>
    Task<IEnumerable<JsonLogicValidationRule>> GetRuleTemplateDeserializeAsync(SchemaVersion schemaVersion);

    /// <summary>
    /// Get rule template by <paramref name="schemaVersion"/>
    /// </summary>
    /// <param name="schemaVersion">Schema version to get by.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous get operation.</returns>
    Task<RuleTemplate> GetRuleTemplateAsync(SchemaVersion schemaVersion);

    /// <summary>
    /// Get rule template by <paramref name="id"/>
    /// </summary>
    /// <param name="id">Rule template ID.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous get operation.</returns>
    Task<RuleTemplate> GetRuleTemplateByIdAsync(Guid id);

    /// <summary>
    /// Get rule templates
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous get operation.</returns>
    Task<List<RuleTemplate>> GetRuleTemplatesAsync();

    /// <summary>
    /// Get rule template versions.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous get operation.</returns>
    Task<List<RuleTemplateOverview>> GetRuleTemplatesVersionsAsync();

    /// <summary>
    /// Check if rule template exists by <paramref name="schemaVersion"/>
    /// </summary>
    /// <param name="schemaVersion">Schema version to check by.</param>
    /// <returns>
    /// A <see cref="Task"/> whose result is <see langword="true" /> if rule template
    /// with specified schema version exists. Otherwise <see langword="false" />
    /// </returns>
    Task<bool> RuleTemplateExistsAsync(SchemaVersion schemaVersion);

    /// <summary>
    /// Check if rule template exists by <paramref name="id"/>
    /// </summary>
    /// <param name="id">Rule template ID to check by.</param>
    /// <returns>
    /// A <see cref="Task"/> whose result is <see langword="true" /> if rule template
    /// with specified ID exists. Otherwise <see langword="false" />
    /// </returns>
    Task<bool> RuleTemplateExistsByIdAsync(Guid id);

    /// <summary>
    /// Save a rule template by its <paramref name="version"/>
    /// </summary>
    /// <param name="version">Rule version to get rule template by.</param>
    /// <param name="rule">JSON object representing a rule template.</param>
    /// <param name="correlationId">Correlation ID passed when submitting the rule template.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous save operation.</returns>
    Task<GuidResponse> SaveRuleTemplateAsJsonAsync(string version, string rule, string correlationId);

    /// <summary>
    /// Update an existing rule template by its <paramref name="version"/>
    /// </summary>
    /// <param name="version">Rule version to get rule template by.</param>
    /// <param name="rule">JSON object representing a rule template.</param>
    /// <param name="correlationId">Correlation ID passed when submitting the rule template.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous save operation.</returns>
    Task<GuidResponse> UpdateRuleTemplateAsJsonAsync(string version, string rule, string correlationId);
}