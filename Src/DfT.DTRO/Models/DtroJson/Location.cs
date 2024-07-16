using System.ComponentModel.DataAnnotations;
using DfT.DTRO.Converters;
using Newtonsoft.Json;

namespace DfT.DTRO.Models.DtroJson;

public class Location
{
    [Required]
    [RegularExpression(
        "^(?:osgb36Epsg27700|wgs84Epsg4326)$",
        ErrorMessage = "Value must be one of 'osgb36Epsg27700' and 'wgs84Epsg4326'.")]
    public string Crs { get; set; }

    [JsonConverter(typeof(BoundingBoxJsonConverter))]
    [Required]
    public BoundingBox Bbox { get; set; }
}