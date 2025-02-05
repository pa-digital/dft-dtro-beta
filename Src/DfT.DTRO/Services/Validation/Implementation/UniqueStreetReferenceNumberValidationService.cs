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

                var usrns = uniqueStreetReferenceNumbers
                    .Select(uniqueStreetReferenceNumber => uniqueStreetReferenceNumber
                        .GetValueOrDefault<long>("usrn"))
                    .ToList();

                if (usrns.Any(usrn => usrn == 0) || usrns.Any(usrn => usrn > 99999999))
                {
                    var error = new SemanticValidationError
                    {
                        Name = "Invalid usrn",
                        Message = "One or more 'usrn' are invalid",
                        Path = $"Source -> Provision -> RegulatedPlace -> {concreteGeometry} -> ExternalReference -> UniqueStreetReferenceNumber -> usrn",
                        Rule = "'usrn' value should be between 0 and 99999999"
                    };

                    errors.Add(error);
                }

                var duplicates = usrns
                .GroupBy(usrn => usrn)
                .Where(usrn => usrn.Count() > 1)
                .Select(key => key)
                .ToList();

                if (duplicates.Any())
                {
                    var error = new SemanticValidationError
                    {
                        Name = "Duplicate unique street reference numbers",
                        Message = "Object to enable linkage of Regulated Place geometry to the National Street Gazetteer Unique Street Reference Number",
                        Path = $"Source -> Provision -> RegulatedPlace -> {concreteGeometry} -> ExternalReference -> UniqueStreetReferenceNumber -> usrn",
                        Rule = $"'usrn' number must be unique"
                    };

                    errors.Add(error);
                }
            }
        }

        return errors;
    }
}
