using Newtonsoft.Json.Linq;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace DfT.DTRO.Services.Validation.Contracts;

public interface IGeometryValidation
{
    BoundingBox ValidateGeometryAgainstCurrentSchemaVersion(JObject jObject, List<SemanticValidationError> errors);

    BoundingBox ValidateGeometryAgainstPreviousSchemaVersions(JObject jObject, SchemaVersion schemaVersion, List<SemanticValidationError> errors);
}