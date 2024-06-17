
/// <summary>
/// Bounding box definition.
/// </summary>
/// <param name="westLongitude">The west longitude of the bounding box.</param>
/// <param name="southLatitude">The south latitude of the bounding box.</param>
/// <param name="eastLongitude">The east longitude of the bounding box.</param>
/// <param name="northLatitude">The north latitude of the bounding box.</param>
public record struct BoundingBox(double westLongitude, double southLatitude, double eastLongitude, double northLatitude)
{
    /// <summary>
    /// A bounding box that limits allowed coordinates for <c>crs == "osgb36Epsg27700"</c>
    /// <br/><br/>
    /// defined as <c>[-103976.3, -16703.87, 652897.98, 1199851.44]</c>.
    /// </summary>
    public static readonly BoundingBox ForOsgb36Epsg27700 = new(-103976.3, -16703.87, 652897.98, 1199851.44);

    /// <summary>
    /// A bounding box that limits allowed coordinates for <c>crs == "osgb36Epsg27700"</c>
    /// <br/><br/>
    /// defined as <c>[-7.5600, 49.9600, 1.7800, 60.8400]</c>.
    /// </summary>
    public static readonly BoundingBox ForWgs84Epsg4326 = new(-7.5600, 49.9600, 1.7800, 60.8400);
}



