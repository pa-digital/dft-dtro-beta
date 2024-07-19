using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DfT.DTRO.Filters;

public class BoundingBoxSchemaFilter : ISchemaFilter
{
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