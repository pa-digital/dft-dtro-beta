namespace Dft.DTRO.Admin.Models.Search;

public record struct BoundingBox(double westLongitude, double southLatitude, double eastLongitude, double northLatitude)
{
    public static readonly BoundingBox ForOsgb36Epsg27700 = new(-103976.3, -16703.87, 652897.98, 1199851.44);

    public static readonly BoundingBox ForWgs84Epsg4326 = new(-7.5600, 49.9600, 1.7800, 60.8400);
}



