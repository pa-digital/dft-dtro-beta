using System.Diagnostics.CodeAnalysis;
using DfT.DTRO.Models.Validation;
using NpgsqlTypes;

namespace DfT.DTRO.Models.DtroJson;
public record struct BoundingBox(double WestLongitude, double SouthLatitude, double EastLongitude, double NorthLatitude)
{
    public static readonly BoundingBox ForOsgb36Epsg27700 = new(-103976.3, -16703.87, 652897.98, 1199851.44);

    public static readonly BoundingBox ForWgs84Epsg4326 = new(-7.5600, 49.9600, 1.7800, 60.8400);

    public bool Contains(double longitude, double latitude)
        => latitude >= SouthLatitude && latitude <= NorthLatitude &&
            longitude >= WestLongitude && longitude <= EastLongitude;

    public bool Contains(double longitude, double latitude, [NotNullWhen(false)] out BoundingBoxErrors errors)
    {
        string longitudeError = null, latitudeError = null;

        if (longitude <= WestLongitude)
        {
            longitudeError = $"{longitude} is below the minimum longitude of {WestLongitude}.";
        }
        else if (longitude >= EastLongitude)
        {
            longitudeError = $"{longitude} is above the maximum longitude of {EastLongitude}.";
        }

        if (latitude <= SouthLatitude)
        {
            latitudeError = $"{latitude} is below the minimum latitude of {SouthLatitude}.";
        }
        else if (latitude >= NorthLatitude)
        {
            latitudeError = $"{latitude} is above the maximum latitude of {NorthLatitude}.";
        }

        if (latitudeError is not null || longitudeError is not null)
        {
            errors = new BoundingBoxErrors
            {
                LatitudeError = latitudeError,
                LongitudeError = longitudeError,
            };
            return false;
        }

        errors = null;
        return true;
    }

    public bool Contains(Coordinates coordinates)
        => Contains(coordinates.Longitude, coordinates.Latitude);

    public bool Contains(Coordinates coordinates, [NotNullWhen(false)] out BoundingBoxErrors errors)
        => Contains(coordinates.Longitude, coordinates.Latitude, out errors);

    public bool Overlaps(BoundingBox other)
        => SouthLatitude <= other.NorthLatitude &&
            other.SouthLatitude <= NorthLatitude &&
            WestLongitude <= other.EastLongitude &&
            other.WestLongitude <= EastLongitude;

    public static implicit operator NpgsqlBox(BoundingBox bbox)
    {
        return new NpgsqlBox(bbox.NorthLatitude, bbox.EastLongitude, bbox.SouthLatitude, bbox.WestLongitude);
    }

    public static BoundingBox Wrapping(IEnumerable<Coordinates> coordinates)
    {
        (double east, double south) = coordinates.First();
        double north = south;
        double west = east;

        foreach (Coordinates crd in coordinates.Skip(1))
        {
            if (crd.Latitude < south)
            {
                south = crd.Latitude;
            }
            else if (crd.Latitude > north)
            {
                north = crd.Latitude;
            }

            if (crd.Longitude < west)
            {
                west = crd.Longitude;
            }
            else if (crd.Longitude > east)
            {
                east = crd.Longitude;
            }
        }

        return new BoundingBox(west, south, east, north);
    }
}