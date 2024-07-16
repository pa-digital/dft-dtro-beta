using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using DfT.DTRO.DAL;
using DfT.DTRO.Extensions;
using DfT.DTRO.Models.DtroDtos;
using DfT.DTRO.Models.SwaCode;
using DfT.DTRO.Models.Validation;

namespace DfT.DTRO.Services.Validation;

public class RecordManagementService : IRecordManagementService
{
    private readonly ISwaCodeDal _swaCodeDal;

    public RecordManagementService(ISwaCodeDal swaCodeDal)
    {
        _swaCodeDal = swaCodeDal;
    }

    public List<SemanticValidationError> ValidateCreationRequest(DtroSubmit dtroSubmit, int? ta)
    {
        List<SemanticValidationError> validationErrors = new();

        if (ta == null)
        {
            validationErrors.Add(new SemanticValidationError { Message = "TRA cannot be null" });
        }

        List<SwaCodeResponse> swaCodes = _swaCodeDal.GetAllCodes().Result;

        int creator = dtroSubmit.Data.GetExpando("source").GetValueOrDefault<int>("traCreator");
        bool isCreatorWithinSwaCodes = swaCodes.Select(response => response.TraId == creator).Any();
        if (!isCreatorWithinSwaCodes)
        {
            validationErrors.Add(new SemanticValidationError
            {
                Message = $"TRA creator '{creator}' is not within accepted SWA codes."
            });
        }

        int owner = dtroSubmit.Data.GetExpando("source").GetValueOrDefault<int>("currentTraOwner");
        bool isOwnerWithinSwaCodes = swaCodes.Select(response => response.TraId == owner).Any();
        if (!isOwnerWithinSwaCodes)
        {
            validationErrors.Add(new SemanticValidationError
            {
                Message = $"TRA owner '{owner}' is not within accepted SWA codes."
            });
        }

        IList<object> traAffected = dtroSubmit.Data.GetExpando("source").GetValue<IList<object>>("traAffected");
        if (traAffected.Any(it => it is not long))
        {
            validationErrors.Add(new SemanticValidationError
            {
                Message = "One or more traffic authorities affected identification is incorrect."
            });
        }

        validationErrors.AddRange(traAffected.Cast<long>()
            .Select(item => new
            {
                item,
                isTraAffectedWithinSwaCodes = swaCodes
                    .Select(response => response.TraId == item)
                    .Any()
            })
            .Where(it => !it.isTraAffectedWithinSwaCodes)
            .Select(it =>
                new SemanticValidationError
                {
                    Message = $"TRA affected '{it.item}' is not within the accepted SWA codes"
                }));

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
            .Select(it => it.GetValue<string>("actionType"))
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