using DfT.DTRO.Models.RuleTemplate;
using DfT.DTRO.Models.SchemaTemplate;
using DfT.DTRO.Models.SharedResponse;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DfT.DTRO.Services;

/// <summary>
/// An implementation of <see cref="IRuleTemplateService"/>.
/// </summary>
public interface IRuleTemplateService
{
    /// <summary>
    /// Returns a schema specified in <paramref name="schemaVersion"/>.
    /// </summary>
    /// <param name="schemaVersion">The Schema Version required.</param>
    /// <returns>A <see cref="SchemaVersion"/> instance.</returns>
    Task<RuleTemplateResponse> GetRuleTemplateAsync(SchemaVersion schemaVersion);

    /// <summary>
    /// Returns a schema specified in <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The Guid of Schema Version required.</param>
    /// <returns>A <see cref="SchemaVersion"/> instance.</returns>
    Task<RuleTemplateResponse> GetRuleTemplateByIdAsync(Guid id);

    /// <summary>
    /// Returns all rules templates.
    /// </summary>
    /// <returns>A <see cref="Task"/> that resolves to a collection of <see cref="RuleTemplateResponse"/> .</returns>
    Task<List<RuleTemplateResponse>> GetRuleTemplatesAsync();

    /// <summary>
    /// Returns all rules template versions.
    /// </summary>
    /// <returns>A <see cref="Task"/> that resolves to a collection of <see cref="SchemaVersion"/> .</returns>
    Task<List<RuleTemplateOverview>> GetRuleTemplatesVersionsAsync();

    /// <summary>
    /// Saves a Rule Template provided in <paramref name="rule"/> to a storage device
    /// </summary>
    /// <param name="version">The Schema Version required.</param>
    /// <param name="rule">The Rule Template Json content.</param>
    /// <param name="correlationId">The Correlation Id.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous save operation.</returns>
    Task<GuidResponse> SaveRuleTemplateAsJsonAsync(string version, string rule, string correlationId);

    /// <summary>
    /// Updates a Rule Template provided in <paramref name="rule"/> to a storage device
    /// </summary>
    /// <param name="version">The Schema Version required.</param>
    /// <param name="rule">The Rule Template Json content.</param>
    /// <param name="correlationId">The Correlation Id.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous update operation.</returns>
    Task<GuidResponse> UpdateRuleTemplateAsJsonAsync(string version, string rule, string correlationId);

    /// <summary>
    /// Checks if the Rule Template exists in the storage.
    /// </summary>
    /// <param name="schemaVersion">The schemaVersion of the Rule Template.</param>
    /// <returns>
    /// A <see cref="Task"/> whose result is <see langword="true"/>
    /// if a Rule Template with the specified Schema Version exists;
    /// otherwise <see langword="false"/>.
    /// </returns>
    Task<bool> RuleTemplateExistsAsync(SchemaVersion schemaVersion);
}