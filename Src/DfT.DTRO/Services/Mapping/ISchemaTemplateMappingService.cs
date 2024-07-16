using System.Collections.Generic;
using DfT.DTRO.Models.DataBase;
using DfT.DTRO.Models.SchemaTemplate;

namespace DfT.DTRO.Services.Mapping;

public interface ISchemaTemplateMappingService
{
    SchemaTemplateResponse MapToSchemaTemplateResponse(SchemaTemplate schemaTemplate);

    List<SchemaTemplateResponse> MapToSchemaTemplateResponse(List<SchemaTemplate> schemaTemplates);
}
