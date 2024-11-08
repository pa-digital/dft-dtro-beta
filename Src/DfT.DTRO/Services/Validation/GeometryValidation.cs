using DfT.DTRO.Helpers;
using DfT.DTRO.Models.Validation;
using Newtonsoft.Json.Linq;

namespace DfT.DTRO.Services.Validation;

public class GeometryValidation : IGeometryValidation
{
    private readonly IBoundingBoxService _boundingBoxService;

    public GeometryValidation(IBoundingBoxService boundingBoxService)
    {
        _boundingBoxService = boundingBoxService;
    }

    public BoundingBox ValidateCoordinatesAgainstBoundingBoxesForCurrentSchemaVersion(JObject data,
       List<SemanticValidationError> errors)
    {
        BoundingBox boundingBox = new();
        JObject jObject = new();

        List<JProperty> geometries = data
            .Descendants()
            .OfType<JProperty>()
            .Where(property => Constants.ConcreteGeometries.Any(property.Name.Contains))
            .ToList();

        foreach (JProperty geometry in geometries)
        {
            if (geometry?.Value is not JObject)
            {
                SemanticValidationError semanticValidationError = new()
                {
                    Message = $"'geometry' is of type '{geometry?.Value.Type}', this it must be an 'object'."
                };

                errors.Add(semanticValidationError);
            }

            jObject = geometry?.Value as JObject;

            if (jObject != null && !jObject.TryGetValue("version", out JToken _))
            {
                SemanticValidationError semanticValidationError = new()
                {
                    Message = "'version' was missing."
                };
                errors.Add(semanticValidationError);
            }

            if (jObject != null && jObject.TryGetValue("version", out JToken value))
            {
                JTokenType type = value.Type;
                if (type != JTokenType.Integer)
                {
                    SemanticValidationError semanticValidationError = new()
                    {
                        Message = "'version' must be an integer."
                    };
                    errors.Add(semanticValidationError);
                }
            }
        }

        JProperty jProperty = geometries.FirstOrDefault();
        return _boundingBoxService.SetBoundingBoxForMultipleGeometries(errors, jProperty, boundingBox);
    }

    public BoundingBox ValidateCoordinatesAgainstBoundingBoxesForLowerSchemaVersions(JObject data, List<SemanticValidationError> errors)
    {
        JProperty geometry = data
            .DescendantsAndSelf()
            .OfType<JProperty>()
            .FirstOrDefault(property => property.Name == "geometry");

        BoundingBox boundingBox = new();
        if (geometry?.Value is not JObject)
        {
            errors.Add(new SemanticValidationError
            {
                Message = $"'geometry' is of type '{geometry?.Value.Type}', this it must be an 'object'."
            });
        }

        JObject jObject = geometry?.Value as JObject;

        if (jObject != null && !jObject.TryGetValue("version", out JToken _))
        {
            errors.Add(new SemanticValidationError
            {
                Message = "'version' was missing."
            });
        }

        if (jObject != null && jObject.TryGetValue("version", out JToken value))
        {
            JTokenType type = value.Type;
            if (type != JTokenType.Integer)
            {
                errors.Add(new SemanticValidationError
                {
                    Message = "'version' must be an integer."
                });
            }
        }

        return _boundingBoxService.SetBoundingBoxForSingleGeometry(errors, jObject, boundingBox);
    }

}