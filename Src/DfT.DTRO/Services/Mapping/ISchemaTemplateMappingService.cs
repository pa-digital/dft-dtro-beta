namespace DfT.DTRO.Services.Mapping;

public interface ISchemaTemplateMappingService
{
    SchemaTemplateResponse MapToSchemaTemplateResponse(SchemaTemplate schemaTemplate);

    List<SchemaTemplateResponse> MapToSchemaTemplateResponse(List<SchemaTemplate> schemaTemplates);
}
