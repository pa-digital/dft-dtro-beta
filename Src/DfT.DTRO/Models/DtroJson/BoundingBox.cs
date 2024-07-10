using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DfT.DTRO.Models.Validation;
using NpgsqlTypes;

namespace DfT.DTRO.Models.DtroJson;
/// <summary>
/// Bounding box definition.
/// </summary>
/// <param name="WestLongitude">The west longitude of the bounding box.</param>
/// <param name="SouthLatitude">The south latitude of the bounding box.</param>
/// <param name="EastLongitude">The east longitude of the bounding box.</param>
/// <param name="NorthLatitude">The north latitude of the bounding box.</param>
public record struct BoundingBox(double WestLongitude, double SouthLatitude, double EastLongitude, double NorthLatitude)
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

    /// <summary>
    /// Checks if the coordinates are within the bounding box.
    /// </summary>
    /// <param name="longitude">The longitude to verify.</param>
    /// <param name="latitude">The latitude to verify.</param>
    /// <returns>
    /// <see langword="true"/> if the coordinates are within the bounding box;
    /// otherwise <see langword="false"/>.
    /// </returns>
    public bool Contains(double longitude, double latitude)
        => latitude >= SouthLatitude && latitude <= NorthLatitude &&
            longitude >= WestLongitude && longitude <= EastLongitude;

    /// <summary>
    /// Checks if the coordinates are within the bounding box.
    /// </summary>
    /// <param name="longitude">The longitude to verify.</param>
    /// <param name="latitude">The latitude to verify.</param>
    /// <param name="errors">
    /// A <see cref="BoundingBoxErrors"/> object that explains the errors.
    /// <br/><br/>
    /// <see langword="null"/> if there were no errors (the method returned <see langword="true"/>).
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the coordinates are within the bounding box;
    /// otherwise <see langword="false"/>.
    /// </returns>
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

    /// <summary>
    /// Checks if the coordinates are within the bounding box.
    /// </summary>
    /// <param name="coordinates">The coordinates to verify.</param>
    /// <returns>
    /// <see langword="true"/> if the coordinates are within the bounding box;
    /// otherwise <see langword="false"/>.
    /// </returns>
    public bool Contains(Coordinates coordinates)
        => Contains(coordinates.Longitude, coordinates.Latitude);

    /// <summary>
    /// Checks if the coordinates are within the bounding box.
    /// </summary>
    /// <param name="coordinates">The coordinates to verify.</param>
    /// <param name="errors">
    /// A <see cref="BoundingBoxErrors"/> object that explains the errors.
    /// <br/><br/>
    /// <see langword="null"/> if there were no errors (the method returned <see langword="true"/>).
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the coordinates are within the bounding box;
    /// otherwise <see langword="false"/>.
    /// </returns>
    public bool Contains(Coordinates coordinates, [NotNullWhen(false)] out BoundingBoxErrors errors)
        => Contains(coordinates.Longitude, coordinates.Latitude, out errors);

    /// <summary>
    /// Determines whether this instance has any overlapping area
    /// with the <see cref="BoundingBox"/> provided in the argument.
    /// </summary>
    /// <param name="other">The <see cref="BoundingBox"/> to check for overlap with.</param>
    /// <returns>
    /// <see langword="true"/> if the overlap exists; otherwise <see langword="false"/>.
    /// </returns>
    public bool Overlaps(BoundingBox other)
        => SouthLatitude <= other.NorthLatitude &&
            other.SouthLatitude <= NorthLatitude &&
            WestLongitude <= other.EastLongitude &&
            other.WestLongitude <= EastLongitude;

    /// <summary>
    /// Converts a <see cref="BoundingBox"/> to an <see cref="NpgsqlBox"/>.
    /// </summary>
    public static implicit operator NpgsqlBox(BoundingBox bbox)
    {
        return new NpgsqlBox(bbox.NorthLatitude, bbox.EastLongitude, bbox.SouthLatitude, bbox.WestLongitude);
    }

    /// <summary>
    /// Creates a minimal <see cref="BoundingBox"/> that contains all the provided coordinates.
    /// </summary>
    /// <param name="coordinates">The coordinates that should be contained within the bounding box.</param>
    /// <returns>A <see cref="BoundingBox"/> that contains all of provided coordinates.</returns>
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