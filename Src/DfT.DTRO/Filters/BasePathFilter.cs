using System.Text.RegularExpressions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DfT.DTRO.Filters;

public class BasePathFilter : IDocumentFilter
{
    public BasePathFilter(string basePath)
    {
        BasePath = basePath;
    }

    public string BasePath { get; }

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        swaggerDoc.Servers.Add(new OpenApiServer() { Url = BasePath });

        var pathsToModify = swaggerDoc.Paths.Where(p => p.Key.StartsWith(BasePath)).ToList();

        foreach (var path in pathsToModify)
        {
            if (path.Key.StartsWith(BasePath))
            {
                string newKey = Regex.Replace(path.Key, $"^{BasePath}", string.Empty);
                swaggerDoc.Paths.Remove(path.Key);
                swaggerDoc.Paths.Add(newKey, path.Value);
            }
        }
    }
}
