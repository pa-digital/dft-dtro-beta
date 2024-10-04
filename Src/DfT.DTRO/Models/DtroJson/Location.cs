using Newtonsoft.Json;

namespace DfT.DTRO.Models.DtroJson;

public class Location
{
    [Required]
    [RegularExpression(
        "^(?:osgb36Epsg27700|wgs84Epsg4326|wkt)$",
        ErrorMessage = "Value must be one of 'osgb36Epsg27700', 'wgs84Epsg4326', or 'wkt'.")]
    public string Format { get; set; }

    [JsonConverter(typeof(BoundingBoxJsonConverter))]
    [Required]
    public BoundingBox Bbox { get; set; }
}