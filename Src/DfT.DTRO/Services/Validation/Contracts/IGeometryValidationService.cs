namespace DfT.DTRO.Services.Validation.Contracts;

/// <summary>
/// Geometry validation service
/// </summary>
public interface IGeometryValidationService
{
    /// <summary>
    /// Validate geometry
    /// </summary>
    /// <param name="dtroSubmit">D-TRO record payload to validate against.</param>
    /// <returns>List of validation errors</returns>
    List<SemanticValidationError> Validate(DtroSubmit dtroSubmit);
}