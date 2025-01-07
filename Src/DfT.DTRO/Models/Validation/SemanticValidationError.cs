using DfT.DTRO.Helpers;

namespace DfT.DTRO.Models.Validation;

/// <summary>
/// Class responsible with validation errors
/// </summary>
public class SemanticValidationError
{
    /// <summary>
    /// Message returned when validation error
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Path returned when validation error
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// Name of the error
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Rule that must be followed
    /// </summary>
    public string Rule { get; set; }

    /// <summary>
    /// Set error validation when incorrect parameters are passed in the payload.
    /// </summary>
    /// <param name="geometryType">Geometry Type</param>
    /// <returns>this</returns>
    public SemanticValidationError SetIncorrectPairSemanticValidationError(GeometryType geometryType) =>
        new()
        {
            Path = $"Source.provision.regulatedPlace.geometry.{geometryType}",
            Message = "Selected geometry",
            Name = "Incorrect pairs",
            Rule = $"Incorrect pairs for selected geometry: {geometryType} "
        };

    /// <summary>
    /// Set error validation when incorrect spatial reference is passed.
    /// </summary>
    /// <returns>this</returns>
    public SemanticValidationError SetSetSpatialReferenceValidationError() =>
        new()
        {
            Message = "British National Grid - Spatial Reference is not present within the geometry or is referenced incorrectly.",
            Path = "Source.provision.regulatedPlace.geometry",
            Name = "Spatial reference",
            Rule = $"Spatial reference should be '{Constants.Srid27700}'"
        };

    /// <summary>
    /// Set error validation when coordinates are beyond UK boundaries
    /// </summary>
    /// <returns>this</returns>
    public SemanticValidationError SetCoordinatesValidationError() =>
        new()
        {
            Message = "The provided coordinates are outside the recognized UK boundaries.",
            Path = "Source.provision.regulatedPlace.geometry",
            Name = "Wrong coordinates",
            Rule = "Coordinates should be within UK coordinates - POLYGON((0 0, 700000 0, 700000 1300000, 0 1300000, 0 0))"
        };

    /// <summary>
    /// Set error related to the wrong geometry type
    /// </summary>
    /// <returns>this</returns>
    public SemanticValidationError SetGeometryValidationError() =>
        new()
        {
            Path = "Source.provision.regulatedPlace.geometry",
            Message = "Selected geometry ",
            Name = "Wrong geometry",
            Rule = $"Geometry accepted should be one of: {GeometryType.PointGeometry}, {GeometryType.LinearGeometry}, {GeometryType.Polygon} or {GeometryType.DirectedLinear}"
        };
}