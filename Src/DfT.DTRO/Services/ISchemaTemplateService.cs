using DfT.DTRO.Models.SchemaTemplate;
using DfT.DTRO.Models.SharedResponse;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace DfT.DTRO.Services;

/// <summary>
/// An implementation of <see cref="ISchemaTemplateService"/>.
/// </summary>
public interface ISchemaTemplateService
{
    /// <summary>
    /// Updates the schema template to active.
    /// </summary>
    /// <param name="schemaVersion">The unique identifier of the schema.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous update operation.</returns>
    Task<GuidResponse> ActivateSchemaTemplateAsync(SchemaVersion schemaVersion);

    /// <summary>
    /// Updates the schema template to not active.
    /// </summary>
    /// <param name="schemaVersion">The unique identifier of the schema.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous update operation.</returns>
    Task<GuidResponse> DeActivateSchemaTemplateAsync(SchemaVersion schemaVersion);

    /// <summary>
    /// Returns a schema specified in <paramref name="schemaVersion"/>.
    /// </summary>
    /// <param name="schemaVersion">The Schema Version required.</param>
    /// <returns>A <see cref="SchemaVersion"/> instance.</returns>
    Task<SchemaTemplateResponse> GetSchemaTemplateAsync(SchemaVersion schemaVersion);

    /// <summary>
    /// Returns a schema specified in <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The Guid of Schema Version required.</param>
    /// <returns>A <see cref="SchemaVersion"/> instance.</returns>
    Task<SchemaTemplateResponse> GetSchemaTemplateByIdAsync(Guid id);

    /// <summary>
    /// Returns all schemas templates.
    /// </summary>
    /// <returns>A <see cref="Task"/> that resolves to a collection of <see cref="SchemaTemplateResponse"/> .</returns>
    Task<List<SchemaTemplateResponse>> GetSchemaTemplatesAsync();

    /// <summary>
    /// Returns all schemas template versions.
    /// </summary>
    /// <returns>A <see cref="Task"/> that resolves to a collection of <see cref="SchemaVersion"/> .</returns>
    Task<List<SchemaTemplateOverview>> GetSchemaTemplatesVersionsAsync();

    /// <summary>
    /// Saves a Schema Template provided in <paramref name="expandoObject"/> to a storage device
    /// after converting it to a JSON string.
    /// </summary>
    /// <param name="version">The Schema Version required.</param>
    /// <param name="expandoObject">The Schema Template Json content.</param>
    /// <param name="correlationId">The Correlation Id.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous save operation.</returns>
    Task<GuidResponse> SaveSchemaTemplateAsJsonAsync(string version, ExpandoObject expandoObject, string correlationId);

    /// <summary>
    /// Updates a Schema Template provided in <paramref name="expandoObject"/> to a storage device
    /// after converting it to a JSON string.
    /// </summary>
    /// <param name="version">The Schema Version required.</param>
    /// <param name="expandoObject">The Schema Template Json content.</param>
    /// <param name="correlationId">The Correlation Id.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous update operation.</returns>
    Task<GuidResponse> UpdateSchemaTemplateAsJsonAsync(string version, ExpandoObject expandoObject, string correlationId);

    /// <summary>
    /// Checks if the Schema Template exists in the storage.
    /// </summary>
    /// <param name="schemaVersion">The schemaVersion of the Schema Template.</param>
    /// <returns>
    /// A <see cref="Task"/> whose result is <see langword="true"/>
    /// if a Schema Template with the specified Schema Version exists;
    /// otherwise <see langword="false"/>.
    /// </returns>
    Task<bool> SchemaTemplateExistsAsync(SchemaVersion schemaVersion);
}