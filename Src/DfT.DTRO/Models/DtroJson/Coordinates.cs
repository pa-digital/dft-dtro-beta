namespace DfT.DTRO.Models.DtroJson;

/// <summary>
/// Represents the geographic coordinates.
/// </summary>
/// <param name="longitude">The longitude (or x coordinate).</param>
/// <param name="latitude">The latitude (or y coordinate).</param>
public record struct Coordinates(double longitude, double latitude);
