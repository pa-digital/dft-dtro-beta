using DfT.DTRO.Models.DataBase;
using DfT.DTRO.Models.SchemaTemplate;
using System.Collections.Generic;

namespace DfT.DTRO.Services.Mapping;

/// <inheritdoc cref="ISchemaTemplateMappingService"/>
public class SchemaTemplateMappingService : ISchemaTemplateMappingService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SchemaTemplateMappingService"/> class.
    /// </summary>
    public SchemaTemplateMappingService()
    {
    }

    /// <inheritdoc/>
    public SchemaTemplateResponse MapToSchemaTemplateResponse(SchemaTemplate schemaTemplate)
    {
        var result = new SchemaTemplateResponse()
        {
            SchemaVersion = schemaTemplate.SchemaVersion,
            Template = schemaTemplate.Template,
            IsActive = schemaTemplate.IsActive
        };

        return result;
    }

    public List<SchemaTemplateResponse> MapToSchemaTemplateResponse(List<SchemaTemplate> schemaTemplates)
    {
        var list = new List<SchemaTemplateResponse>();
        foreach (var schemaTemplate in schemaTemplates)
        {
            list.Add(MapToSchemaTemplateResponse(schemaTemplate));
        }

        return list;
    }
}
