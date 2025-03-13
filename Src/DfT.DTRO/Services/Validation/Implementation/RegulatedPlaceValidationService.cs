namespace DfT.DTRO.Services.Validation.Implementation;

/// <inheritdoc cref="IRegulatedPlaceValidationService"/>
public class RegulatedPlaceValidationService : IRegulatedPlaceValidationService
{
    /// <inheritdoc cref="IRegulatedPlaceValidationService"/>
    public List<SemanticValidationError> Validate(DtroSubmit dtroSubmit)
    {
        var errors = new List<SemanticValidationError>();

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

        if (!regulatedPlaces.Any())
        {
            var error = new SemanticValidationError
            {
                Name = $"Invalid '{Constants.RegulatedPlace}'",
                Message = "Object indicating the geographical and spatial location subject to a regulation",
                Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.RegulatedPlace}",
                Rule = $"At least one '{Constants.RegulatedPlace}' must be present"
            };

            errors.Add(error);
        }

        var hasAssignments = dtroSubmit.SchemaVersion >= new SchemaVersion("3.4.0") &&
                          regulatedPlaces.Any(regulatedPlace => regulatedPlace.HasField(Constants.Assignment));
        if (hasAssignments)
        {
            var assignments = regulatedPlaces
                .Select(regulatedPlace => 
                    regulatedPlace.GetValueOrDefault<bool>(Constants.Assignment))
                .ToList();

            if (assignments.Any(assignment => assignment != true || assignment != false))
            {
                var error = new SemanticValidationError
                {
                    Name = $"Invalid '{Constants.Assignment}'",
                    Message = "Indicates that the regulated place is subject to an assignment has been granted under section 28(1) of the New Roads and Street Works Act 1991",
                    Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.RegulatedPlace} -> {Constants.Assignment}",
                    Rule = $"'{Constants.Assignment}' if present, must be of type '{typeof(bool)}'"
                };

                errors.Add(error);
            }
        }

        var hasBusRoutes = dtroSubmit.SchemaVersion >= new SchemaVersion("3.4.0") &&
                           regulatedPlaces.Any(regulatedPlace => regulatedPlace.HasField(Constants.BusRoute));
        if (hasBusRoutes)
        {
            var busRoutes = regulatedPlaces
                .Select(regulatedPlace => 
                    regulatedPlace.GetValueOrDefault<bool>(Constants.BusRoute))
                .ToList();

            if (busRoutes.Any(busRoute => busRoute != true || busRoute != false))
            {
                var error = new SemanticValidationError
                {
                    Name = $"Invalid '{Constants.BusRoute}'",
                    Message = "Indicates that the regulated place relates to (1) a road outside Greater London which is included in the route of a local service or (2) a road in Greater London which is included in the route of a London bus service",
                    Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.RegulatedPlace} -> {Constants.BusRoute}",
                    Rule = $"'{Constants.BusRoute}' if present, must be of type '{typeof(bool)}'"
                };

                errors.Add(error);
            }
        }

        var hasByways = dtroSubmit.SchemaVersion >= new SchemaVersion("3.4.0") &&
                        regulatedPlaces.Any(regulatedPlace => regulatedPlace.HasField(Constants.BywayType));
        if (hasByways)
        {
            var byways = regulatedPlaces
                .Select(regulatedPlace => 
                    regulatedPlace.GetValueOrDefault<bool>(Constants.BywayType))
                .ToList();

            var areAcceptedByways = byways
                .TrueForAll(byway => Constants.BywayTypes.Any(byway.Equals));
            if (!areAcceptedByways)
            {
                var error = new SemanticValidationError
                {
                    Name = $"Invalid '{Constants.BywayType}'",
                    Message = "Indicates that the regulated place relates to (1) a road outside Greater London which is included in the route of a local service or (2) a road in Greater London which is included in the route of a London bus service",
                    Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.RegulatedPlace} -> {Constants.BywayType}",
                    Rule = $"'{Constants.BywayType}' if present, must be one of '{string.Join(",", Constants.BywayTypes)}'"
                };

                errors.Add(error);
            }
        }

        var hasConcessions = dtroSubmit.SchemaVersion >= new SchemaVersion("3.4.0") &&
                             regulatedPlaces.Any(regulatedPlace => regulatedPlace.HasField(Constants.Concession));
        if (hasConcessions)
        {
            var concessions = regulatedPlaces
                .Select(regulatedPlace => 
                    regulatedPlace.GetValueOrDefault<bool>(Constants.Concession))
                .ToList();
            if (concessions.Any(concession => concession != true || concession != false))
            {
                var error = new SemanticValidationError
                {
                    Name = $"Invalid '{Constants.Concession}'",
                    Message = "Indicates that the regulated place is subject to a concession within the meaning given by section 1(2) of the New Roads and Street Works Act 1991",
                    Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.RegulatedPlace} -> {Constants.Concession}",
                    Rule = $"'{Constants.Concession}' if present, must be of type '{typeof(bool)}'"
                };

                errors.Add(error);
            }
        }

        var descriptions = regulatedPlaces
            .Select(regulatedPlace => regulatedPlace.GetValueOrDefault<string>(Constants.Description))
            .ToList();
        if (descriptions.Any(string.IsNullOrEmpty))
        {
            var error = new SemanticValidationError
            {
                Name = $"Invalid '{Constants.Description}'",
                Message = "Free text description of the regulated place",
                Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.RegulatedPlace} -> {Constants.Description}",
                Rule = $"'{Constants.Description}' must be present and of type '{typeof(string)}'"
            };

            errors.Add(error);
        }
        
        var hasPassedInTypes = dtroSubmit.SchemaVersion >= new SchemaVersion("3.3.0");
        if (hasPassedInTypes)
        {
            var passedInTypes = regulatedPlaces
                .Select(it => it.GetValueOrDefault<string>(Constants.Type))
                .ToList();

            var areValidRegulationTypes = passedInTypes
                .All(passedInType => passedInType != null &&
                                     Constants.RegulatedPlaceTypes.Any(passedInType.Equals));

            if (!areValidRegulationTypes)
            {
                SemanticValidationError error = new()
                {
                    Name = "Regulate place type",
                    Message = "Regulated place type missing or incorrect.",
                    Path = "Source -> Provision -> RegulatedPlace -> type",
                    Rule = $"One of '{string.Join(",", Constants.RegulatedPlaceTypes)}' type(s) must be present.",
                };

                errors.Add(error);
            }
        }

        var hasTramCar = dtroSubmit.SchemaVersion >= new SchemaVersion("3.4.0") &&
                             regulatedPlaces.Any(regulatedPlace => regulatedPlace.HasField(Constants.TramCar));
        if (hasTramCar)
        {
            var tramCars = regulatedPlaces
                .Select(regulatedPlace => 
                    regulatedPlace.GetValueOrDefault<bool>(Constants.TramCar))
                .ToList();
            if (tramCars.Any(concession => concession != true || concession != false))
            {
                var error = new SemanticValidationError
                {
                    Name = $"Invalid '{Constants.TramCar}'",
                    Message = "Indicates that the regulated place relates to a road on which a tramcar or trolley service vehicle is provided",
                    Path = $"{Constants.Source} -> {Constants.Provision} -> {Constants.RegulatedPlace} -> {Constants.TramCar}",
                    Rule = $"'{Constants.TramCar}' if present, must be of type '{typeof(bool)}'"
                };

                errors.Add(error);
            }
        }

        return errors;
    }
}