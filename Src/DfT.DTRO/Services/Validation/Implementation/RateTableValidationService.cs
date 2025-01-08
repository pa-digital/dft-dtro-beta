namespace DfT.DTRO.Services.Validation.Implementation;

/// <inheritdoc cref="IRateTableValidationService"/>
public class RateTableValidationService : IRateTableValidationService
{
    /// <inheritdoc cref="IRateTableValidationService"/>
    public List<SemanticValidationError> Validate(DtroSubmit dtroSubmit)
    {
        List<SemanticValidationError> errors = new();

        var rateTables = dtroSubmit
            .Data
            .GetValueOrDefault<IList<object>>("Source.Provision".ToBackwardCompatibility(dtroSubmit.SchemaVersion))
            .OfType<ExpandoObject>()
            .SelectMany(expandoObject => expandoObject
                .GetValueOrDefault<IList<object>>("Regulation".ToBackwardCompatibility(dtroSubmit.SchemaVersion))
                .OfType<ExpandoObject>())
            .SelectMany(expandoObject => expandoObject
                .GetValueOrDefault<IList<object>>("Condition".ToBackwardCompatibility(dtroSubmit.SchemaVersion))
                .OfType<ExpandoObject>())
            .Select(expandoObject => expandoObject.GetValueOrDefault<ExpandoObject>("RateTable"))
            .ToList();

        var multipleUris = rateTables
            .Select(rateTable => rateTable.GetValueOrDefault<string>(Constants.AdditionalInformation))
            .ToList();

        var isValidUri = multipleUris
            .Where(multipleUri => !string.IsNullOrEmpty(multipleUri))
            .Select(multipleUri => Uri.TryCreate(multipleUri, UriKind.Absolute, out var uri) &&
                                   (uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == Uri.UriSchemeHttp))
            .ToList();

        if (isValidUri.Any(it => it == false))
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
            .Select(rateTable => rateTable.GetValueOrDefault<string>(Constants.Type))
            .ToList();

        var areValidTypes = passedInTypes
            .Where(passedInType => !string.IsNullOrEmpty(passedInType))
            .Select(passedInType => Constants.RateTypes.Any(passedInType.Equals))
            .ToList();

        if (areValidTypes.Any(isValidType => isValidType == false))
        {
            SemanticValidationError error = new()
            {
                Name = "Rate type",
                Message = "Defines the type of rate in use. [Defined by APDS - ISO/TS 5206-1]",
                Path = "Source -> Provision -> Regulation -> Condition -> RateTable -> type",
                Rule = $"Rate type must be one of '{string.Join(",", Constants.RateTypes)}'",
            };

            errors.Add(error);
        }

        //var rateLineCollections = rateTables
        //    .SelectMany(expandoObject => expandoObject
        //        .GetValueOrDefault<IList<object>>("RateLineCollection")
        //        .OfType<ExpandoObject>())
        //    .ToList();

        //var passedInCurrencies = rateLineCollections
        //    .Select(rateLineCollection => rateLineCollection.GetValueOrDefault<string>(Constants.ApplicableCurrency))
        //    .ToList();

        //var areValidCurrencies = passedInCurrencies
        //    .Where(passedInCurrency => !string.IsNullOrEmpty(passedInCurrency))
        //    .Select(passedInCurrency => Constants.CurrencyTypes.Any(passedInCurrency.Equals))
        //    .ToList();

        //if (areValidCurrencies.Any(isValidCurrency => isValidCurrency == false))
        //{
        //    SemanticValidationError error = new()
        //    {
        //        Name = "Applicable currency",
        //        Message = "The monetary currency that rates are specified in this rate line collection. [Defined by APDS - ISO/TS 5206-1]",
        //        Path = "Source -> Provision -> Regulation -> Condition -> RateTable -> RateLineCollection -> applicableCurrency",
        //        Rule = $"Applicable currency must be one of '{string.Join(",", Constants.CurrencyTypes)}'",
        //    };

        //    errors.Add(error);
        //}

        return errors;
    }
}