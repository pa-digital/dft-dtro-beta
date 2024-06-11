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
        var validationErrors = new List<SemanticValidationError>();

        var sourceReference = dtroSubmit.Data.GetExpando("source").GetValue<string>("reference");
        if (!sourceReference.IsGuid())
        {
            validationErrors.Add(new SemanticValidationError
            {
                Message = "Source reference is not of type UUID"
            });
        }

        var sourceActionType = dtroSubmit.Data.GetExpando("source").GetValue<string>("actionType");
        if (!sourceActionType.IsEnum("SourceActionType"))
        {
            validationErrors.Add(new SemanticValidationError
            {
                Message = "Source action type has to be contain one of the accepted values: new, amendment, noChange, errorFix."
                //TODO: add the error of the user
            });
        }

        List<string> provisionReferences = dtroSubmit.Data.GetValueOrDefault<IList<object>>("source.provision")
            .OfType<ExpandoObject>()
            .Select(it => it.GetValue<string>("reference"))
            .ToList();

        if (!provisionReferences.TrueForAll(reference => reference.IsGuid()))
        {
            validationErrors.Add(new SemanticValidationError
            {
                Message = "One or more provision reference is not of type UUID"
            });
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