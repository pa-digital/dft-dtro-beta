using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Newtonsoft.Json.Linq;
using GeometryType = DfT.DTRO.Enums.GeometryType;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DfT.DTRO.Services.Mapping;

public class BoundingBoxService : IBoundingBoxService
{
    private readonly LoggingExtension _loggingExtension;

    private static SemanticValidationError Error => new();

    public BoundingBoxService(LoggingExtension loggingExtension) => _loggingExtension = loggingExtension;

    public BoundingBox SetBoundingBoxForSingleGeometry(List<SemanticValidationError> errors, JProperty jProperty, BoundingBox boundingBox)
    {
        JObject jObject = jProperty.Value as JObject;

        var geometrySelected = jObject?
            .Children<JProperty>()
            .FirstOrDefault(property => Constants.ConcreteGeometries.Any(property.Name.Equals));

        IEnumerable<JToken> values;
        string json;
        bool isValid;
        string toValidate;

        switch (geometrySelected?.Name)
        {
            case "PointGeometry":
                values = geometrySelected
                    .DescendantsAndSelf()
                    .Select(token => token.First?.Value<JToken>(Constants.Point))
                    .FirstOrDefault();

                json = values.ToIndentedJsonString();
                if (!json.Contains(Constants.Srid27700))
                {
                    errors.Add(Error.SetSetSpatialReferenceValidationError());

                    _loggingExtension.LogError(nameof(SetBoundingBoxForSingleGeometry), "", "Spatial reference error", string.Join(",", errors));

                    return new BoundingBox();
                }
                toValidate = json.GetBetween(";", "\"");
                isValid = ArePairsValid(errors, toValidate, GeometryType.PointGeometry);
                if (!isValid)
                {
                    return new BoundingBox();
                }
                isValid = IsInUk(errors, toValidate, GeometryType.PointGeometry);

                break;
            case "LinearGeometry":
                values = geometrySelected
                    .DescendantsAndSelf()
                    .Select(token => token.First?.Value<JToken>(Constants.LineString))
                    .FirstOrDefault();
                json = values.ToIndentedJsonString();
                if (!json.Contains(Constants.Srid27700))
                {
                    errors.Add(Error.SetSetSpatialReferenceValidationError());

                    _loggingExtension.LogError(nameof(SetBoundingBoxForSingleGeometry), "", "Spatial reference error", string.Join(",", errors));

                    return new BoundingBox();
                }
                toValidate = json.GetBetween(";", "\"");
                isValid = ArePairsValid(errors, toValidate, GeometryType.LinearGeometry);
                if (!isValid)
                {
                    return new BoundingBox();
                }
                isValid = IsInUk(errors, toValidate, GeometryType.LinearGeometry);

                break;
            case "Polygon":
                values = geometrySelected
                    .DescendantsAndSelf()
                    .Select(token => token.First?.Value<JToken>(Constants.Polygon))
                    .FirstOrDefault();
                json = values.ToIndentedJsonString();
                if (!json.Contains(Constants.Srid27700))
                {
                    errors.Add(Error.SetSetSpatialReferenceValidationError());

                    _loggingExtension.LogError(nameof(SetBoundingBoxForSingleGeometry), "", "Spatial reference error", string.Join(",", errors));

                    return new BoundingBox();
                }

                toValidate = json.GetBetween(";", "\"");

                isValid = ArePairsValid(errors, toValidate, GeometryType.Polygon);
                if (!isValid)
                {
                    return new BoundingBox();
                }
                isValid = IsInUk(errors, toValidate, GeometryType.Polygon);


                break;
            case "DirectedLinear":
                values = geometrySelected
                    .DescendantsAndSelf()
                    .Select(token => token.First?.Value<JToken>(Constants.DirectedLineString))
                    .FirstOrDefault();
                json = values.ToIndentedJsonString();
                if (!json.Contains(Constants.Srid27700))
                {
                    errors.Add(Error.SetSetSpatialReferenceValidationError());

                    _loggingExtension.LogError(nameof(SetBoundingBoxForSingleGeometry), "", "Spatial reference error", string.Join(",", errors));

                    return new BoundingBox();
                }
                toValidate = json.GetBetween(";", "\"");

                isValid = ArePairsValid(errors, toValidate, GeometryType.DirectedLinear);
                if (!isValid)
                {
                    return new BoundingBox();
                }

                isValid = IsInUk(errors, toValidate, GeometryType.DirectedLinear);

                break;
            default:
                errors.Add(new SemanticValidationError
                {
                    Path = "Source.provision.regulatedPlace.geometry",
                    Message = "Selected geometry ",
                    Name = "Wrong geometry",
                    Rule = $"Geometry accepted should be one of: {GeometryType.PointGeometry}, {GeometryType.LinearGeometry}, {GeometryType.Polygon} or {GeometryType.DirectedLinear}"
                });

                _loggingExtension.LogError(nameof(SetBoundingBoxForSingleGeometry), "", "Geometry type error", string.Join(",", errors));

                return new BoundingBox();
        }

        if (!isValid)
        {
            errors.Add(Error.SetCoordinatesValidationError());

            _loggingExtension.LogError(nameof(SetBoundingBoxForSingleGeometry), "", "Geometry boundary error", string.Join(",", errors));

            return new BoundingBox();
        }

        string geometry = json.GetBetween("(", ")");
        JToken token = JToken.FromObject(geometry);
        return boundingBox.ValidateAgainstBoundingBox(token);
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
                values = jProperty
                    .DescendantsAndSelf()
                    .Select(token => token.First?.Value<JToken>(Constants.Point))
                    .FirstOrDefault();
                json = values.ToIndentedJsonString();
                if (!json.Contains(Constants.Srid27700))
                {
                    errors.Add(Error.SetSetSpatialReferenceValidationError());

                    _loggingExtension.LogError(nameof(SetBoundingBoxForMultipleGeometries), "", "Spatial reference error", string.Join(",", errors));
                    return new BoundingBox();
                }
                toValidate = json.GetBetween(";", "\"");
                isValid = ArePairsValid(errors, toValidate, GeometryType.PointGeometry);
                if (!isValid)
                {
                    _loggingExtension.LogError(nameof(SetBoundingBoxForMultipleGeometries), "", "Geometry pairs are not valid", string.Join(",", errors));
                    return new BoundingBox();
                }
                isValid = IsInUk(errors, toValidate, GeometryType.PointGeometry);

                break;
            case "LinearGeometry":
                values = jProperty
                    .DescendantsAndSelf()
                    .Select(token => token.First?.Value<JToken>(Constants.LineString))
                    .FirstOrDefault();
                json = values.ToIndentedJsonString();
                if (!json.Contains(Constants.Srid27700))
                {
                    errors.Add(Error.SetSetSpatialReferenceValidationError());
                    _loggingExtension.LogError(nameof(SetBoundingBoxForMultipleGeometries), "", "Spatial reference error", string.Join(",", errors));
                    return new BoundingBox();
                }
                toValidate = json.GetBetween(";", "\"");
                isValid = ArePairsValid(errors, toValidate, GeometryType.LinearGeometry);
                if (!isValid)
                {
                    _loggingExtension.LogError(nameof(SetBoundingBoxForMultipleGeometries), "", "Geometry pairs are not valid", string.Join(",", errors));
                    return new BoundingBox();
                }
                isValid = IsInUk(errors, toValidate, GeometryType.LinearGeometry);

                break;
            case "Polygon":
                values = jProperty
                    .DescendantsAndSelf()
                    .Select(token => token.First?.Value<JToken>(Constants.Polygon))
                    .FirstOrDefault();
                json = values.ToIndentedJsonString();
                if (!json.Contains(Constants.Srid27700))
                {
                    errors.Add(Error.SetSetSpatialReferenceValidationError());
                    _loggingExtension.LogError(nameof(SetBoundingBoxForMultipleGeometries), "", "Spatial reference error", string.Join(",", errors));
                    return new BoundingBox();
                }

                toValidate = json.GetBetween(";", "\"");

                isValid = ArePairsValid(errors, toValidate, GeometryType.Polygon);
                if (!isValid)
                {
                    _loggingExtension.LogError(nameof(SetBoundingBoxForMultipleGeometries), "", "Geometry pairs are not valid", string.Join(",", errors));
                    return new BoundingBox();
                }
                isValid = IsInUk(errors, toValidate, GeometryType.Polygon);


                break;
            case "DirectedLinear":
                values = jProperty
                    .DescendantsAndSelf()
                    .Select(token => token.First?.Value<JToken>(Constants.DirectedLineString))
                    .FirstOrDefault();
                json = values.ToIndentedJsonString();
                if (!json.Contains(Constants.Srid27700))
                {
                    errors.Add(Error.SetSetSpatialReferenceValidationError());
                    _loggingExtension.LogError(nameof(SetBoundingBoxForMultipleGeometries), "", "Spatial reference error", string.Join(",", errors));
                    return new BoundingBox();
                }
                toValidate = json.GetBetween(";", "\"");

                isValid = ArePairsValid(errors, toValidate, GeometryType.DirectedLinear);
                if (!isValid)
                {
                    _loggingExtension.LogError(nameof(SetBoundingBoxForMultipleGeometries), "", "Geometry pairs are not valid", string.Join(",", errors));
                    return new BoundingBox();
                }

                isValid = IsInUk(errors, toValidate, GeometryType.DirectedLinear);

                break;
            default:
                errors.Add(new SemanticValidationError
                {
                    Path = "Source.provision.regulatedPlace.geometry",
                    Message = "Selected geometry ",
                    Name = "Wrong geometry",
                    Rule = $"Geometry accepted should be one of: {GeometryType.PointGeometry}, {GeometryType.LinearGeometry}, {GeometryType.Polygon} or {GeometryType.DirectedLinear}"
                });
                _loggingExtension.LogError(nameof(SetBoundingBoxForMultipleGeometries), "", "Geometry type error", string.Join(",", errors));
                return new BoundingBox();
        }

        if (!isValid)
        {
            errors.Add(Error.SetCoordinatesValidationError());
            _loggingExtension.LogError(nameof(SetBoundingBoxForMultipleGeometries), "", "Geometry boundary error", string.Join(",", errors));
            return new BoundingBox();
        }

        string geometry = json.GetBetween("(", ")");
        JToken token = JToken.FromObject(geometry);
        return boundingBox.ValidateAgainstBoundingBox(token);
    }

    private bool IsInUk(List<SemanticValidationError> errors, string toValidate, GeometryType geometryType)
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
            default:
                errors.Add(Error.SetGeometryValidationError());
                _loggingExtension.LogError(nameof(SetBoundingBoxForMultipleGeometries), "", "Geometry type error", string.Join(",", errors));
                break;
        }
        return isWithinUk;
    }


    private bool ArePairsValid(List<SemanticValidationError> errors, string toValidate,
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
                    errors.Add(Error.SetIncorrectPairSemanticValidationError(GeometryType.PointGeometry));

                    _loggingExtension.LogError(nameof(ArePairsValid), "", "Geometry pair error", string.Join(",", errors));
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
                    errors.Add(Error.SetIncorrectPairSemanticValidationError(GeometryType.LinearGeometry));

                    _loggingExtension.LogError(nameof(ArePairsValid), "", "Geometry pair error", string.Join(",", errors));
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
                    errors.Add(Error.SetIncorrectPairSemanticValidationError(GeometryType.Polygon));

                    _loggingExtension.LogError(nameof(ArePairsValid), "", "Geometry pair error", string.Join(",", errors));
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
                    errors.Add(Error.SetIncorrectPairSemanticValidationError(GeometryType.DirectedLinear));

                    _loggingExtension.LogError(nameof(ArePairsValid), "", "Geometry pair error", string.Join(",", errors));
                }
                break;
            default:
                errors.Add(new SemanticValidationError
                {
                    Path = "Source.provision.regulatedPlace.geometry",
                    Message = "Selected geometry",
                    Name = "Wrong geometry",
                    Rule = $"Geometry accepted should be one of: {GeometryType.PointGeometry}, {GeometryType.LinearGeometry}, {GeometryType.Polygon} or {GeometryType.DirectedLinear}"
                });

                _loggingExtension.LogError(nameof(ArePairsValid), "", "Geometry pair error", string.Join(",", errors));

                break;
        }

        return arePairsValid;
    }

    //private static BoundingBox ValidateAgainstBoundingBox(IEnumerable<JToken> values)
    //{
    //    List<string> points = values
    //        .Value<string>()
    //        .Split(" ")
    //        .Select(it => it
    //            .Replace("(", "")
    //            .Replace(")", "")
    //            .Replace(",", ""))
    //        .ToList();

    //    List<Coordinates> coordinates = points
    //        .Select(point => new Coordinates
    //        {
    //            Longitude = point.AsInt(),
    //            Latitude = point.AsInt()
    //        })
    //        .ToList();

    //    return BoundingBox.Wrapping(coordinates);
    //}
}