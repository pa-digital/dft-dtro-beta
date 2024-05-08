using DfT.DTRO.Models.DataBase;
using DfT.DTRO.Models.SchemaTemplate;
using System.Collections.Generic;

namespace DfT.DTRO.Services.Mapping;

/// <summary>
/// Provides methods used for mapping <see cref="SchemaTemplate"/> to other types.
/// </summary>
public interface ISchemaTemplateMappingService
{
    /// <summary>
    /// A mapping service for a db object to a external contract    /// </summary>
    /// <param name="schemaTemplate">The <see cref="SchemaTemplate"/> to infer index fields for.</param>
    SchemaTemplateResponse MapToSchemaTemplateResponse(SchemaTemplate schemaTemplate);

    /// <summary>
    /// A mapping service for a list of db objects to a external contract    /// </summary>
    /// <param name="schemaTemplates">The <see cref="SchemaTemplate"/> to infer index fields for.</param>
    List<SchemaTemplateResponse> MapToSchemaTemplateResponse(List<SchemaTemplate> schemaTemplates);
}
