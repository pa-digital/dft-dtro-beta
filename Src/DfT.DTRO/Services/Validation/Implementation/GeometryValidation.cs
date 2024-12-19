using Newtonsoft.Json.Linq;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DfT.DTRO.Services.Validation.Implementation;

public class GeometryValidation : IGeometryValidation
{
    private readonly IBoundingBoxService _boundingBoxService;
    private readonly LoggingExtension _loggingExtension;

    public GeometryValidation(IBoundingBoxService boundingBoxService, LoggingExtension loggingExtension)
    {
        _boundingBoxService = boundingBoxService;
        _loggingExtension = loggingExtension;
    }

    public BoundingBox ValidateGeometryAgainstCurrentSchemaVersion(JObject data, List<SemanticValidationError> errors)
    {
        BoundingBox boundingBox = new();

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
                    Message = $"'{nameof(geometry)}' is of type '{geometry?.Value.Type}', this it must be an 'object'."
                };

                errors.Add(semanticValidationError);
                _loggingExtension.LogError(nameof(ValidateGeometryAgainstCurrentSchemaVersion), "", "Geometry error", string.Join(",", errors));
            }

            JObject jObject = geometry?.Value as JObject;

            if (jObject != null && !jObject.TryGetValue(Constants.Version, out JToken _))
            {
                SemanticValidationError semanticValidationError = new()
                {
                    Message = $"'{Constants.Version}' was missing."
                };
                errors.Add(semanticValidationError);
                _loggingExtension.LogError(nameof(ValidateGeometryAgainstCurrentSchemaVersion), "", "Version error", string.Join(",", errors));
            }

            if (jObject != null && jObject.TryGetValue(Constants.Version, out JToken value))
            {
                JTokenType type = value.Type;
                if (type != JTokenType.Integer)
                {
                    SemanticValidationError semanticValidationError = new()
                    {
                        Message = $"'{Constants.Version}' must be an integer."
                    };
                    errors.Add(semanticValidationError);
                    _loggingExtension.LogError(nameof(ValidateGeometryAgainstCurrentSchemaVersion), "", "Version type error", string.Join(",", errors));
                }
            }
        }

        JProperty jProperty = geometries.FirstOrDefault();
        return _boundingBoxService.SetBoundingBoxForMultipleGeometries(errors, jProperty, boundingBox);
    }

    public BoundingBox ValidateGeometryAgainstPreviousSchemaVersions(JObject data, SchemaVersion schemaVersion, List<SemanticValidationError> errors)
    {
        JProperty geometry = data
            .DescendantsAndSelf()
            .OfType<JProperty>()
            .FirstOrDefault(property => property.Name == "Geometry".ToBackwardCompatibility(schemaVersion));

        BoundingBox boundingBox = new();
        if (geometry?.Value is not JObject)
        {
            errors.Add(new SemanticValidationError
            {
                Message = $"'{nameof(geometry)}' is of type '{geometry?.Value.Type}', this it must be an 'object'."
            });
            _loggingExtension.LogError(nameof(ValidateGeometryAgainstPreviousSchemaVersions), "", "Geometry error", string.Join(",", errors));

        }

        JObject jObject = geometry?.Value as JObject;

        if (jObject != null && !jObject.TryGetValue(Constants.Version, out JToken _))
        {
            errors.Add(new SemanticValidationError
            {
                Message = $"'{Constants.Version}' was missing."
            });
            _loggingExtension.LogError(nameof(ValidateGeometryAgainstPreviousSchemaVersions), "", "Version error", string.Join(",", errors));
        }

        if (jObject != null && jObject.TryGetValue(Constants.Version, out JToken value))
        {
            JTokenType type = value.Type;
            if (type != JTokenType.Integer)
            {
                errors.Add(new SemanticValidationError
                {
                    Message = $"'{Constants.Version}' must be an integer."
                });
                _loggingExtension.LogError(nameof(ValidateGeometryAgainstPreviousSchemaVersions), "", "Version type error", string.Join(",", errors));
            }
        }

        return _boundingBoxService.SetBoundingBoxForSingleGeometry(errors, geometry, boundingBox);
    }

}