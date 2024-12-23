namespace DfT.DTRO.Services.Validation.Implementation;

/// <inheritdoc cref="IUniqueStreetReferenceNumberValidationService"/>
public class UniqueStreetReferenceNumberValidationService : IUniqueStreetReferenceNumberValidationService
{
    /// <inheritdoc cref="IUniqueStreetReferenceNumberValidationService"/>
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

                var usrns = uniqueStreetReferenceNumbers
                    .Select(uniqueStreetReferenceNumber => uniqueStreetReferenceNumber
                        .GetValueOrDefault<long>("usrn"))
                    .ToList();

                if (usrns.Any(usrn => usrn == 0))
                {
                    var error = new SemanticValidationError
                    {
                        Name = "Invalid usrn",
                        Message = "",
                        Path = $"Source -> Provision -> RegulatedPlace -> {concreteGeometry} -> ExternalReference -> UniqueStreetReferenceNumber -> usrn",
                        Rule = "One or more 'usrn' is invalid"
                    };

                    errors.Add(error);
                }
            }
        }

        return errors;
    }
}
