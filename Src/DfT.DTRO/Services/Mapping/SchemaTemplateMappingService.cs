using System.Collections.Generic;
using DfT.DTRO.Models.DataBase;
using DfT.DTRO.Models.SchemaTemplate;

namespace DfT.DTRO.Services.Mapping;

public class SchemaTemplateMappingService : ISchemaTemplateMappingService
{
    public SchemaTemplateMappingService()
    {
    }

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
