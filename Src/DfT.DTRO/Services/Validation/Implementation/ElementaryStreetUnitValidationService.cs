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
                var hasExternalReference = geometry
                    .GetExpandoOrDefault(concreteGeometry)
                    .HasField("ExternalReference");
                if (!hasExternalReference)
                {
                    continue;
                }

                var externalReferences = geometry
                    .GetValueOrDefault<IList<object>>($"{concreteGeometry}.ExternalReference")
                    .OfType<ExpandoObject>()
                    .ToList();

                var uniqueStreetReferenceNumbers = externalReferences
                    .SelectMany(externalReference => externalReference.GetValueOrDefault<IList<object>>("UniqueStreetReferenceNumber"))
                    .OfType<ExpandoObject>()
                    .ToList();

                var elementaryStreetUnits = uniqueStreetReferenceNumbers
                    .Where(it => it.HasField("ElementaryStreetUnit"))
                    .SelectMany(uniqueStreetReferenceNumber => uniqueStreetReferenceNumber.GetValueOrDefault<IList<object>>("ElementaryStreetUnit"))
                    .OfType<ExpandoObject>()
                    .ToList();

                var esus = elementaryStreetUnits
                    .Select(elementaryStreetUnit => elementaryStreetUnit
                        .GetValueOrDefault<long>("esu"))
                    .ToList();

                if (!esus.TrueForAll(esu => esu is > 10000000 and < 100000000000000))
                {
                    var error = new SemanticValidationError
                    {
                        Name = "Invalid esu ID",
                        Message = "One or more “esu” are invalid",
                        Path = $"Source -> Provision -> RegulatedPlace -> {concreteGeometry} -> ExternalReference -> UniqueStreetReferenceNumber -> ElementaryStreetUnit -> esu",
                        Rule = "'esu' value should follow the NSG DEC convention and be between 10,000,001 (8 digits) and 99,999,999,999,999 (14 digits) and specified as an integer (no leading zeros). " +
                               "This shall correspond to a value found in the National Street Gazetteer"
                    };

                    errors.Add(error);
                }
            }
        }

        return errors;
    }
}