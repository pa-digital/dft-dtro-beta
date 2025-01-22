namespace DfT.DTRO.Services.Validation.Implementation;

/// <inheritdoc cref="IProvisionValidationService"/>
public class ProvisionValidationService : IProvisionValidationService
{
    /// <inheritdoc cref="IProvisionValidationService"/>
    public List<SemanticValidationError> Validate(DtroSubmit dtroSubmit)
    {
        List<SemanticValidationError> validationErrors = new();

        var provisions = dtroSubmit.Data
            .GetValueOrDefault<IList<object>>("Source.Provision"
                .ToBackwardCompatibility(dtroSubmit.SchemaVersion))
            .Cast<ExpandoObject>()
            .ToList();

        var actionTypes = provisions.Select(provision => provision.GetValue<string>("actionType")).ToList();
        var areActionTypesValid = actionTypes.TrueForAll(actionType => Constants.ProvisionActionTypes.Any(actionType.Equals));
        if (!areActionTypesValid)
        {
            var error = new SemanticValidationError
            {
                Name = "Invalid 'actionType'",
                Message = "Indicates the nature of update",
                Rule = $"Provision 'actionType' must contain one of the following accepted values: '{string.Join(",", Constants.ProvisionActionTypes)}'",
                Path = "Source -> Provision -> actionType"
            };

            validationErrors.Add(error);
        }

        var orderReportingPoints = provisions
            .Select(provision => provision.GetValueOrDefault<string>("orderReportingPoint"))
            .ToList();

        var areValidOrderReportingPoints = orderReportingPoints
            .TrueForAll(orderReportingPoint => Constants.OrderReportingPointTypes.Any(orderReportingPoint.Equals));

        if (!areValidOrderReportingPoints)
        {
            var error = new SemanticValidationError
            {
                Name = "Invalid order reporting point",
                Message = "Attribute identifying the lifecycle point and nature of a Provision",
                Rule = $"'orderReportingPoint' must be one of '{string.Join(",", Constants.OrderReportingPointTypes)}'",
                Path = "Source -> Provision -> orderReportingPoint"
            };

            validationErrors.Add(error);
        }

        var provisionDescription = provisions
            .Select(it => it.GetValueOrDefault<string>("provisionDescription"))
            .ToList();

        if (provisionDescription.Any(string.IsNullOrEmpty))
        {
            var error = new SemanticValidationError
            {
                Name = "Invalid description",
                Message = "Free text description of the referenced provision",
                Rule = "Provision 'provisionDescription' must be of type 'string'",
                Path = "Source -> Provision -> provisionDescription"
            };

            validationErrors.Add(error);
        }

        var references = provisions.Select(provision => provision.GetValueOrDefault<string>("reference")).ToList();
        if (references.Any(string.IsNullOrWhiteSpace))
        {
            var error = new SemanticValidationError
            {
                Name = "Invalid reference",
                Message = "Indicates a system reference to the relevant Provision of the TRO",
                Rule = "Provision 'reference' must be of type 'string'",
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
                    Rule = "Each provision 'reference' must be unique and of type 'string'",
                    Path = "Source -> Provision -> reference"
                };

                return error;
            });

            validationErrors.AddRange(errors);
        }

        return validationErrors;
    }
}