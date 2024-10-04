using DfT.DTRO.Models.Validation;

namespace DfT.DTRO.Services.Validation;

public class RecordManagementService : IRecordManagementService
{
    private readonly IDtroUserDal _dtroUserDal;

    public RecordManagementService(IDtroUserDal swaCodeDal)
    {
        _dtroUserDal = swaCodeDal;
    }

    public List<SemanticValidationError> ValidateCreationRequest(DtroSubmit dtroSubmit, int? submittedByTa)
    {
        List<SemanticValidationError> validationErrors = new();

        if (submittedByTa == null)
        {
            validationErrors.Add(new SemanticValidationError { Message = "TRA cannot be null" });
        }

        List<DtroUserResponse> swaCodes = _dtroUserDal.GetAllDtroUsersAsync().Result;

        int creator = dtroSubmit.Data.GetExpando("Source").GetValueOrDefault<int>("traCreator");
        bool isCreatorWithinSwaCodes = swaCodes.Select(response => response.TraId == creator).Any();
        if (!isCreatorWithinSwaCodes)
        {
            validationErrors.Add(new SemanticValidationError
            {
                Message = $"TRA creator '{creator}' is in our records."
            });
        }

        int owner = dtroSubmit.Data.GetExpando("Source").GetValueOrDefault<int>("currentTraOwner");
        bool isOwnerWithinSwaCodes = swaCodes.Select(response => response.TraId == owner).Any();
        if (!isOwnerWithinSwaCodes)
        {
            validationErrors.Add(new SemanticValidationError
            {
                Message = $"TRA owner '{owner}' is not in our records."
            });
        }

        if (creator != submittedByTa || owner != submittedByTa)
        {
            validationErrors.Add(new SemanticValidationError
            {
                Message = $"TRA '{submittedByTa}' cannot add/update a DTRO for another TRA. (creator TRA is '{creator}', owner TRA is '{owner}' )"
            });
        }

        IList<object> traAffected = dtroSubmit.Data.GetExpando("Source").GetValue<IList<object>>("traAffected");
        if (traAffected.Any(it => it is not long))
        {
            validationErrors.Add(new SemanticValidationError
            {
                Message = "One or more TRA affected identification is incorrect."
            });
        }

        List<int?> traIds = swaCodes.Where(it => it.TraId != null).Select(it => it.TraId).ToList();
        List<long> traAffectedIds = traAffected.Select(it => it).Cast<long>().ToList();
        bool isTraAffectedWithinSwaCodes = traAffectedIds.TrueForAll(it => traIds.Contains((int)it));

        if (!isTraAffectedWithinSwaCodes)
        {
            validationErrors.Add(new SemanticValidationError
            {
                Message = "One or more TRA affected is not in our records."
            });
        }

        var sourceReference = dtroSubmit.Data.GetExpando("Source").GetValueOrDefault<string>("reference");
        if (string.IsNullOrWhiteSpace(sourceReference))
        {
            validationErrors.Add(new SemanticValidationError
            {
                Message = "Source reference is not of type string"
            });
        }

        var sourceActionType = dtroSubmit.Data.GetExpando("Source").GetValueOrDefault<string>("actionType");
        if (!sourceActionType.IsEnum("SourceActionType"))
        {
            validationErrors.Add(new SemanticValidationError
            {
                Message = "Source action type has to be contain one of the accepted values: new, amendment, noChange, errorFix."
            });
        }

        List<string> provisionReferences = dtroSubmit.Data.GetValueOrDefault<IList<object>>("Source.provision")
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

        List<string> provisionActionTypes = dtroSubmit.Data.GetValueOrDefault<IList<object>>("Source.provision")
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