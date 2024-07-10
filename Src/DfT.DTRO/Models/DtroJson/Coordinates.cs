namespace DfT.DTRO.Models.DtroJson;

/// <summary>
/// Represents the geographic coordinates.
/// </summary>
/// <param name="Longitude">The longitude (or x coordinate).</param>
/// <param name="Latitude">The latitude (or y coordinate).</param>
public record struct Coordinates(double Longitude, double Latitude);
