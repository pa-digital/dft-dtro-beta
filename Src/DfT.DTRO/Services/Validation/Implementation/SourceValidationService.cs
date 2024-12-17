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
    public List<SemanticValidationError> ValidateSource(DtroSubmit dtroSubmit, int? submittedByTa)
    {
        List<SemanticValidationError> validationErrors = [];

        var source = dtroSubmit.Data.GetExpando("Source");

        var actionType = source.GetValueOrDefault<string>("actionType");
        if (!actionType.IsEnum("SourceActionType"))
        {
            var error = new SemanticValidationError
            {
                Name = $"Invalid '{nameof(SourceActionType)}' error",
                Message = "Indicates the nature of update between D-TRO records or their constituent parts",
                Rule = $"Source '{actionType}' must contain one of the following accepted values: '{string.Join(",", Enum.GetNames<SourceActionType>())}'",
                Path = "Source -> actionType"
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
                Rule = $"Source traffic regulation authority current owner has to be one of '{string.Join(",", traCodes)}'",
                Path = "Source -> currentTraOwner"
            };

            validationErrors.Add(error);
        }

        var reference = source.GetValueOrDefault<string>("reference");
        if (string.IsNullOrWhiteSpace(reference))
        {
            var error = new SemanticValidationError
            {
                Name = $"'Invalid {nameof(reference)}'",
                Message = "Indicates the nature of update between D-TRO records or their constituent parts.",
                Rule = $"Source '{nameof(reference)}' must be of type '{typeof(string)}'",
                Path = "Source -> reference"
            };

            validationErrors.Add(error);
        }

        var section = source.GetValueOrDefault<string>("section");
        if (string.IsNullOrEmpty(section))
        {
            var error = new SemanticValidationError
            {
                Name = $"Invalid '{nameof(section)}'",
                Message = "Reference to the section of the D-TRO",
                Rule = $"Source '{nameof(section)}' need to be present.",
                Path = "Source -> section"
            };

            validationErrors.Add(error);
        }

        var traAffectedCodes = source.GetValueOrDefault<IList<object>>("traAffected").Cast<long>().ToList();
        if (!traAffectedCodes.Any())
        {
            var error = new SemanticValidationError
            {
                Name = $"Invalid or null '{nameof(traAffectedCodes)}'",
                Message = "Traffic regulation authorities who roads are affected by this D-TRO",
                Rule = $"Source '{nameof(traAffectedCodes)}' must not be null and an array of integers",
                Path = "Source -> traAffected"
            };
            validationErrors.Add(error);
        }

        var isTraAffectedWithinSwaCodes = traAffectedCodes
            .TrueForAll(traAffectedCode => traCodes.Contains((int)traAffectedCode));

        if (!isTraAffectedWithinSwaCodes)
        {
            validationErrors.Add(new SemanticValidationError
            {
                Name = $"Invalid 'Traffic regulation authority codes'",
                Message = "Traffic regulation authorities who roads are affected by this D-TRO",
                Rule = $"One of the source '{string.Join(",", traAffectedCodes)}' codes are not register",
                Path = "Source -> traAffected"
            });
        }

        var traCreator = source.GetValueOrDefault<int>("traCreator");
        var isCreatorWithinSwaCodes = users.Select(response => response.TraId == traCreator).Any();
        if (!isCreatorWithinSwaCodes)
        {
            var error = new SemanticValidationError
            {
                Name = "Invalid 'Current Traffic regulation authority creator'",
                Message = "Traffic regulation authority maintaining this D-TRO (SWA-like code)",
                Rule = $"Traffic regulation authority creator has to be one of '{string.Join(",", traCodes)}'",
                Path = "Source -> traCreator"
            };

            validationErrors.Add(error);
        }

        if (submittedByTa == null)
        {
            var semanticValidationError = new SemanticValidationError
            {
                Name = "Invalid 'traffic regulation authority' code",
                Message = "Traffic regulation authority code submitting D-TRO record",
                Rule = "Traffic regulation authority code cannot be null"
            };

            validationErrors.Add(semanticValidationError);
        }

        if (traCreator != submittedByTa || currentTraOwner != submittedByTa)
        {
            var error = new SemanticValidationError
            {
                Name = "Traffic regulation authority code submitted is invalid",
                Message = $"TRA '{submittedByTa}' cannot add/update a TRO for another TRA. (This D-TRO creator ID is '{traCreator}', owner ID is '{currentTraOwner}' )",
                Rule = $"'{traCreator}' or '{currentTraOwner}' must be the same with '{submittedByTa}'",
                Path = "Source -> traCreator and Source -> currentTraOwner"
            };

            validationErrors.Add(error);
        }

        var troName = source.GetValueOrDefault<string>("troName");
        if (string.IsNullOrEmpty(troName))
        {
            var error = new SemanticValidationError
            {
                Name = $"Invalid '{nameof(troName)}'",
                Message = "Traffic regulation order published title",
                Rule = $"Source '{nameof(troName)}' need to be present",
                Path = "Source -> troName"
            };

            validationErrors.Add(error);
        }

        return validationErrors;
    }
}