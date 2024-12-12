#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace DfT.DTRO.Services.Validation.Implementation;

public class RegulatedPlaceValidation : IRegulatedPlaceValidation
{
    public IList<SemanticValidationError> ValidateRegulatedPlacesType(DtroSubmit request, SchemaVersion schemaVersion)
    {
        var errors = new List<SemanticValidationError>();

        List<ExpandoObject> regulatedPlaces = request
            .Data
            .GetValueOrDefault<IList<object>>("Source.Provision".ToBackwardCompatibility(request.SchemaVersion))
            .OfType<ExpandoObject>()
            .SelectMany(expandoObject => expandoObject
                .GetValue<IList<object>>("RegulatedPlace".ToBackwardCompatibility(request.SchemaVersion))
                .OfType<ExpandoObject>())
            .ToList();

        if (schemaVersion < new SchemaVersion("3.3.0"))
        {
            return errors;
        }

        var passedInTypes = regulatedPlaces.Select(it => it.GetValueOrDefault<string>("type"));
        var regulatedPlaceTypes = typeof(RegulatedPlaceType).GetDisplayNames<RegulatedPlaceType>();
        var areValidRegulationTypes = passedInTypes
            .All(passedInType => passedInType != null &&
                                 regulatedPlaceTypes.Any(passedInType.Equals));

        if (areValidRegulationTypes)
        {
            return errors;
        }

        SemanticValidationError error = new()
        {
            Name = "Regulate place type",
            Message = "Regulated place type missing or incorrect.",
            Path = "Source -> Provision -> RegulatedPlace",
            Rule = $"'{RegulatedPlaceType.RegulationLocation}' type must be present.",
        };

        errors.Add(error);

        return errors;
    }
}