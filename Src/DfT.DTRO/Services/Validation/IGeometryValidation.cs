using DfT.DTRO.Models.Validation;
using Newtonsoft.Json.Linq;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace DfT.DTRO.Services.Validation;

public interface IGeometryValidation
{
    BoundingBox ValidateCoordinatesAgainstBoundingBoxesForCurrentSchemaVersion(JObject jObject, List<SemanticValidationError> errors);

    BoundingBox ValidateCoordinatesAgainstBoundingBoxesForLowerSchemaVersions(JObject jObject, List<SemanticValidationError> errors);
}