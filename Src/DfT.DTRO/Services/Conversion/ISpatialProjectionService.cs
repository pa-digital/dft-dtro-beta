namespace DfT.DTRO.Services.Conversion;

public interface ISpatialProjectionService
{
    Coordinates Wgs84ToOsgb36(Coordinates coordinates)
        => Wgs84ToOsgb36(coordinates.Longitude, coordinates.Latitude);

    Coordinates Wgs84ToOsgb36(double longitude, double latitude);

    BoundingBox Wgs84ToOsgb36(BoundingBox boundingBox)
        => Wgs84ToOsgb36(
            boundingBox.WestLongitude,
            boundingBox.SouthLatitude,
            boundingBox.EastLongitude,
            boundingBox.NorthLatitude);

    BoundingBox Wgs84ToOsgb36(double westLongitude, double southLatitude, double eastLongitude, double northLatitude);

    BoundingBox Osgb36ToWgs84(double eastLongitude, double southLatitude, double westLongitude, double northLatitude);

    BoundingBox WktSrid27700(double eastLongitude, double southLatitude, double westLongitude, double northLatitude);
}
