using DfT.DTRO.Models.Validation;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Newtonsoft.Json.Linq;
using Coordinates = DfT.DTRO.Models.DtroJson.Coordinates;
using GeometryType = DfT.DTRO.Enums.GeometryType;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DfT.DTRO.Services.Mapping;

public class BoundingBoxService : IBoundingBoxService
{
    public BoundingBox SetBoundingBoxForSingleGeometry(List<SemanticValidationError> errors, JObject jObject, BoundingBox boundingBox)
    {
        JProperty children = jObject.Children<JProperty>().ElementAt(1);
        IEnumerable<JToken> values;
        string json;
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
                        Message = "British National Grid - Spatial Reference is not present in the geometry or is referenced incorrectly.",
                        Path = "Source.provision.regulatedPlace.geometry",
                        Name = "Spatial reference",
                        Rule = "Spatial reference should be SRID=27700"
                    });

                    return new BoundingBox();
                }
                toValidate = json.GetBetween(";", "\"");
                isValid = ArePairsValid(errors, toValidate, GeometryType.PointGeometry);
                if (!isValid)
                {
                    return new BoundingBox();
                }
                isValid = IsInUk(errors, toValidate, GeometryType.PointGeometry);
                if (!isValid)
                {
                    errors.Add(new SemanticValidationError
                    {
                        Message = "The provided coordinates are outside the recognized UK boundaries.",
                        Path = "Source.provision.regulatedPlace.geometry",
                        Name = "Wrong coordinates",
                        Rule = "Coordinates should be within UK coordinates - POLYGON((0 0, 700000 0, 700000 1300000, 0 1300000, 0 0))"
                    });
                    return new BoundingBox();
                }

                break;
            case "LinearGeometry":
                values = children.DescendantsAndSelf().Skip(7).FirstOrDefault();
                json = values.ToIndentedJsonString();
                if (!json.Contains("SRID=27700;"))
                {
                    errors.Add(new SemanticValidationError
                    {
                        Message = "British National Grid - Spatial Reference is not present within the geometry or is referenced incorrectly.",
                        Path = "Source.provision.regulatedPlace.geometry",
                        Name = "Spatial reference",
                        Rule = "Spatial reference should be SRID=27700"
                    });
                    return new BoundingBox();
                }
                toValidate = json.GetBetween(";", "\"");
                isValid = ArePairsValid(errors, toValidate, GeometryType.LinearGeometry);
                if (!isValid)
                {
                    return new BoundingBox();
                }
                isValid = IsInUk(errors, toValidate, GeometryType.LinearGeometry);
                if (!isValid)
                {
                    errors.Add(new SemanticValidationError
                    {
                        Message = "The provided coordinates are outside the recognized UK boundaries.",
                        Path = "Source.provision.regulatedPlace.geometry",
                        Name = "Wrong coordinates",
                        Rule = "Coordinates should be within UK coordinates - POLYGON((0 0, 700000 0, 700000 1300000, 0 1300000, 0 0))"
                    });
                    return new BoundingBox();
                }

                break;
            case "Polygon":
                values = children.DescendantsAndSelf().LastOrDefault();
                json = values.ToIndentedJsonString();
                if (!json.Contains("SRID=27700;"))
                {
                    errors.Add(new SemanticValidationError
                    {
                        Message = "British National Grid - Spatial Reference is not present within the geometry or is referenced incorrectly.",
                        Path = "Source.provision.regulatedPlace.geometry",
                        Name = "Spatial reference",
                        Rule = "Spatial reference should be SRID=27700"
                    });
                    return new BoundingBox();
                }

                toValidate = json.GetBetween(";", "\"");

                isValid = ArePairsValid(errors, toValidate, GeometryType.Polygon);
                if (!isValid)
                {
                    return new BoundingBox();
                }
                isValid = IsInUk(errors, toValidate, GeometryType.Polygon);
                if (!isValid)
                {
                    errors.Add(new SemanticValidationError
                    {
                        Message = "The provided coordinates are outside the recognized UK boundaries.",
                        Path = "Source.provision.regulatedPlace.geometry",
                        Name = "Wrong coordinates",
                        Rule = "Coordinates should be within UK coordinates - POLYGON((0 0, 700000 0, 700000 1300000, 0 1300000, 0 0))"
                    });
                    return new BoundingBox();
                }


                break;
            case "DirectedLinear":
                values = children.DescendantsAndSelf().LastOrDefault();
                json = values.ToIndentedJsonString();
                if (!json.Contains("SRID=27700;"))
                {
                    errors.Add(new SemanticValidationError
                    {
                        Message = "British National Grid - Spatial Reference is not present within the geometry or is referenced incorrectly.",
                        Path = "Source.provision.regulatedPlace.geometry",
                        Name = "Spatial reference",
                        Rule = "Spatial reference should be SRID=27700"
                    });
                    return new BoundingBox();
                }
                toValidate = json.GetBetween(";", "\"");

                isValid = ArePairsValid(errors, toValidate, GeometryType.DirectedLinear);
                if (!isValid)
                {
                    return new BoundingBox();
                }

                isValid = IsInUk(errors, toValidate, GeometryType.DirectedLinear);
                if (!isValid)
                {
                    errors.Add(new SemanticValidationError
                    {
                        Message = "The provided coordinates are outside the recognized UK boundaries.",
                        Path = "Source.provision.regulatedPlace.geometry",
                        Name = "Wrong coordinates",
                        Rule = "Coordinates should be within UK coordinates - POLYGON((0 0, 700000 0, 700000 1300000, 0 1300000, 0 0))"
                    });
                    return new BoundingBox();
                }

                break;
            default:
                errors.Add(new SemanticValidationError
                {
                    Path = "Source.provision.regulatedPlace.geometry",
                    Message = "Selected geometry ",
                    Name = "Wrong geometry",
                    Rule = $"Geometry accepted should be one of: {GeometryType.PointGeometry}, {GeometryType.LinearGeometry}, {GeometryType.Polygon} or {GeometryType.DirectedLinear}"
                });
                return new BoundingBox();
        }

        string geometry = json.GetBetween("(", ")");
        JToken token = JToken.FromObject(geometry);
        boundingBox = ValidateAgainstBoundingBox(token);

        return boundingBox;
    }

    public BoundingBox SetBoundingBoxForMultipleGeometries(List<SemanticValidationError> errors, JProperty jProperty,
        BoundingBox boundingBox)
    {
        IEnumerable<JToken> values;
        string json;
        bool isValid;
        string toValidate;
        switch (jProperty.Name)
        {
            case "PointGeometry":
                values = jProperty.DescendantsAndSelf().Skip(3).FirstOrDefault();
                json = values.ToIndentedJsonString();
                if (!json.Contains("SRID=27700;"))
                {
                    errors.Add(new SemanticValidationError
                    {
                        Message = "British National Grid - Spatial Reference is not present in the geometry or is referenced incorrectly.",
                        Path = "Source.provision.regulatedPlace.geometry",
                        Name = "Spatial reference",
                        Rule = "Spatial reference should be SRID=27700"
                    });

                    return new BoundingBox();
                }
                toValidate = json.GetBetween(";", "\"");
                isValid = ArePairsValid(errors, toValidate, GeometryType.PointGeometry);
                if (!isValid)
                {
                    return new BoundingBox();
                }
                isValid = IsInUk(errors, toValidate, GeometryType.PointGeometry);
                if (!isValid)
                {
                    errors.Add(new SemanticValidationError
                    {
                        Message = "The provided coordinates are outside the recognized UK boundaries.",
                        Path = "Source.provision.regulatedPlace.geometry",
                        Name = "Wrong coordinates",
                        Rule = "Coordinates should be within UK coordinates - POLYGON((0 0, 700000 0, 700000 1300000, 0 1300000, 0 0))"
                    });
                    return new BoundingBox();
                }

                break;
            case "LinearGeometry":
                values = jProperty.DescendantsAndSelf().Skip(7).FirstOrDefault();
                json = values.ToIndentedJsonString();
                if (!json.Contains("SRID=27700;"))
                {
                    errors.Add(new SemanticValidationError
                    {
                        Message = "British National Grid - Spatial Reference is not present within the geometry or is referenced incorrectly.",
                        Path = "Source.provision.regulatedPlace.geometry",
                        Name = "Spatial reference",
                        Rule = "Spatial reference should be SRID=27700"
                    });
                    return new BoundingBox();
                }
                toValidate = json.GetBetween(";", "\"");
                isValid = ArePairsValid(errors, toValidate, GeometryType.LinearGeometry);
                if (!isValid)
                {
                    return new BoundingBox();
                }
                isValid = IsInUk(errors, toValidate, GeometryType.LinearGeometry);
                if (!isValid)
                {
                    errors.Add(new SemanticValidationError
                    {
                        Message = "The provided coordinates are outside the recognized UK boundaries.",
                        Path = "Source.provision.regulatedPlace.geometry",
                        Name = "Wrong coordinates",
                        Rule = "Coordinates should be within UK coordinates - POLYGON((0 0, 700000 0, 700000 1300000, 0 1300000, 0 0))"
                    });
                    return new BoundingBox();
                }

                break;
            case "Polygon":
                values = jProperty.DescendantsAndSelf().LastOrDefault();
                json = values.ToIndentedJsonString();
                if (!json.Contains("SRID=27700;"))
                {
                    errors.Add(new SemanticValidationError
                    {
                        Message = "British National Grid - Spatial Reference is not present within the geometry or is referenced incorrectly.",
                        Path = "Source.provision.regulatedPlace.geometry",
                        Name = "Spatial reference",
                        Rule = "Spatial reference should be SRID=27700"
                    });
                    return new BoundingBox();
                }

                toValidate = json.GetBetween(";", "\"");

                isValid = ArePairsValid(errors, toValidate, GeometryType.Polygon);
                if (!isValid)
                {
                    return new BoundingBox();
                }
                isValid = IsInUk(errors, toValidate, GeometryType.Polygon);
                if (!isValid)
                {
                    errors.Add(new SemanticValidationError
                    {
                        Message = "The provided coordinates are outside the recognized UK boundaries.",
                        Path = "Source.provision.regulatedPlace.geometry",
                        Name = "Wrong coordinates",
                        Rule = "Coordinates should be within UK coordinates - POLYGON((0 0, 700000 0, 700000 1300000, 0 1300000, 0 0))"
                    });
                    return new BoundingBox();
                }


                break;
            case "DirectedLinear":
                values = jProperty.DescendantsAndSelf().ElementAt(4);
                json = values.ToIndentedJsonString();
                if (!json.Contains("SRID=27700;"))
                {
                    errors.Add(new SemanticValidationError
                    {
                        Message = "British National Grid - Spatial Reference is not present within the geometry or is referenced incorrectly.",
                        Path = "Source.provision.regulatedPlace.geometry",
                        Name = "Spatial reference",
                        Rule = "Spatial reference should be SRID=27700"
                    });
                    return new BoundingBox();
                }
                toValidate = json.GetBetween(";", "\"");

                isValid = ArePairsValid(errors, toValidate, GeometryType.DirectedLinear);
                if (!isValid)
                {
                    return new BoundingBox();
                }

                isValid = IsInUk(errors, toValidate, GeometryType.DirectedLinear);
                if (!isValid)
                {
                    errors.Add(new SemanticValidationError
                    {
                        Message = "The provided coordinates are outside the recognized UK boundaries.",
                        Path = "Source.provision.regulatedPlace.geometry",
                        Name = "Wrong coordinates",
                        Rule = "Coordinates should be within UK coordinates - POLYGON((0 0, 700000 0, 700000 1300000, 0 1300000, 0 0))"
                    });
                    return new BoundingBox();
                }

                break;
            default:
                errors.Add(new SemanticValidationError
                {
                    Path = "Source.provision.regulatedPlace.geometry",
                    Message = "Selected geometry ",
                    Name = "Wrong geometry",
                    Rule = $"Geometry accepted should be one of: {GeometryType.PointGeometry}, {GeometryType.LinearGeometry}, {GeometryType.Polygon} or {GeometryType.DirectedLinear}"
                });
                return new BoundingBox();
        }

        string geometry = json.GetBetween("(", ")");
        JToken token = JToken.FromObject(geometry);
        boundingBox = ValidateAgainstBoundingBox(token);

        return boundingBox;
    }

    private static bool IsInUk(List<SemanticValidationError> errors, string toValidate, GeometryType geometryType)
    {
        const string ukBoundaryWkt = "POLYGON((0 0, 700000 0, 700000 1300000, 0 1300000, 0 0))";
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
                    Message = "Selected geometry ",
                    Name = "Wrong geometry",
                    Rule = $"Geometry accepted should be one of: {GeometryType.PointGeometry}, {GeometryType.LinearGeometry}, {GeometryType.Polygon} or {GeometryType.DirectedLinear}"
                });
                break;
            default:
                errors.Add(new SemanticValidationError
                {
                    Path = "Source.provision.regulatedPlace.geometry",
                    Message = "Selected geometry ",
                    Name = "Wrong geometry",
                    Rule = $"Geometry accepted should be one of: {GeometryType.PointGeometry}, {GeometryType.LinearGeometry}, {GeometryType.Polygon} or {GeometryType.DirectedLinear}"
                });
                break;
        }
        return isWithinUk;
    }


    private static bool ArePairsValid(List<SemanticValidationError> errors, string toValidate,
        GeometryType geometryType)
    {
        WKTReader wktReader = new();
        bool arePairsValid = false;
        switch (geometryType)
        {
            case GeometryType.PointGeometry:
                try
                {
                    wktReader.Read(toValidate);
                    arePairsValid = true;
                }
                catch
                {
                    errors.Add(new SemanticValidationError
                    {
                        Path = $"Source.provision.regulatedPlace.geometry.{GeometryType.PointGeometry}",
                        Message = "Selected geometry",
                        Name = "Incorrect pairs.",
                        Rule = $"Incorrect pairs for selected geometry: {GeometryType.PointGeometry}"
                    });
                }
                break;
            case GeometryType.LinearGeometry:
                try
                {
                    wktReader.Read(toValidate);
                    arePairsValid = true;
                }
                catch
                {
                    errors.Add(new SemanticValidationError
                    {
                        Path = $"Source.provision.regulatedPlace.geometry.{GeometryType.LinearGeometry}",
                        Message = "Selected geometry",
                        Name = "Incorrect pairs",
                        Rule = $"Incorrect pairs for selected geometry: {GeometryType.LinearGeometry} "
                    });
                }
                break;
            case GeometryType.Polygon:
                try
                {
                    wktReader.Read(toValidate);
                    arePairsValid = true;
                }
                catch
                {
                    errors.Add(new SemanticValidationError
                    {
                        Path = $"Source.provision.regulatedPlace.geometry.{GeometryType.Polygon}",
                        Message = "Selected geometry",
                        Name = "Incorrect pairs",
                        Rule = $"Incorrect pairs for selected geometry: {GeometryType.Polygon} "
                    });
                }
                break;
            case GeometryType.DirectedLinear:
                try
                {
                    wktReader.Read(toValidate);
                    arePairsValid = true;
                }
                catch
                {
                    errors.Add(new SemanticValidationError
                    {
                        Path = $"Source.provision.regulatedPlace.geometry.{GeometryType.DirectedLinear}",
                        Message = "Selected geometry",
                        Name = "Incorrect pairs",
                        Rule = $"Incorrect pairs for selected geometry: {GeometryType.DirectedLinear} "
                    });
                }
                break;
            case GeometryType.Unknown:
                errors.Add(new SemanticValidationError
                {
                    Path = "Source.provision.regulatedPlace.geometry",
                    Message = "Selected geometry",
                    Name = "Unknown geometry",
                    Rule = $"Geometry accepted should be one of: {GeometryType.PointGeometry}, {GeometryType.LinearGeometry}, {GeometryType.Polygon} or {GeometryType.DirectedLinear}"
                });
                break;
            default:
                errors.Add(new SemanticValidationError
                {
                    Path = "Source.provision.regulatedPlace.geometry",
                    Message = "Selected geometry",
                    Name = "Wrong geometry",
                    Rule = $"Geometry accepted should be one of: {GeometryType.PointGeometry}, {GeometryType.LinearGeometry}, {GeometryType.Polygon} or {GeometryType.DirectedLinear}"
                });
                break;
        }

        return arePairsValid;
    }

    private static BoundingBox ValidateAgainstBoundingBox(IEnumerable<JToken> values)
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
            .Select(paint => new Coordinates
            {
                Longitude = paint.AsInt(),
                Latitude = paint.AsInt()
            })
            .ToList();

        return BoundingBox.Wrapping(coordinates);
    }
}