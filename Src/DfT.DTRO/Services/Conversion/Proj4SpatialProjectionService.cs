using DotSpatial.Projections;

namespace DfT.DTRO.Services.Conversion;

public class Proj4SpatialProjectionService : ISpatialProjectionService
{
    private static readonly ProjectionInfo Osgb36ProjectionInfo =
        ProjectionInfo.FromProj4String(
            "+proj=tmerc +lat_0=49 +lon_0=-2 +k=0.9996012717 " +
            "+x_0=400000 +y_0=-100000 +ellps=airy +datum=OSGB36 " +
            "+units=m +no_defs");

    private static readonly ProjectionInfo Wgs84ProjectionInfo =
        ProjectionInfo.FromProj4String("+proj=longlat +ellps=WGS84 +datum=WGS84 +no_defs");

    public Coordinates Wgs84ToOsgb36(double longitude, double latitude)
    {
        var coordinateArray = new double[2] { longitude, latitude };
        var altitude = new double[1] { 0 };

        Reproject.ReprojectPoints(
            xy: coordinateArray,
            z: altitude,
            source: Wgs84ProjectionInfo,
            dest: Osgb36ProjectionInfo,
            startIndex: 0,
            numPoints: 1);

        return new Coordinates(coordinateArray[0], coordinateArray[1]);
    }

    public BoundingBox Wgs84ToOsgb36(double eastLongitude, double southLatitude, double westLongitude, double northLatitude)
    {
        var coordinateArray = new double[4] { eastLongitude, southLatitude, westLongitude, northLatitude };
        var altitude = new double[2] { 0, 1 };

        Reproject.ReprojectPoints(
            xy: coordinateArray,
            z: altitude,
            source: Wgs84ProjectionInfo,
            dest: Osgb36ProjectionInfo,
            startIndex: 0,
            numPoints: 2);

        return new BoundingBox(coordinateArray[0], coordinateArray[1], coordinateArray[2], coordinateArray[3]);
    }

    public BoundingBox Osgb36ToWgs84(double eastLongitude, double southLatitude, double westLongitude, double northLatitude)
    {
        var coordinateArray = new double[4] { eastLongitude, southLatitude, westLongitude, northLatitude };
        var altitude = new double[2] { 0, 1 };

        Reproject.ReprojectPoints(
            xy: coordinateArray,
            z: altitude,
            source: Osgb36ProjectionInfo,
            dest: Wgs84ProjectionInfo,
            startIndex: 0,
            numPoints: 2);

        return new BoundingBox(coordinateArray[0], coordinateArray[1], coordinateArray[2], coordinateArray[3]);
    }

    public BoundingBox WktSrid27700(double eastLongitude, double southLatitude, double westLongitude, double northLatitude)
    {
        var coordinateArray = new double[4] { eastLongitude, southLatitude, westLongitude, northLatitude };
        var altitude = new double[2] { 0, 1 };

        Reproject.ReprojectPoints(
            xy: coordinateArray,
            z: altitude,
            source: Wgs84ProjectionInfo,
            dest: Osgb36ProjectionInfo,
            startIndex: 0,
            numPoints: 2);

        return new BoundingBox(coordinateArray[0], coordinateArray[1], coordinateArray[2], coordinateArray[3]);
    }
}
