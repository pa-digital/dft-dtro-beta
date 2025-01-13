namespace DfT.DTRO.Services.Validation.Implementation;

/// <inheritdoc cref="ISourceValidationService"/>
public class SourceValidationService : ISourceValidationService
{
    private readonly IDtroUserDal _dtroUserDal;


    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="swaCodeDal">D-TRO user data access layer</param>
    public SourceValidationService(IDtroUserDal swaCodeDal) => _dtroUserDal = swaCodeDal;

    /// <inheritdoc cref="ISourceValidationService"/>
    public List<SemanticValidationError> Validate(DtroSubmit dtroSubmit, int? submittedByTa)
    {
        List<SemanticValidationError> validationErrors = new();

        var source = dtroSubmit.Data.GetExpando("Source");

        var actionType = source.GetValueOrDefault<string>("actionType");
        if (!actionType.IsEnum("SourceActionType"))
        {
            var error = new SemanticValidationError
            {
                Name = "Invalid 'actionType'",
                Message = "Indicates the nature of update between D-TRO records or their constituent parts",
                Path = "Source -> actionType",
                Rule = $"Source 'actionType' must contain one of the following accepted values: '{string.Join(",", typeof(SourceActionType).GetDisplayNames<SourceActionType>())}'"
            };

            validationErrors.Add(error);
        }

        var users = _dtroUserDal.GetAllDtroUsersAsync().Result;
        var traCodes = users.Where(user => user != null).Select(user => user.TraId).ToList();

        var currentTraOwner = source.GetValueOrDefault<int>("currentTraOwner");
        var isOwnerWithinSwaCodes = traCodes.Any(traCode => traCode == currentTraOwner);
        if (!isOwnerWithinSwaCodes)
        {
            var error = new SemanticValidationError
            {
                Name = "Invalid 'Current Traffic regulation authority current owner'",
                Message = "Current Traffic regulation authority maintaining this D-TRO (SWA-like code)",
                Path = "Source -> currentTraOwner",
                Rule = $"Source 'currentTraOwner' must contain one of these '{string.Join(",", traCodes)}' codes"
            };

            validationErrors.Add(error);
        }

        var reference = source.GetValueOrDefault<string>("reference");
        if (string.IsNullOrWhiteSpace(reference))
        {
            var error = new SemanticValidationError
            {
                Name = "Invalid 'reference'",
                Message = "Indicates the nature of update between D-TRO records or their constituent parts.",
                Path = "Source -> reference",
                Rule = $"Source 'reference' must be of type '{typeof(string)}'"
            };

            validationErrors.Add(error);
        }

        var section = source.GetValueOrDefault<string>("section");
        if (string.IsNullOrEmpty(section))
        {
            var error = new SemanticValidationError
            {
                Name = "Invalid 'section'",
                Message = "Reference to the section of the D-TRO",
                Path = "Source -> section",
                Rule = "Source 'section' need to be present."
            };

            validationErrors.Add(error);
        }

        var traAffectedCodes = source.GetValueOrDefault<IList<object>>("traAffected").Cast<long>().ToList();
        if (!traAffectedCodes.Any())
        {
            var error = new SemanticValidationError
            {
                Name = "Invalid or null 'traAffected'",
                Message = "Traffic regulation authorities who roads are affected by this D-TRO",
                Path = "Source -> traAffected",
                Rule = "Source 'traAffected' must not be null and an array of integers"
            };
            validationErrors.Add(error);
        }

        var isTraAffectedWithinSwaCodes = traAffectedCodes
            .TrueForAll(traAffectedCode => traCodes.Contains((int)traAffectedCode));

        if (!isTraAffectedWithinSwaCodes)
        {
            validationErrors.Add(new SemanticValidationError
            {
                Name = "Invalid 'traAffected'",
                Message = "Traffic regulation authorities who roads are affected by this D-TRO",
                Path = "Source -> traAffected",
                Rule = $"One or more of the Source '{string.Join(",", traAffectedCodes)}' codes are not register"
            });
        }

        var traCreator = source.GetValueOrDefault<int>("traCreator");
        var isCreatorWithinSwaCodes = users.Any(response => response.TraId == traCreator);
        if (!isCreatorWithinSwaCodes)
        {
            var error = new SemanticValidationError
            {
                Name = "Invalid 'traCreator'",
                Message = "Traffic regulation authority maintaining this D-TRO (SWA-like code)",
                Path = "Source -> traCreator",
                Rule = $"Source 'traCreator' has to be one of '{string.Join(",", traCodes)}'"
            };

            validationErrors.Add(error);
        }

        if (submittedByTa == null)
        {
            var semanticValidationError = new SemanticValidationError
            {
                Name = "Invalid 'traCreator' code",
                Message = "Traffic regulation authority code submitting D-TRO record (SWA-like code)",
                Rule = "Source 'traCreator' code cannot be null"
            };

            validationErrors.Add(semanticValidationError);
        }

        if (traCreator != submittedByTa || currentTraOwner != submittedByTa)
        {
            var error = new SemanticValidationError
            {
                Name = "Traffic regulation authority code submitted is invalid",
                Message = $"TRA '{submittedByTa}' cannot add/update a TRO for another TRA. (This D-TRO creator ID is '{traCreator}', owner ID is '{currentTraOwner}' )",
                Path = "Source -> traCreator and Source -> currentTraOwner",
                Rule = $"'{submittedByTa}' must be '{traCreator}' or '{currentTraOwner}'",
            };

            validationErrors.Add(error);
        }

        var troName = source.GetValueOrDefault<string>("troName");
        if (string.IsNullOrEmpty(troName))
        {
            var error = new SemanticValidationError
            {
                Name = $"Invalid 'troName'",
                Message = "Traffic regulation order published title",
                Rule = $"Source 'troName' need to be present",
                Path = "Source -> troName"
            };

            validationErrors.Add(error);
        }

        return validationErrors;
    }
}