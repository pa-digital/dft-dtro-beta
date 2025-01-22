namespace DfT.DTRO.Services.Validation.Implementation;

/// <inheritdoc cref="IRateTableValidationService"/>
public class RateTableValidationService : IRateTableValidationService
{
    /// <inheritdoc cref="IRateTableValidationService"/>
    public List<SemanticValidationError> Validate(DtroSubmit dtroSubmit)
    {
        List<SemanticValidationError> errors = new();

        var regulations = dtroSubmit
            .Data
            .GetValueOrDefault<IList<object>>("Source.Provision".ToBackwardCompatibility(dtroSubmit.SchemaVersion))
            .OfType<ExpandoObject>()
            .SelectMany(provision => provision
                .GetValueOrDefault<IList<object>>("Regulation".ToBackwardCompatibility(dtroSubmit.SchemaVersion))
                .OfType<ExpandoObject>())
            .ToList();

        if (regulations.Any(it => !it.HasField("Condition") && !it.HasField("ConditionSet")))
        {
            return errors;
        }

        var rateTables = regulations
            .Select(regulation => regulation
                .GetExpandoOrDefault("RateTable"
                    .ToBackwardCompatibility(dtroSubmit.SchemaVersion)))
            .Where(rateTable => rateTable != null)
            .ToList();

        if (!rateTables.Any())
        {
            return errors;
        }


        var multipleUris = rateTables
            .Where(rateTable => rateTable.HasField(Constants.AdditionalInformation))
            .Select(rateTable => rateTable.GetValueOrDefault<string>(Constants.AdditionalInformation))
            .ToList();

        var areValidUris = multipleUris
            .Where(multipleUri => !string.IsNullOrEmpty(multipleUri))
            .Select(multipleUri => Uri.TryCreate(multipleUri, UriKind.Absolute, out var uri) &&
                                   (uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == Uri.UriSchemeHttp))
            .ToList();

        if (areValidUris.Any(it => it == false))
        {
            SemanticValidationError error = new()
            {
                Name = "Additional information",
                Message = "URI locator for supplementary additional information concerning use of the rate table.",
                Path = "Source -> Provision -> Regulation -> Condition -> RateTable -> additionalInformation",
                Rule = "If present, additional information must be formatted as URI",
            };

            errors.Add(error);
        }

        var passedInTypes = rateTables
            .Where(rateTable => rateTable.HasField(Constants.Type))
            .Select(rateTable => rateTable.GetValueOrDefault<string>(Constants.Type))
            .ToList();

        var areValidTypes = passedInTypes
            .Where(passedInType => !string.IsNullOrEmpty(passedInType))
            .All(passedInType => Constants.RateTypes.Any(passedInType.Equals));

        if (!areValidTypes)
        {
            SemanticValidationError error = new()
            {
                Name = "Rate type",
                Message = "Defines the type of rate in use.",
                Path = "Source -> Provision -> Regulation -> Condition -> RateTable -> type",
                Rule = $"Rate type must be one of '{string.Join(",", Constants.RateTypes)}'",
            };

            errors.Add(error);
        }

        return errors;
    }
}