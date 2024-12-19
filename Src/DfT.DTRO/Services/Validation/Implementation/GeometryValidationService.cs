using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

namespace DfT.DTRO.Services.Validation.Implementation;

/// <inheritdoc cref="IGeometryValidationService"/>
public class GeometryValidationService : IGeometryValidationService
{
    /// <inheritdoc cref="IGeometryValidationService"/>
    public IList<SemanticValidationError> ValidateGeometry(DtroSubmit dtroSubmit)
    {
        var errors = new List<SemanticValidationError>();


        var passedInGeometries = dtroSubmit
            .Data
            .GetValueOrDefault<IList<object>>("Source.Provision".ToBackwardCompatibility(dtroSubmit.SchemaVersion))
            .OfType<ExpandoObject>()
            .SelectMany(provisions => provisions
                .GetValueOrDefault<IList<object>>("RegulatedPlace".ToBackwardCompatibility(dtroSubmit.SchemaVersion))
                .OfType<ExpandoObject>())
            .ToList()
            .Where(expandoObject => Constants.ConcreteGeometries.Any(expandoObject.HasField))
            .ToList();

        foreach (var passedInGeometry in passedInGeometries)
            foreach (var concreteGeometry in Constants.ConcreteGeometries)
            {
                if (passedInGeometry.HasField(concreteGeometry))
                {
                    var selectedGeometry = passedInGeometry.GetExpando(concreteGeometry);
                    switch (concreteGeometry)
                    {
                        case "PointGeometry":
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
                                    Rule = "One or more 'representation' is missing"
                                };

                                errors.Add(error);
                            }

                            var representationPointType = typeof(PointType).GetDisplayNames<PointType>().ToList();
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
                            break;
                        case "Polygon":
                            break;
                        case "DirectedLinear":
                            break;
                    }
                }
            }

        return errors;
    }

    private bool IsBritishGrid(string source)
    {
        var parts = source.Split(';');
        return parts[0] == "SRID=27700";
    }

    private bool AreValidCoordinates(string source)
    {
        WKTReader wktReader = new();
        var geometry = source.Split(';').Last();
        var read = wktReader.Read(geometry);
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