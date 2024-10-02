using DfT.DTRO.Models.Validation;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Newtonsoft.Json.Linq;
using Coordinates = DfT.DTRO.Models.DtroJson.Coordinates;
using GeometryType = DfT.DTRO.Enums.GeometryType;

namespace DfT.DTRO.Services.Mapping;

public class BoundingBoxService : IBoundingBoxService
{
    public BoundingBox SetBoundingBox(List<SemanticValidationError> errors, JObject obj, BoundingBox boundingBox)
    {
        JProperty children = obj.Children<JProperty>().ElementAt(1);
        IEnumerable<JToken> values;
        string json;
        string geometry;
        JToken token;
        bool isValid;
        string toValidate;
        switch (children.Name)
        {
            case "PointGeometry":
                values = children.DescendantsAndSelf().Skip(3).FirstOrDefault();
                json = values.ToIndentedJsonString();
                if (!json.Contains("SRID=27700;"))
                {
                    errors.Add(new SemanticValidationError
                    {
                        Message = "British National Grid - Spatial Reference not present in the geometry or it is wrong referenced.",
                        Path = "Source.provision.regulatedPlace.geometry"
                    });

                    return new BoundingBox();
                }
                toValidate = json.GetBetween(";", "\"");
                isValid = IsInUk(errors, toValidate, GeometryType.PointGeometry);
                if (!isValid)
                {
                    errors.Add(new SemanticValidationError()
                    {
                        Message = "Coordinates you provided are not within UK coordinates",
                        Path = "Source.provision.regulatedPlace.geometry"
                    });
                    return new BoundingBox();
                }
                geometry = json.GetBetween("(", ")");
                token = JToken.FromObject(geometry);
                boundingBox = ValidateAgainstBoundingBox(token);
                break;
            case "LinearGeometry":
                values = children.DescendantsAndSelf().Skip(7).FirstOrDefault();
                json = values.ToIndentedJsonString();
                if (!json.Contains("SRID=27700;"))
                {
                    errors.Add(new SemanticValidationError
                    {
                        Message = "British National Grid - Spatial Reference not present in the geometry or it is wrong referenced.",
                        Path = "Source.provision.regulatedPlace.geometry"
                    });
                    return new BoundingBox();
                }
                toValidate = json.GetBetween(";", "\"");
                isValid = IsInUk(errors, toValidate, GeometryType.LinearGeometry);
                if (!isValid)
                {
                    errors.Add(new SemanticValidationError()
                    {
                        Message = "Coordinates you provided are not within UK coordinates",
                        Path = "Source.provision.regulatedPlace.geometry"
                    });
                    return new BoundingBox();
                }
                geometry = json.GetBetween("(", ")");
                token = JToken.FromObject(geometry);
                boundingBox = ValidateAgainstBoundingBox(token);
                break;
            case "Polygon":
                values = children.DescendantsAndSelf().LastOrDefault();
                json = values.ToIndentedJsonString();
                if (!json.Contains("SRID=27700;"))
                {
                    errors.Add(new SemanticValidationError
                    {
                        Message = "British National Grid - Spatial Reference not present in the geometry or it is wrong referenced.",
                        Path = "Source.provision.regulatedPlace.geometry"
                    });
                    return new BoundingBox();
                }

                toValidate = json.GetBetween(";", "\"");
                isValid = IsInUk(errors, toValidate, GeometryType.Polygon);
                if (!isValid)
                {
                    errors.Add(new SemanticValidationError()
                    {
                        Message = "Coordinates you provided are not within UK coordinates",
                        Path = "Source.provision.regulatedPlace.geometry"
                    });
                    return new BoundingBox();
                }
                geometry = json.GetBetween("(", ")");
                token = JToken.FromObject(geometry);
                boundingBox = ValidateAgainstBoundingBox(token);
                break;
            case "DirectedLinear":
                values = children.DescendantsAndSelf().LastOrDefault();
                json = values.ToIndentedJsonString();
                if (!json.Contains("SRID=27700;"))
                {
                    errors.Add(new SemanticValidationError
                    {
                        Message = "British National Grid - Spatial Reference not present in the geometry or it is wrong referenced.",
                        Path = "Source.provision.regulatedPlace.geometry"
                    });
                    return new BoundingBox();
                }
                toValidate = json.GetBetween(";", "\"");
                isValid = IsInUk(errors, toValidate, GeometryType.DirectedLinear);
                if (!isValid)
                {
                    errors.Add(new SemanticValidationError()
                    {
                        Message = "Coordinates you provided are not within UK coordinates",
                        Path = "Source.provision.regulatedPlace.geometry"
                    });
                    return new BoundingBox();
                }
                geometry = json.GetBetween("(", ")");
                token = JToken.FromObject(geometry);
                boundingBox = ValidateAgainstBoundingBox(token);
                break;
            default:
                errors.Add(new SemanticValidationError
                {
                    Path = "Source.provision.regulatedPlace.geometry",
                    Message = $"Selected geometry is not one of accepted types: {GeometryType.PointGeometry}, {GeometryType.LinearGeometry}, {GeometryType.Polygon} or {GeometryType.DirectedLinear}"
                });
                return new BoundingBox();
                break;
        }

        return boundingBox;
    }

    private static bool IsInUk(List<SemanticValidationError> errors, string toValidate, GeometryType geometryType)
    {
        const string ukBoundaryWkt = "POLYGON((500000 100000, 700000 100000, 700000 200000, 500000 200000, 500000 100000))";
        WKTReader wktReader = new();
        Polygon ukBoundary = wktReader.Read(ukBoundaryWkt) as Polygon;
        bool isWithinUk = false;
        switch (geometryType)
        {
            case GeometryType.Polygon:
                Polygon polygonToValidate = wktReader.Read(toValidate) as Polygon;
                isWithinUk = ukBoundary != null && ukBoundary.Contains(polygonToValidate);
                break;
            case GeometryType.PointGeometry:
                Point pointToValidate = wktReader.Read(toValidate) as Point;
                isWithinUk = ukBoundary != null && ukBoundary.Contains(pointToValidate);
                break;
            case GeometryType.DirectedLinear:
                LineString directedLinearToValidate = wktReader.Read(toValidate) as LineString;
                isWithinUk = ukBoundary != null && ukBoundary.Contains(directedLinearToValidate);
                break;
            case GeometryType.LinearGeometry:
                LineString lineStringToValidate = wktReader.Read(toValidate) as LineString;
                isWithinUk = ukBoundary != null && ukBoundary.Contains(lineStringToValidate);
                break;
            case GeometryType.Unknown:
                errors.Add(new SemanticValidationError
                {
                    Path = "Source.provision.regulatedPlace.geometry",
                    Message = $"Selected geometry is not one of accepted types: {GeometryType.PointGeometry}, {GeometryType.LinearGeometry}, {GeometryType.Polygon} or {GeometryType.DirectedLinear}"
                });
                break;
            default:
                errors.Add(new SemanticValidationError
                {
                    Path = "Source.provision.regulatedPlace.geometry",
                    Message = $"Selected geometry is not one of accepted types: {GeometryType.PointGeometry}, {GeometryType.LinearGeometry}, {GeometryType.Polygon} or {GeometryType.DirectedLinear}"
                });
                break;
        }
        return isWithinUk;
    }

    private static BoundingBox ValidateAgainstBoundingBox(IEnumerable<JToken> values)
    {
        List<Coordinates> coordinates = new();

        List<string> points = values
            .Value<string>()
            .Split(" ")
            .Select(it => it
                .Replace("(", "")
                .Replace(")", "")
                .Replace(",", ""))
            .ToList();

        for (int index = 0; index < points.Count; index++)
        {
            Coordinates coordinate = new();
            coordinate.Longitude = points[index].AsInt();
            index++;
            coordinate.Latitude = points[index].AsInt();

            coordinates.Add(coordinate);
        }

        return BoundingBox.Wrapping(coordinates);

    }
}