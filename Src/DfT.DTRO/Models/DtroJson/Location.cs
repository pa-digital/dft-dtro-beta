using DfT.DTRO.Converters;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace DfT.DTRO.Models.DtroJson;

/// <summary>
/// A structure of the location query.
/// </summary>
public class Location
{
    /// <summary>
    /// Coordinate reference system used to defined the bounding box.
    /// </summary>
    /// <example>osgb36Epsg27700.</example>
    [Required]
    [RegularExpression(
        "^(?:osgb36Epsg27700|wgs84Epsg4326)$",
        ErrorMessage = "Value must be one of 'osgb36Epsg27700' and 'wgs84Epsg4326'.")]
    public string Crs { get; set; }

    /// <summary>
    /// Bounding box defining location of DTROs requested in the query.
    /// </summary>
    [JsonConverter(typeof(BoundingBoxJsonConverter))]
    [Required]
    public BoundingBox Bbox { get; set; }
}