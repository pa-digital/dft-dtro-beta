namespace DfT.DTRO.Services.Validation.Implementation;

/// <inheritdoc cref="IExternalReferenceValidationService"/>
public class ExternalReferenceValidationService : IExternalReferenceValidationService
{
    private readonly SystemClock _clock = new();

    /// <inheritdoc cref="IExternalReferenceValidationService"/>
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
            foreach (var concreteGeometry in Constants.ConcreteGeometries)
            {
                if (!geometry.HasField(concreteGeometry))
                {
                    continue;
                }

                var hasExternalReference = geometry
                    .GetExpandoOrDefault(concreteGeometry)
                    .HasField("ExternalReference");
                if (!hasExternalReference)
                {
                    continue;
                }

                var lastDateUpdates = geometry
                    .GetValueOrDefault<IList<object>>($"{concreteGeometry}.ExternalReference")
                    .OfType<ExpandoObject>()
                    .Select(it => it.GetDateTimeOrNull("lastUpdateDate"))
                    .ToList();

                if (!lastDateUpdates.All(dateTime => dateTime.HasValue))
                {
                    var error = new SemanticValidationError
                    {
                        Name = "Missing last update date",
                        Message = "Indicates the date the USRN reference was last updated",
                        Path = $"Source -> Provision -> RegulatedPlace -> {concreteGeometry} -> ExternalReference -> lastUpdateDate",
                        Rule = "One or more 'lastUpdateDate' is missing"
                    };

                    errors.Add(error);
                }


                if (lastDateUpdates.All(dateTime => dateTime >= _clock.UtcNow))
                {
                    var error = new SemanticValidationError
                    {
                        Name = "Invalid last update date",
                        Message = "Indicates the date the USRN reference was last updated",
                        Path = $"Source -> Provision -> RegulatedPlace -> {concreteGeometry} -> ExternalReference -> lastUpdateDate",
                        Rule = $"'lastUpdateDate' cannot be in the future"
                    };

                    errors.Add(error);
                }
            }
        }

        return errors;
    }
}