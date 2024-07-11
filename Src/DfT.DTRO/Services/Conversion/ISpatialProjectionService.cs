using DfT.DTRO.Models.DtroJson;

namespace DfT.DTRO.Services.Conversion;

/// <summary>
/// Provides utilities to convert between coordinate reference systems.
/// </summary>
public interface ISpatialProjectionService
{
    /// <summary>
    /// Projects coordinates from wgs84epsg4326 to osgb36epsg27700.
    /// </summary>
    /// <param name="coordinates">The coordinates to project.</param>
    /// <returns>The coordinates resulting from the projection.</returns>
    Coordinates Wgs84ToOsgb36(Coordinates coordinates)
        => Wgs84ToOsgb36(coordinates.Longitude, coordinates.Latitude);

    /// <summary>
    /// Projects coordinates from wgs84epsg4326 to osgb36epsg27700
    /// and produces a result of type <see cref="Coordinates"/>.
    /// </summary>
    /// <returns>The coordinates resulting from the projection.</returns>
    Coordinates Wgs84ToOsgb36(double longitude, double latitude);

    /// <summary>
    /// Projects bounding box from wgs84epsg4326 to osgb36epsg27700.
    /// </summary>
    /// <param name="boundingBox">The bounding box to project.</param>
    /// <returns>The coordinates resulting from the projection.</returns>
    BoundingBox Wgs84ToOsgb36(BoundingBox boundingBox)
        => Wgs84ToOsgb36(
            boundingBox.WestLongitude,
            boundingBox.SouthLatitude,
            boundingBox.EastLongitude,
            boundingBox.NorthLatitude);

    /// <summary>
    /// Projects bounding box coordinates from wgs84epsg4326 to osgb36epsg27700
    /// and produces a result of type <see cref="BoundingBox"/>.
    /// </summary>
    /// <param name="westLongitude">West Longitude.</param>
    /// <param name="southLatitude">South Latitude.</param>
    /// <param name="eastLongitude">East Longitude.</param>
    /// <param name="northLatitude">Nort Latitude.</param>
    /// <returns>The bounding box resulting from the projection.</returns>
    BoundingBox Wgs84ToOsgb36(double westLongitude, double southLatitude, double eastLongitude, double northLatitude);

    /// <summary>
    /// Projects bounding box coordinates from osgb36epsg27700 to wgs84epsg4326
    /// and produces a result of type <see cref="BoundingBox" />.
    /// </summary>
    /// <param name="eastLongitude">East Longitude.</param>
    /// <param name="southLatitude">South Latitude.</param>
    /// <param name="westLongitude">West Longitude.</param>
    /// <param name="northLatitude">Nort Latitude.</param>
    /// <returns>The bounding box resulting from the projection.</returns>
    BoundingBox Osgb36ToWgs84(double eastLongitude, double southLatitude, double westLongitude, double northLatitude);
}
