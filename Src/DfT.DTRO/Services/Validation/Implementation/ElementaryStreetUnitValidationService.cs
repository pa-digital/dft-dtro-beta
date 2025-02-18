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
            .GetValueOrDefault<IList<object>>("source.provision".ToBackwardCompatibility(dtroSubmit.SchemaVersion))
            .OfType<ExpandoObject>()
            .SelectMany(provisions => provisions
                .GetValueOrDefault<IList<object>>("regulatedPlace".ToBackwardCompatibility(dtroSubmit.SchemaVersion))
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
                    .HasField("externalReference");
                if (!hasExternalReference)
                {
                    continue;
                }

                var externalReferences = geometry
                    .GetValueOrDefault<IList<object>>($"{concreteGeometry}.externalReference")
                    .OfType<ExpandoObject>()
                    .ToList();

                var uniqueStreetReferenceNumbers = externalReferences
                    .SelectMany(externalReference => externalReference.GetValueOrDefault<IList<object>>("uniqueStreetReferenceNumber"))
                    .OfType<ExpandoObject>()
                    .ToList();

                var elementaryStreetUnits = uniqueStreetReferenceNumbers
                    .Where(it => it.HasField("elementaryStreetUnit"))
                    .SelectMany(uniqueStreetReferenceNumber => uniqueStreetReferenceNumber.GetValueOrDefault<IList<object>>("elementaryStreetUnit"))
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
                        Path = $"source -> provision -> regulatedPlace -> {concreteGeometry} -> externalReference -> uniqueStreetReferenceNumber -> elementaryStreetUnit -> esu",
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