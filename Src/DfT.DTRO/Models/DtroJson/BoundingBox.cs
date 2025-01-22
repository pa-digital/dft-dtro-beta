using Newtonsoft.Json.Linq;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DfT.DTRO.Models.DtroJson;
public record struct BoundingBox(double WestLongitude, double SouthLatitude, double EastLongitude, double NorthLatitude)
{
    public static readonly BoundingBox ForOsgb36Epsg27700 = new(-103976.3, -16703.87, 652897.98, 1199851.44);

    public static readonly BoundingBox ForWgs84Epsg4326 = new(-7.5600, 49.9600, 1.7800, 60.8400);

    public static readonly BoundingBox ForWktSrid27700 = new(0, 0, 700000, 1300000);

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

    public static implicit operator NpgsqlBox(BoundingBox boundingBox) =>
        new(boundingBox.NorthLatitude,
            boundingBox.EastLongitude,
            boundingBox.SouthLatitude,
            boundingBox.WestLongitude);

    public BoundingBox ValidateAgainstBoundingBox(IEnumerable<JToken> values)
    {
        List<string> points = values
            .Value<string>()
            .Split(" ")
            .Select(it => it
                .Replace("(", "")
                .Replace(")", "")
                .Replace(",", ""))
            .ToList();

        List<Coordinates> coordinates = points
            .Select(point => new Coordinates
            {
                Longitude = point.AsInt(),
                Latitude = point.AsInt()
            })
            .ToList();

        return Wrapping(coordinates);
    }

    private static BoundingBox Wrapping(IEnumerable<Coordinates> coordinates)
    {
        IEnumerable<Coordinates> coordinatesEnumerable = coordinates.ToList();

        (double east, double south) = coordinatesEnumerable.First();
        double north = south;
        double west = east;

        foreach (Coordinates coordinate in coordinatesEnumerable.Skip(1))
        {
            if (coordinate.Latitude < south)
            {
                south = coordinate.Latitude;
            }
            else if (coordinate.Latitude > north)
            {
                north = coordinate.Latitude;
            }

            if (coordinate.Longitude < west)
            {
                west = coordinate.Longitude;
            }
            else if (coordinate.Longitude > east)
            {
                east = coordinate.Longitude;
            }
        }

        return new BoundingBox(west, south, east, north);
    }
}