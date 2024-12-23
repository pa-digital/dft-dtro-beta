using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

namespace DfT.DTRO.Services.Validation.Implementation;

/// <inheritdoc cref="IGeometryValidationService"/>
public class GeometryValidationService : IGeometryValidationService
{
    /// <inheritdoc cref="IGeometryValidationService"/>
    public List<SemanticValidationError> Validate(DtroSubmit dtroSubmit)
    {
        var errors = new List<SemanticValidationError>();


        var passedInGeometries = dtroSubmit
            .Data
            .GetValueOrDefault<IList<object>>("Source.Provision".ToBackwardCompatibility(dtroSubmit.SchemaVersion))
            .OfType<ExpandoObject>()
            .SelectMany(provisions => provisions
                .GetValueOrDefault<IList<object>>("RegulatedPlace".ToBackwardCompatibility(dtroSubmit.SchemaVersion))
                .OfType<ExpandoObject>())
            .Where(expandoObject => Constants.ConcreteGeometries.Any(expandoObject.HasField))
            .ToList();

        foreach (var passedInGeometry in passedInGeometries)
            foreach (var concreteGeometry in Constants.ConcreteGeometries)
            {
                if (passedInGeometry.HasField(concreteGeometry))
                {
                    var selectedGeometry = passedInGeometry.GetExpando(concreteGeometry);
                    var version = selectedGeometry.GetValueOrDefault<long>("version");
                    if (version == 0)
                    {
                        var error = new SemanticValidationError
                        {
                            Name = "Invalid geometry version",
                            Message = "Version of geometry linked to a concrete forms of geometry",
                            Path = "Source -> Provision -> RegulatedPlace -> Geometry -> version",
                            Rule = $"Version number must be an integer and cannot be '{version}'"
                        };

                        errors.Add(error);
                    }

                    switch (concreteGeometry)
                    {
                        case "PointGeometry":
                            var point = selectedGeometry.GetValueOrDefault<string>("point");
                            if (string.IsNullOrEmpty(point))
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid geometry coordinates",
                                    Message = "Geometry coordinates linked to 'PointGeometry'",
                                    Path = "Source -> Provision -> RegulatedPlace -> PointGeometry -> point",
                                    Rule = "The coordinates cannot be null"
                                };

                                errors.Add(error);
                            }

                            var isBritishGrid = IsBritishGrid(point);
                            if (!isBritishGrid)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid geometry grid",
                                    Message = "Geometry grid linked to 'PointGeometry'",
                                    Path = "Source -> Provision -> RegulatedPlace -> PointGeometry -> point",
                                    Rule = "The grid must be 'SRID=27700"
                                };

                                errors.Add(error);
                            }
                            var areValidCoordinates = AreValidCoordinates(point);
                            if (!areValidCoordinates)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid coordinates pair",
                                    Message = "Geometry grid linked to 'PointGeometry'",
                                    Path = "Source -> Provision -> RegulatedPlace -> PointGeometry -> point",
                                    Rule = $"Check the coordinates pairs '{point}'"
                                };

                                errors.Add(error);
                            }

                            var isWithinBoundaries = IsWithinUkBoundaries(point, concreteGeometry);
                            if (!isWithinBoundaries)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid coordinates",
                                    Message = "Geometry grid linked to 'PointGeometry'",
                                    Path = "Source -> Provision -> RegulatedPlace -> PointGeometry -> point",
                                    Rule = $"Coordinates '{point}' must be within Great Britain"
                                };

                                errors.Add(error);
                            }

                            var representation = selectedGeometry.GetValueOrDefault<string>("representation");
                            if (string.IsNullOrEmpty(representation))
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Missing representation",
                                    Message = "Indicates the nature of the point location for a point representation of a regulated place.",
                                    Path = "Source -> Provision -> RegulatedPlace -> PointGeometry -> representation",
                                    Rule = "'representation' is missing"
                                };

                                errors.Add(error);
                            }

                            var representationPointType = typeof(PointType)
                                .GetDisplayNames<PointType>()
                                .ToList();
                            var isValidRepresentation = representationPointType
                                .Any(it => representation != null && representation.Contains(it));

                            if (!isValidRepresentation)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid representation",
                                    Message = "Indicates the nature of the point location for a point representation of a regulated place.",
                                    Path = "Source -> Provision -> RegulatedPlace -> PointGeometry -> representation",
                                    Rule = $"'{representation}' must be one of '{string.Join(",", representationPointType)}'"
                                };

                                errors.Add(error);
                            }

                            break;
                        case "LinearGeometry":
                            var direction = selectedGeometry.GetValueOrDefault<string>("direction");
                            if (string.IsNullOrEmpty(direction))
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Missing direction",
                                    Message = "Indicates the direction of the applicability of the referenced regulation.",
                                    Path = "Source -> Provision -> RegulatedPlace -> LinearGeometry -> direction",
                                    Rule = "'direction' is missing"
                                };

                                errors.Add(error);
                            }

                            var linearDirectionType = typeof(LinearDirectionType)
                                .GetDisplayNames<LinearDirectionType>()
                                .ToList();
                            var isValidLinearDirectionType =
                                linearDirectionType.Any(it => direction != null && direction.Contains(it));
                            if (!isValidLinearDirectionType)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid direction",
                                    Message = "Indicates the direction of the applicability of the referenced regulation.",
                                    Path = "Source -> Provision -> RegulatedPlace -> LinearGeometry -> direction",
                                    Rule = $"'{direction}' must be on of '{string.Join(",", linearDirectionType)}'"
                                };

                                errors.Add(error);
                            }

                            var lateralPosition = selectedGeometry.GetValueOrDefault<string>("lateralPosition");
                            if (string.IsNullOrEmpty(lateralPosition))
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Missing lateral position",
                                    Message = "Indicates the lateral position across a road of the linear representation of a regulated place.",
                                    Path = "Source -> Provision -> RegulatedPlace -> LinearGeometry -> lateralPosition",
                                    Rule = "'lateralPosition' is missing"
                                };

                                errors.Add(error);
                            }

                            var linearLateralPositionType = typeof(LinearLateralPositionType)
                                .GetDisplayNames<LinearLateralPositionType>()
                                .ToList();

                            var isValidLateralPosition = linearLateralPositionType
                                .Any(it => lateralPosition != null && lateralPosition.Contains(it));
                            if (!isValidLateralPosition)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid lateral position",
                                    Message = "Indicates the lateral position across a road of the linear representation of a regulated place.",
                                    Path = "Source -> Provision -> RegulatedPlace -> LinearGeometry -> lateralPosition",
                                    Rule = $"'{lateralPosition}' must be one of '{string.Join(",", linearLateralPositionType)}'"
                                };

                                errors.Add(error);
                            }

                            var lineString = selectedGeometry.GetValueOrDefault<string>("linestring");
                            if (string.IsNullOrEmpty(lineString))
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid geometry coordinates",
                                    Message = "Geometry coordinates linked to 'LinearGeometry'",
                                    Path = "Source -> Provision -> RegulatedPlace -> LinearGeometry -> linestring",
                                    Rule = "The coordinates cannot be null"
                                };

                                errors.Add(error);
                            }

                            isBritishGrid = IsBritishGrid(lineString);
                            if (!isBritishGrid)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid geometry grid",
                                    Message = "Geometry grid linked to 'LinearGeometry'",
                                    Path = "Source -> Provision -> RegulatedPlace -> LinearGeometry -> linestring",
                                    Rule = "The grid must be 'SRID=27700"
                                };

                                errors.Add(error);
                            }
                            areValidCoordinates = AreValidCoordinates(lineString);
                            if (!areValidCoordinates)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid coordinates pair",
                                    Message = "Geometry grid linked to 'LinearGeometry'",
                                    Path = "Source -> Provision -> RegulatedPlace -> LinearGeometry -> linestring",
                                    Rule = $"Check the coordinates pairs '{lineString}'"
                                };

                                errors.Add(error);
                            }

                            isWithinBoundaries = IsWithinUkBoundaries(lineString, concreteGeometry);
                            if (!isWithinBoundaries)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid coordinates",
                                    Message = "Geometry grid linked to 'LinearGeometry'",
                                    Path = "Source -> Provision -> RegulatedPlace -> LinearGeometry -> linestring",
                                    Rule = $"Coordinates '{lineString}' must be within Great Britain"
                                };

                                errors.Add(error);
                            }

                            representation = selectedGeometry.GetValueOrDefault<string>("representation");
                            if (string.IsNullOrEmpty(representation))
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Missing representation",
                                    Message = "Indicates the nature of the point location for a linear representation of a regulated place.",
                                    Path = "Source -> Provision -> RegulatedPlace -> LinearGeometry -> representation",
                                    Rule = "'representation' is missing"
                                };

                                errors.Add(error);
                            }

                            var linearTypes = typeof(LinearType).GetDisplayNames<LinearType>().ToList();
                            var isValidLinearType = linearTypes
                                .Any(it => representation != null && representation.Contains(it));
                            if (!isValidLinearType)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid representation",
                                    Message = "Indicates the nature of the point location for a linear representation of a regulated place.",
                                    Path = "Source -> Provision -> RegulatedPlace -> LinearGeometry -> representation",
                                    Rule = $"'{representation}' must be one of '{string.Join(",", linearTypes)}'"
                                };

                                errors.Add(error);
                            }
                            break;
                        case "Polygon":
                            var polygon = selectedGeometry.GetValueOrDefault<string>("polygon");
                            if (string.IsNullOrEmpty(polygon))
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid geometry coordinates",
                                    Message = "Geometry coordinates linked to 'Polygon'",
                                    Path = "Source -> Provision -> RegulatedPlace -> Polygon -> polygon",
                                    Rule = "The coordinates cannot be null"
                                };

                                errors.Add(error);
                            }

                            isBritishGrid = IsBritishGrid(polygon);
                            if (!isBritishGrid)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid geometry grid",
                                    Message = "Geometry grid linked to 'Polygon'",
                                    Path = "Source -> Provision -> RegulatedPlace -> Polygon -> polygon",
                                    Rule = "The grid must be 'SRID=27700"
                                };

                                errors.Add(error);
                            }
                            areValidCoordinates = AreValidCoordinates(polygon);
                            if (!areValidCoordinates)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid coordinates pair",
                                    Message = "Geometry grid linked to 'Polygon'",
                                    Path = "Source -> Provision -> RegulatedPlace -> Polygon -> polygon",
                                    Rule = $"Check the coordinates pairs '{polygon}'"
                                };

                                errors.Add(error);
                            }

                            isWithinBoundaries = IsWithinUkBoundaries(polygon, concreteGeometry);
                            if (!isWithinBoundaries)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid coordinates",
                                    Message = "Geometry grid linked to 'Polygon'",
                                    Path = "Source -> Provision -> RegulatedPlace -> Polygon -> polygon",
                                    Rule = $"Coordinates '{polygon}' must be within Great Britain"
                                };

                                errors.Add(error);
                            }
                            break;
                        case "DirectedLinear":
                            var directedLineString = selectedGeometry.GetValueOrDefault<string>("directedLineString");
                            if (string.IsNullOrEmpty(directedLineString))
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid geometry coordinates",
                                    Message = "Geometry coordinates linked to 'DirectedLinear'",
                                    Path = "Source -> Provision -> RegulatedPlace -> DirectedLinear -> directedLineString",
                                    Rule = "The coordinates cannot be null"
                                };

                                errors.Add(error);
                            }

                            isBritishGrid = IsBritishGrid(directedLineString);
                            if (!isBritishGrid)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid geometry grid",
                                    Message = "Geometry grid linked to 'DirectedLinear'",
                                    Path = "Source -> Provision -> RegulatedPlace -> DirectedLinear -> directedLineString",
                                    Rule = "The grid must be 'SRID=27700"
                                };

                                errors.Add(error);
                            }
                            areValidCoordinates = AreValidCoordinates(directedLineString);
                            if (!areValidCoordinates)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid coordinates pair",
                                    Message = "Geometry grid linked to 'DirectedLinear'",
                                    Path = "Source -> Provision -> RegulatedPlace -> DirectedLinear -> directedLineString",
                                    Rule = $"Check the coordinates pairs '{directedLineString}'"
                                };

                                errors.Add(error);
                            }

                            isWithinBoundaries = IsWithinUkBoundaries(directedLineString, concreteGeometry);
                            if (!isWithinBoundaries)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid coordinates",
                                    Message = "Geometry grid linked to 'DirectedLinear'",
                                    Path = "Source -> Provision -> RegulatedPlace -> DirectedLinear -> directedLineString",
                                    Rule = $"Coordinates '{directedLineString}' must be within Great Britain"
                                };

                                errors.Add(error);
                            }
                            break;
                    }
                }
            }

        return errors;
    }

    private bool IsBritishGrid(string source)
    {
        var parts = source.Split(';');
        var first = parts.First();
        return first == "SRID=27700";
    }

    private bool AreValidCoordinates(string source)
    {
        WKTReader wktReader = new();
        var parts = source.Split(';');
        var last = parts.Last();
        var read = wktReader.Read(last);
        return read.IsValid;
    }


    private bool IsWithinUkBoundaries(string source, string concreteGeometry)
    {
        const string ukBoundaryWkt = "POLYGON((0 0, 700000 0, 700000 1300000, 0 1300000, 0 0))";
        WKTReader wktReader = new();
        Polygon ukBoundary = wktReader.Read(ukBoundaryWkt) as Polygon;
        var coordinates = source.Split(';').Last();
        bool isWithinUk = false;
        switch (concreteGeometry)
        {
            case "PointGeometry":
                Point pointToValidate = wktReader.Read(coordinates) as Point;
                isWithinUk = ukBoundary != null && ukBoundary.Contains(pointToValidate);
                break;
            case "LinearGeometry":
                LineString lineStringToValidate = wktReader.Read(coordinates) as LineString;
                isWithinUk = ukBoundary != null && ukBoundary.Contains(lineStringToValidate);
                break;
            case "Polygon":
                Polygon polygonToValidate = wktReader.Read(coordinates) as Polygon;
                isWithinUk = ukBoundary != null && ukBoundary.Contains(polygonToValidate);
                break;
            case "DirectedLinear":
                LineString directedLinearToValidate = wktReader.Read(coordinates) as LineString;
                isWithinUk = ukBoundary != null && ukBoundary.Contains(directedLinearToValidate);
                break;
        }
        return isWithinUk;
    }
}