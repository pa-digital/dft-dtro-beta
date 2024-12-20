namespace DfT.DTRO.Services.Validation.Implementation;

/// <inheritdoc cref="IRegulatedPlaceValidationService"/>
public class RegulatedPlaceValidationService : IRegulatedPlaceValidationService
{
    /// <inheritdoc cref="IRegulatedPlaceValidationService"/>
    public IList<SemanticValidationError> ValidateRegulatedPlaces(DtroSubmit dtroSubmit)
    {
        var errors = new List<SemanticValidationError>();

        List<ExpandoObject> regulatedPlaces = dtroSubmit
            .Data
            .GetValueOrDefault<IList<object>>("Source.Provision".ToBackwardCompatibility(dtroSubmit.SchemaVersion))
            .OfType<ExpandoObject>()
            .SelectMany(expandoObject => expandoObject
                .GetValue<IList<object>>("RegulatedPlace".ToBackwardCompatibility(dtroSubmit.SchemaVersion))
                .OfType<ExpandoObject>())
            .ToList();

        var descriptions = regulatedPlaces.Select(it => it.GetValueOrDefault<string>("description"));

        if (descriptions.Any(string.IsNullOrEmpty))
        {
            var error = new SemanticValidationError
            {
                Name = "Invalid regulated place description",
                Message = "Free text description of the regulated place",
                Path = "Source -> Provision -> RegulatedPlace -> description",
                Rule = "One or more 'description(s)' are missing"
            };

            errors.Add(error);
        }

        var passedInTypes = regulatedPlaces.Select(it => it.GetValueOrDefault<string>("type")).ToList();

        //TODO: This should be refactored once schema version 3.2.4 will be decommissioned.
        if (passedInTypes.Any(string.IsNullOrEmpty) && dtroSubmit.SchemaVersion < new SchemaVersion("3.3.0"))
        {
            return errors;
        }

        var regulatedPlaceTypes = typeof(RegulatedPlaceType).GetDisplayNames<RegulatedPlaceType>();
        var areValidRegulationTypes = passedInTypes
            .All(passedInType => passedInType != null &&
                                 regulatedPlaceTypes.Any(passedInType.Equals));

        if (!areValidRegulationTypes)
        {
            SemanticValidationError error = new()
            {
                Name = "Regulate place type",
                Message = "Regulated place type missing or incorrect.",
                Path = "Source -> Provision -> RegulatedPlace -> type",
                Rule = $"One of '{string.Join(",", typeof(RegulatedPlaceType).GetDisplayNames<RegulatedPlaceType>())}' type(s) must be present.",
            };

            errors.Add(error);
        }

        return errors;
    }
}