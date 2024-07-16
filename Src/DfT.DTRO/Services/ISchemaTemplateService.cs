using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using DfT.DTRO.Models.SchemaTemplate;
using DfT.DTRO.Models.SharedResponse;

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
}