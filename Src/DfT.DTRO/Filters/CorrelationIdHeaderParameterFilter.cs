using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.SwaggerGen;

public class CorrelationIdHeaderParameterFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "X-Correlation-ID",
            In = ParameterLocation.Header,
            Required = true,
            Schema = new OpenApiSchema
            {
                Type = "string",
                Default = new OpenApiString(CorrelationSettings.CorrelationId.ToString()),
                Description = "UUID formatted string to track the request through the enquiries stack."
            }
        });
    }
}
