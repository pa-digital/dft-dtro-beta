using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

namespace DfT.DTRO.Services.Validation.Implementation;

/// <inheritdoc cref="IGeometryValidationService"/>
public class GeometryValidationService : IGeometryValidationService
{
    private readonly List<SemanticValidationError> _errors = new();

    /// <inheritdoc cref="IGeometryValidationService"/>
    public List<SemanticValidationError> Validate(DtroSubmit dtroSubmit)
    {
        if (dtroSubmit.SchemaVersion < new SchemaVersion("3.3.0"))
        {
            var passedInOldGeometries = dtroSubmit
                .Data
                .GetValueOrDefault<IList<object>>($"{Constants.Source}.{Constants.Provision}".ToBackwardCompatibility(dtroSubmit.SchemaVersion))
                .OfType<ExpandoObject>()
                .SelectMany(provisions => provisions
                    .GetValueOrDefault<IList<object>>($"{Constants.RegulatedPlace}".ToBackwardCompatibility(dtroSubmit.SchemaVersion))
                    .OfType<ExpandoObject>())
            .Where(expandoObject => expandoObject.HasField(Constants.Geometry))
            .ToList();

            var oldGeometries = passedInOldGeometries.Select(it => it.GetExpandoOrDefault(Constants.Geometry)).ToList();
            foreach (var oldGeometry in oldGeometries)
            {
                var version = oldGeometry.GetValueOrDefault<long>(Constants.Version);
                if (version == 0)
                {
                    var error = new SemanticValidationError
                    {
                        Name = $"Invalid geometry '{Constants.Version}'",
                        Message = "Version of geometry linked to a concrete instance of geometry",
                        Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.RegulatedPlace} -> {Constants.Geometry} -> {Constants.Version}",
                        Rule = $"{Constants.Version} must be an integer and cannot be negative or 0"
                    };

                    _errors.Add(error);
                }

                foreach (var concreteGeometry in Constants.ConcreteGeometries.Where(oldGeometry.HasField))
                {
                    var selectedGeometry = oldGeometry.GetExpando(concreteGeometry);
                    switch (concreteGeometry)
                    {
                        case "PointGeometry":
                            var point = selectedGeometry.GetValueOrDefault<string>(Constants.Point);
                            if (string.IsNullOrEmpty(point))
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid geometry coordinates",
                                    Message = $"Geometry coordinates linked to '{Constants.PointGeometry}'",
                                    Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.RegulatedPlace} -> {Constants.PointGeometry} -> {Constants.Point}",
                                    Rule = "The coordinates cannot be null"
                                };

                                _errors.Add(error);
                            }

                            var isBritishGrid = IsBritishGrid(point);
                            if (!isBritishGrid)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid geometry grid",
                                    Message = $"Geometry grid linked to '{Constants.PointGeometry}'",
                                    Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.RegulatedPlace} -> {Constants.PointGeometry} -> {Constants.Point}",
                                    Rule = $"The WKT string must start with '{Constants.Srid27700}'"
                                };

                                _errors.Add(error);
                            }
                            var isWithinBoundaries = IsWithinUkBoundaries(point, concreteGeometry);
                            if (!isWithinBoundaries)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid coordinates",
                                    Message = $"Geometry coordinates linked to '{Constants.PointGeometry}'",
                                    Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.RegulatedPlace} -> {Constants.PointGeometry} -> {Constants.Point}",
                                    Rule = $"Coordinates '{point}' are incorrect or not within Great Britain"
                                };

                                _errors.Add(error);
                            }

                            var representation = selectedGeometry.GetValueOrDefault<string>(Constants.Representation);
                            if (string.IsNullOrEmpty(representation))
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = $"Missing '{Constants.Representation}'",
                                    Message = "Indicates the nature of the point location for a point representation of a regulated place.",
                                    Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.RegulatedPlace} -> {Constants.PointGeometry} -> {Constants.Representation}",
                                    Rule = $"'{Constants.Representation}' is missing"
                                };

                                _errors.Add(error);
                            }

                            var isValidRepresentation = Constants.PointTypes.Any(it => representation != null && representation.Equals(it));
                            if (!isValidRepresentation)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = $"Invalid '{Constants.Representation}'",
                                    Message = "Indicates the nature of the point location for a point representation of a regulated place.",
                                    Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.RegulatedPlace} -> {Constants.PointGeometry} -> {Constants.Representation}",
                                    Rule = $"'{Constants.Representation}' must be one of '{string.Join(",", Constants.PointTypes)}'"
                                };

                                _errors.Add(error);
                            }

                            break;
                        case "LinearGeometry":
                            var direction = selectedGeometry.GetValueOrDefault<string>(Constants.Direction);
                            if (string.IsNullOrEmpty(direction))
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Missing direction",
                                    Message = "Indicates the direction of the applicability of the referenced regulation.",
                                    Path = "Source -> Provision -> RegulatedPlace -> LinearGeometry -> direction",
                                    Rule = "'direction' is missing"
                                };

                                _errors.Add(error);
                            }
                            var isValidLinearDirectionType =
                                Constants.LinearDirectionTypes.Any(it => direction != null && direction.Equals(it));
                            if (!isValidLinearDirectionType)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid direction",
                                    Message = "Indicates the direction of the applicability of the referenced regulation.",
                                    Path = "Source -> Provision -> RegulatedPlace -> LinearGeometry -> direction",
                                    Rule = $"'direction' must be on of '{string.Join(",", Constants.LinearDirectionTypes)}'"
                                };

                                _errors.Add(error);
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

                                _errors.Add(error);
                            }

                            var isValidLateralPosition = Constants.LinearLateralPositionTypes.Any(it => lateralPosition != null && lateralPosition.Equals(it));
                            if (!isValidLateralPosition)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid lateral position",
                                    Message = "Indicates the lateral position across a road of the linear representation of a regulated place.",
                                    Path = "Source -> Provision -> RegulatedPlace -> LinearGeometry -> lateralPosition",
                                    Rule = $"'lateralPosition' must be one of '{string.Join(",", Constants.LinearLateralPositionTypes)}'"
                                };

                                _errors.Add(error);
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

                                _errors.Add(error);
                            }

                            isBritishGrid = IsBritishGrid(lineString);
                            if (!isBritishGrid)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid geometry grid",
                                    Message = "Geometry grid linked to 'LinearGeometry'",
                                    Path = "Source -> Provision -> RegulatedPlace -> LinearGeometry -> linestring",
                                    Rule = $"The WKT string must start with '{Constants.Srid27700}'"
                                };

                                _errors.Add(error);
                            }

                            isWithinBoundaries = IsWithinUkBoundaries(lineString, concreteGeometry);
                            if (!isWithinBoundaries)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid coordinates",
                                    Message = "Geometry grid linked to 'LinearGeometry'",
                                    Path = "Source -> Provision -> RegulatedPlace -> LinearGeometry -> linestring",
                                    Rule = $"Coordinates '{lineString}' are incorrect or not within Great Britain"
                                };

                                _errors.Add(error);
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

                                _errors.Add(error);
                            }

                            var isValidLinearType = Constants.LinearTypes.Any(it => representation != null && representation.Equals(it));
                            if (!isValidLinearType)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid representation",
                                    Message = "Indicates the nature of the point location for a linear representation of a regulated place.",
                                    Path = "Source -> Provision -> RegulatedPlace -> LinearGeometry -> representation",
                                    Rule = $"'representation' must be one of '{string.Join(",", Constants.LinearTypes)}'"
                                };

                                _errors.Add(error);
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

                                _errors.Add(error);
                            }

                            isBritishGrid = IsBritishGrid(polygon);
                            if (!isBritishGrid)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid geometry grid",
                                    Message = "Geometry grid linked to 'Polygon'",
                                    Path = "Source -> Provision -> RegulatedPlace -> Polygon -> polygon",
                                    Rule = $"The WKT string must start with '{Constants.Srid27700}'"
                                };

                                _errors.Add(error);
                            }

                            isWithinBoundaries = IsWithinUkBoundaries(polygon, concreteGeometry);
                            if (!isWithinBoundaries)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid coordinates",
                                    Message = "Indicates that the given coordinates are broadly appropriate",
                                    Path = "Source -> Provision -> RegulatedPlace -> Polygon -> polygon",
                                    Rule = $"Coordinates '{polygon}' are incorrect or not within Great Britain"
                                };

                                _errors.Add(error);
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

                                _errors.Add(error);
                            }

                            isBritishGrid = IsBritishGrid(directedLineString);
                            if (!isBritishGrid)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid geometry grid",
                                    Message = "Geometry grid linked to 'DirectedLinear'",
                                    Path = "Source -> Provision -> RegulatedPlace -> DirectedLinear -> directedLineString",
                                    Rule = $"The WKT string must start with '{Constants.Srid27700}'"
                                };

                                _errors.Add(error);
                            }

                            isWithinBoundaries = IsWithinUkBoundaries(directedLineString, concreteGeometry);
                            if (!isWithinBoundaries)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid coordinates",
                                    Message = "Indicates that the given coordinates are broadly appropriate",
                                    Path = "Source -> Provision -> RegulatedPlace -> DirectedLinear -> directedLineString",
                                    Rule = $"Coordinates '{directedLineString}' are incorrect or not within Great Britain"
                                };

                                _errors.Add(error);
                            }
                            break;
                    }
                }
            }
        }
        else if (dtroSubmit.SchemaVersion >=new SchemaVersion("3.3.0") && dtroSubmit.SchemaVersion < new SchemaVersion("3.4.0"))
        {
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
            {
                foreach (var concreteGeometry in Constants.ConcreteGeometries.Where(passedInGeometry.HasField))
                {
                    var selectedGeometry = passedInGeometry.GetExpando(concreteGeometry);
                    var version = selectedGeometry.GetValueOrDefault<long>("version");
                    if (version == 0)
                    {
                        var error = new SemanticValidationError
                        {
                            Name = "Invalid geometry version",
                            Message = "Version of geometry linked to a concrete instance of geometry",
                            Path = "Source -> Provision -> RegulatedPlace -> Geometry -> version",
                            Rule = $"Version number must be an integer and cannot be '{version}'"
                        };

                        _errors.Add(error);
                    }

                    switch (concreteGeometry)
                    {
                        case "PointGeometry":
                            var point = selectedGeometry.GetValueOrDefault<string>("point");
                            if (string.IsNullOrEmpty(point))
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid coordinates",
                                    Message = "Geometry coordinates linked to 'PointGeometry'",
                                    Path = "Source -> Provision -> RegulatedPlace -> PointGeometry -> point",
                                    Rule = "The coordinates cannot be null"
                                };

                                _errors.Add(error);
                            }

                            var isBritishGrid = IsBritishGrid(point);
                            if (!isBritishGrid)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid geometry grid",
                                    Message = "Geometry grid linked to 'PointGeometry'",
                                    Path = "Source -> Provision -> RegulatedPlace -> PointGeometry -> point",
                                    Rule = $"The WKT string must start with '{Constants.Srid27700}'"
                                };

                                _errors.Add(error);
                            }

                            var isWithinBoundaries = IsWithinUkBoundaries(point, concreteGeometry);
                            if (!isWithinBoundaries)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid coordinates",
                                    Message = "Geometry grid linked to 'PointGeometry'",
                                    Path = "Source -> Provision -> RegulatedPlace -> PointGeometry -> point",
                                    Rule = $"Coordinates '{point}' are incorrect or not within Great Britain"
                                };

                                _errors.Add(error);
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

                                _errors.Add(error);
                            }

                            var isValidRepresentation = Constants.PointTypes.Any(it => representation != null && representation.Equals(it));
                            if (!isValidRepresentation)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid representation",
                                    Message = "Indicates the nature of the point location for a point representation of a regulated place.",
                                    Path = "Source -> Provision -> RegulatedPlace -> PointGeometry -> representation",
                                    Rule = $"'representation' must be one of '{string.Join(",", Constants.PointTypes)}'"
                                };

                                _errors.Add(error);
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

                                _errors.Add(error);
                            }

                            var isValidLinearDirectionType = Constants.LinearDirectionTypes.Any(it => direction != null && direction.Equals(it));
                            if (!isValidLinearDirectionType)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid direction",
                                    Message = "Indicates the direction of the applicability of the referenced regulation.",
                                    Path = "Source -> Provision -> RegulatedPlace -> LinearGeometry -> direction",
                                    Rule = $"'direction' must be on of '{string.Join(",", Constants.LinearDirectionTypes)}'"
                                };

                                _errors.Add(error);
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

                                _errors.Add(error);
                            }

                            var isValidLateralPosition = Constants.LinearLateralPositionTypes.Any(it => lateralPosition != null && lateralPosition.Equals(it));
                            if (!isValidLateralPosition)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid lateral position",
                                    Message = "Indicates the lateral position across a road of the linear representation of a regulated place.",
                                    Path = "Source -> Provision -> RegulatedPlace -> LinearGeometry -> lateralPosition",
                                    Rule = $"'lateralPosition' must be one of '{string.Join(",", Constants.LinearLateralPositionTypes)}'"
                                };

                                _errors.Add(error);
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

                                _errors.Add(error);
                            }

                            isBritishGrid = IsBritishGrid(lineString);
                            if (!isBritishGrid)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid geometry grid",
                                    Message = "Geometry grid linked to 'LinearGeometry'",
                                    Path = "Source -> Provision -> RegulatedPlace -> LinearGeometry -> linestring",
                                    Rule = $"The WKT string must start with '{Constants.Srid27700}'"
                                };

                                _errors.Add(error);
                            }

                            isWithinBoundaries = IsWithinUkBoundaries(lineString, concreteGeometry);
                            if (!isWithinBoundaries)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid coordinates",
                                    Message = "Geometry grid linked to 'LinearGeometry'",
                                    Path = "Source -> Provision -> RegulatedPlace -> LinearGeometry -> linestring",
                                    Rule = $"Coordinates '{lineString}' are incorrect or not within Great Britain"
                                };

                                _errors.Add(error);
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

                                _errors.Add(error);
                            }

                            var isValidLinearType = Constants.LinearTypes.Any(it => representation != null && representation.Equals(it));
                            if (!isValidLinearType)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid representation",
                                    Message = "Indicates the nature of the point location for a linear representation of a regulated place.",
                                    Path = "Source -> Provision -> RegulatedPlace -> LinearGeometry -> representation",
                                    Rule = $"'representation' must be one of '{string.Join(",", Constants.LinearTypes)}'"
                                };

                                _errors.Add(error);
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

                                _errors.Add(error);
                            }

                            isBritishGrid = IsBritishGrid(polygon);
                            if (!isBritishGrid)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid geometry grid",
                                    Message = "Geometry grid linked to 'Polygon'",
                                    Path = "Source -> Provision -> RegulatedPlace -> Polygon -> polygon",
                                    Rule = $"The WKT string must start with '{Constants.Srid27700}'"
                                };

                                _errors.Add(error);
                            }

                            isWithinBoundaries = IsWithinUkBoundaries(polygon, concreteGeometry);
                            if (!isWithinBoundaries)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid coordinates",
                                    Message = "Indicates that the given coordinates are broadly appropriate",
                                    Path = "Source -> Provision -> RegulatedPlace -> Polygon -> polygon",
                                    Rule = $"Coordinates '{polygon}' are incorrect or not within Great Britain"
                                };

                                _errors.Add(error);
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

                                _errors.Add(error);
                            }

                            isBritishGrid = IsBritishGrid(directedLineString);
                            if (!isBritishGrid)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid geometry grid",
                                    Message = "Geometry grid linked to 'DirectedLinear'",
                                    Path = "Source -> Provision -> RegulatedPlace -> DirectedLinear -> directedLineString",
                                    Rule = $"The WKT string must start with '{Constants.Srid27700}'"
                                };

                                _errors.Add(error);
                            }

                            isWithinBoundaries = IsWithinUkBoundaries(directedLineString, concreteGeometry);
                            if (!isWithinBoundaries)
                            {
                                var error = new SemanticValidationError
                                {
                                    Name = "Invalid coordinates",
                                    Message = "Indicates that the given coordinates are broadly appropriate",
                                    Path = "Source -> Provision -> RegulatedPlace -> DirectedLinear -> directedLineString",
                                    Rule = $"Coordinates '{directedLineString}' are incorrect or not within Great Britain"
                                };

                                _errors.Add(error);
                            }
                            break;
                    }
                }
            }
        }
        else
        {
            var regulatedPlaces = dtroSubmit
                .Data
                .GetValueOrDefault<IList<object>>($"{Constants.Source}.{Constants.Provision}"
                    .ToBackwardCompatibility(dtroSubmit.SchemaVersion))
                .OfType<ExpandoObject>()
                .SelectMany(expandoObject => expandoObject
                    .GetValue<IList<object>>($"{Constants.RegulatedPlace}"
                        .ToBackwardCompatibility(dtroSubmit.SchemaVersion))
                    .OfType<ExpandoObject>())
                .ToList();

            var regulatedPlaceTypes = regulatedPlaces
                .Select(regulatedPlace => regulatedPlace.GetValueOrDefault<string>(Constants.Type))
                .ToList();
            var areDiversionRoutes = regulatedPlaceTypes
                .Any(RegulatedPlaceType.DiversionRoute.GetDisplayName().Equals);

            if (areDiversionRoutes)
            {
                var passedInGeometries = dtroSubmit
                    .Data
                    .GetValueOrDefault<IList<object>>($"{Constants.Source}.{Constants.Provision}".ToBackwardCompatibility(dtroSubmit.SchemaVersion))
                    .OfType<ExpandoObject>()
                    .SelectMany(provisions => provisions
                        .GetValueOrDefault<IList<object>>($"{Constants.RegulatedPlace}".ToBackwardCompatibility(dtroSubmit.SchemaVersion))
                        .OfType<ExpandoObject>())
                    .Where(expandoObject => Constants.ConcreteGeometries.Any(expandoObject.HasField))
                    .ToList();

                var hasDiversionTypes = regulatedPlaces
                    .Any(regulatedPlace => regulatedPlace.HasField(Constants.DiversionTypeObj));

                if (hasDiversionTypes)
                {
                    var diversionTypes = regulatedPlaces
                        .Select(regulatedPlace =>
                            regulatedPlace.GetExpandoOrDefault(Constants.DiversionTypeObj))
                        .ToList();

                    var diversionRouteTypes = diversionTypes
                        .Select(diversionType => 
                            diversionType.GetValueOrDefault<string>(Constants.DiversionType))
                        .ToList();

                    var areAcceptedDiversionRouteTypes = diversionRouteTypes.TrueForAll(diversionRouteType =>
                        Constants.DiversionRouteTypes.Any(diversionRouteType.Equals));

                    if (!areAcceptedDiversionRouteTypes)
                    {
                        var error = new SemanticValidationError
                        {
                            Name = $"Invalid '{Constants.DiversionType}'",
                            Message = "",
                            Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.RegulatedPlace} -> {Constants.Geometry} -> {Constants.DiversionTypeObj} -> {Constants.DiversionType}",
                            Rule = $"'{Constants.DiversionType}' must be of type '{typeof(string)}' and one of {string.Join(",", Constants.DiversionRouteTypes)}"
                        };

                        _errors.Add(error);
                    }
                }
            }
        }
        return _errors;
    }

    private static bool IsBritishGrid(string source)
    {
        var parts = source.Split(';');
        var first = parts.First();
        return first == Constants.Srid27700;
    }

    private static bool IsWithinUkBoundaries(string source, string concreteGeometry)
    {
        const string ukBoundaryWkt = "POLYGON((0 0, 700000 0, 700000 1300000, 0 1300000, 0 0))";
        WKTReader wktReader = new();
        Polygon ukBoundary = wktReader.Read(ukBoundaryWkt) as Polygon;
        var coordinates = source.Split(';').Last();
        bool isWithinUk = false;
        try
        {
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
        catch
        {
            return false;
        }
    }
}