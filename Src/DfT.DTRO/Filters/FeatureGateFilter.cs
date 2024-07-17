using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DfT.DTRO.Filters;

public class FeatureGateFilter : IDocumentFilter
{
    private readonly IFeatureManager _featureManager;

    public FeatureGateFilter(IFeatureManager featureManager)
    {
        _featureManager = featureManager;
    }

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        foreach (var apiDescription in context.ApiDescriptions)
        {
            var filterPipeline = apiDescription.ActionDescriptor.FilterDescriptors;
            var filterMetaData = filterPipeline.Select(filterInfo => filterInfo.Filter).SingleOrDefault(filter => filter is FeatureGateAttribute);
            if (filterMetaData == default)
            {
                continue;
            }

            var featureGateAttribute = filterMetaData as FeatureGateAttribute;

            var isActive = false;

            if (featureGateAttribute.RequirementType == RequirementType.Any)
            {
                isActive = featureGateAttribute.Features.Any(feature =>
                    _featureManager.IsEnabledAsync(feature).GetAwaiter().GetResult());
            }
            else
            {
                isActive = featureGateAttribute.Features.All(feature =>
                    _featureManager.IsEnabledAsync(feature).GetAwaiter().GetResult());
            }

            if (isActive)
            {
                continue;
            }

            var path = swaggerDoc.Paths.FirstOrDefault(o => o.Key.EndsWith(apiDescription.RelativePath));

            path.Value.Operations.Remove(ToOperationType(apiDescription.HttpMethod));

            if (!path.Value.Operations.Any())
            {
                swaggerDoc.Paths.Remove(path.Key);
            }
        }
    }

    private static OperationType ToOperationType(string httpMethod) => httpMethod.ToUpperInvariant() switch
    {
        "GET" => OperationType.Get,
        "POST" => OperationType.Post,
        "PUT" => OperationType.Put,
        "PATCH" => OperationType.Patch,
        "DELETE" => OperationType.Delete,
        "TRACE" => OperationType.Trace,
        "OPTIONS" => OperationType.Options,
        "HEAD" => OperationType.Head,
        _ => throw new InvalidOperationException($"'{httpMethod}' is not a known HTTP method"),
    };
}