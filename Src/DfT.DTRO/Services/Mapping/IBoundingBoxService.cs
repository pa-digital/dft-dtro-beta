using DfT.DTRO.Models.Validation;
using Newtonsoft.Json.Linq;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DfT.DTRO.Services.Mapping;

public interface IBoundingBoxService
{
    BoundingBox SetBoundingBoxForSingleGeometry(List<SemanticValidationError> errors, JProperty jProperty, BoundingBox boundingBox);

    BoundingBox SetBoundingBoxForMultipleGeometries(List<SemanticValidationError> errors, JProperty jProperty,
        BoundingBox boundingBox);
}