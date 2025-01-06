namespace DfT.DTRO.Services.Validation.Implementation;

/// <inheritdoc cref="IElementaryStreetUnitValidationService"/>
public class ElementaryStreetUnitValidationService : IElementaryStreetUnitValidationService
{
    /// <inheritdoc cref="IElementaryStreetUnitValidationService"/>
    public List<SemanticValidationError> Validate(DtroSubmit dtroSubmit)
    {
        var errors = new List<SemanticValidationError>();

        var geometries = dtroSubmit
            .Data
            .GetValueOrDefault<IList<object>>("Source.Provision".ToBackwardCompatibility(dtroSubmit.SchemaVersion))
            .OfType<ExpandoObject>()
            .SelectMany(provisions => provisions
                .GetValueOrDefault<IList<object>>("RegulatedPlace".ToBackwardCompatibility(dtroSubmit.SchemaVersion))
                .OfType<ExpandoObject>())
            .Where(expandoObject => Constants.ConcreteGeometries.Any(expandoObject.HasField))
            .Where(expandoObject => expandoObject != null)
            .ToList();

        foreach (var geometry in geometries)
        {
            foreach (var concreteGeometry in Constants.ConcreteGeometries.Where(geometry.HasField))
            {
                var externalReferences = geometry
                    .GetValueOrDefault<IList<object>>($"{concreteGeometry}.ExternalReference")
                    .OfType<ExpandoObject>()
                    .ToList();

                var uniqueStreetReferenceNumbers = externalReferences
                    .SelectMany(externalReference => externalReference.GetValueOrDefault<IList<object>>("UniqueStreetReferenceNumber"))
                    .OfType<ExpandoObject>()
                    .ToList();

                if (!uniqueStreetReferenceNumbers.Any(it => it.HasField("ElementaryStreetUnit")))
                {
                    return errors;
                }

                var elementaryStreetUnits = uniqueStreetReferenceNumbers
                    .SelectMany(uniqueStreetReferenceNumber => uniqueStreetReferenceNumber.GetValueOrDefault<IList<object>>("ElementaryStreetUnit"))
                    .OfType<ExpandoObject>()
                    .ToList();

                var esus = elementaryStreetUnits
                    .Select(elementaryStreetUnit => elementaryStreetUnit
                        .GetValueOrDefault<long>("esu"))
                    .ToList();

                if (esus.Any(usrn => usrn == 0))
                {
                    var error = new SemanticValidationError
                    {
                        Name = "Invalid esu",
                        Message = "",
                        Path = $"Source -> Provision -> RegulatedPlace -> {concreteGeometry} -> ExternalReference -> UniqueStreetReferenceNumber -> ElementaryStreetUnit -> esu",
                        Rule = "One or more 'esu' is invalid"
                    };

                    errors.Add(error);
                }
            }
        }

        return errors;
    }
}