using DfT.DTRO.Models.DtroJson;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DfT.DTRO.Filters;

/// <summary>
/// Filter to set schema of bounding box arguments to array.
/// The bounding box is internally transformed from user-provided array to bounding box object.
/// </summary>
public class BoundingBoxSchemaFilter : ISchemaFilter
{
    /// <inheritdoc />
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(BoundingBox))
        {
            schema.Type = "array";
            schema.MinItems = 4;
            schema.MaxItems = 4;
            schema.Items = new OpenApiSchema { Type = "number", Format = "double" };
            schema.Properties = null;
            schema.Example = new OpenApiArray
            {
                new OpenApiDouble(-103976.3),
                new OpenApiDouble(-16703.87),
                new OpenApiDouble(652897.98),
                new OpenApiDouble(1199851.44)
            };
        }
    }
}