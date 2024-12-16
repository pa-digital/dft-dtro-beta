namespace DfT.DTRO.Services.Validation.Implementation;

/// <inheritdoc cref="IProvisionValidationService"/>
public class ProvisionValidationService : IProvisionValidationService
{
    /// <inheritdoc cref="IProvisionValidationService"/>
    public List<SemanticValidationError> ValidateProvision(DtroSubmit dtroSubmit)
    {
        List<SemanticValidationError> validationErrors = [];

        var provisions = dtroSubmit.Data
            .GetValueOrDefault<IList<object>>("Source.Provision"
                .ToBackwardCompatibility(dtroSubmit.SchemaVersion))
            .Cast<ExpandoObject>()
            .ToList();

        var actionTypes = provisions.Select(provision => provision.GetValue<string>("actionType")).ToList();
        if (!actionTypes.TrueForAll(actionType => actionType.IsEnum("ProvisionActionType")))
        {
            var error = new SemanticValidationError
            {
                Name = $"Invalid '{nameof(ProvisionActionType)}' error",
                Message = "Indicates the nature of update between D-TRO records or their constituent parts",
                Rule = $"Provision '{actionTypes}' must contain one of the following accepted values: '{string.Join(",", Enum.GetNames<ProvisionActionType>())}'",
                Path = "Source -> Provision -> actionType"
            };

            validationErrors.Add(error);
        }

        var orderReportingPoints = provisions
            .Select(provision => provision.GetValueOrDefault<string>("orderReportingPoint"))
            .ToList();

        var areValidOrderReportingPoints = orderReportingPoints
            .Select(orderReportingPoint => orderReportingPoint
                .IsEnum("OrderReportingPointType"))
            .Any();
        if (!areValidOrderReportingPoints)
        {
            var error = new SemanticValidationError
            {
                Name = "Invalid order reporting point",
                Message = "Object identifying the characteristics of a traffic regulation measure (D-TRO & notice) specified Provision",
                Rule = $"One or more provisions 'orderReportingPoint' must be of type '{string.Join(",", Enum.GetNames<OrderReportingPointType>())}'",
                Path = "Source -> Provision -> orderReportingPoint"
            };

            validationErrors.Add(error);
        }

        var descriptions = provisions
            .Select(it => it.GetValueOrDefault<string>("provisionDescription"))
            .ToList();

        if (!descriptions.Any(string.IsNullOrEmpty))
        {
            var error = new SemanticValidationError
            {
                Name = "Invalid provision description",
                Message = "Free text description of the referenced provision",
                Rule = "One or more provision descriptions are missing",
                Path = "Source -> Provision -> provisionDescription"
            };

            validationErrors.Add(error);
        }

        var references = provisions
            .Select(it => it.GetValueOrDefault<string>("reference"))
            .ToList();

        if (references.Any(string.IsNullOrWhiteSpace))
        {
            var error = new SemanticValidationError
            {
                Name = "Invalid reference",
                Message = "Indicates the nature of update between D-TRO records or their constituent parts.",
                Rule = $"Provision 'reference' must be of type '{typeof(string)}'",
                Path = "Source -> Provision -> reference"
            };

            validationErrors.Add(error);
        }

        if (references.Count > 1)
        {
            var dictionary = references
                .GroupBy(reference => reference)
                .Where(grouping => grouping.Count() > 1)
                .ToDictionary(grouping => grouping.Key, grouping => grouping.Count());


            IEnumerable<SemanticValidationError> errors = dictionary.Select(kv =>
            {
                var error = new SemanticValidationError
                {
                    Name = $"'{kv.Value}' duplication reference",
                    Message = $"Provision reference '{kv.Key}' is present {kv.Value} times.",
                    Rule = $"Each provision 'reference' must be unique and of type '{typeof(string)}'",
                    Path = "Source -> Provision -> reference"
                };

                return error;
            });

            validationErrors.AddRange(errors);
        }

        return validationErrors;
    }
}