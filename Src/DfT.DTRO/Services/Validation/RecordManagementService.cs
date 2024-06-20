using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using DfT.DTRO.Extensions;
using DfT.DTRO.Models.DtroDtos;
using DfT.DTRO.Models.Validation;

namespace DfT.DTRO.Services.Validation;

public class RecordManagementService : IRecordManagementService
{
    public List<SemanticValidationError> ValidateCreationRequest(DtroSubmit dtroSubmit)
    {
        List<SemanticValidationError> validationErrors = new();

        var sourceReference = dtroSubmit.Data.GetExpando("source").GetValueOrDefault<string>("reference");
        if (string.IsNullOrWhiteSpace(sourceReference))
        {
            validationErrors.Add(new SemanticValidationError
            {
                Message = "Source reference is not of type string"
            });
        }

        var sourceActionType = dtroSubmit.Data.GetExpando("source").GetValueOrDefault<string>("actionType");
        if (!sourceActionType.IsEnum("SourceActionType"))
        {
            validationErrors.Add(new SemanticValidationError
            {
                Message = "Source action type has to be contain one of the accepted values: new, amendment, noChange, errorFix."
                //TODO: add the error of the user
            });
        }

        List<string> provisionReferences = dtroSubmit.Data.GetValueOrDefault<IList<object>>("source.provision")
        ?.OfType<ExpandoObject>()
        .Select(it => it.GetValue<string>("reference"))
        .ToList() ?? new List<string>();

        if (provisionReferences.Any(string.IsNullOrWhiteSpace))
        {
            SemanticValidationError error = new()
            {
                Message = "One or more provision reference is not of type string"
            };
            validationErrors.Add(error);
        }

        if (provisionReferences.Count > 1)
        {
            Dictionary<string, int> dictionary = provisionReferences
                .GroupBy(it => it)
                .Where(it => it.Count() > 1)
                .ToDictionary(it => it.Key, it => it.Count());


            IEnumerable<SemanticValidationError> errors = dictionary.Select(kv => new SemanticValidationError
            {
                Message = $"Provision reference '{kv.Key}' is present {kv.Value} times."
            });

            validationErrors.AddRange(errors);
        }

        List<string> provisionActionTypes = dtroSubmit.Data.GetValueOrDefault<IList<object>>("source.provision")
            .OfType<ExpandoObject>()
            .Select(it=>it.GetValue<string>("actionType"))
            .ToList();

        if (!provisionActionTypes.TrueForAll(it => it.IsEnum("ProvisionActionType")))
        {
            validationErrors.Add(new SemanticValidationError
            {
                Message = "Provision action type(s) has to contain one of the accepted values: new, partialAmendment, fullAmendment, partialRevoke, fullRevoke, noChange, errorFix"
            });
        }

        return validationErrors;
    }
}