using DfT.DTRO.Models.Validation;
using Newtonsoft.Json.Linq;

namespace DfT.DTRO.Services.Mapping;

public interface IBoundingBoxService
{
    BoundingBox SetBoundingBox(List<SemanticValidationError> errors, JObject obj, BoundingBox boundingBox);
}