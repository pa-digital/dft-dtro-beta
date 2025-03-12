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

        var source = dtroSubmit.Data.GetExpando(Constants.Source);

        var actionType = source.GetValueOrDefault<string>(Constants.ActionType);
        var isActionTypesValid = Constants.SourceActionTypes.Any(sourceActionType => actionType.Equals(sourceActionType));

        if (!isActionTypesValid)
        {
            var error = new SemanticValidationError
            {
                Name = $"Invalid '{Constants.ActionType}'",
                Message = "Indicates the nature of update between D-TRO records or their constituent parts",
                Path = $"{Constants.Source} -> {Constants.ActionType}",
                Rule = $"{Constants.Source} '{Constants.ActionType}' must contain one of the following accepted values: '{string.Join(",", Constants.SourceActionTypes)}'"
            };

            validationErrors.Add(error);
        }

        DateOnly comingIntoForceValidDate = default;
        if (dtroSubmit.SchemaVersion >= new SchemaVersion("3.4.0") && source.HasField(Constants.ComingIntoForceDate))
        {
            var comingIntoForceDate = source.GetValueOrDefault<string>(Constants.ComingIntoForceDate);
            var isValidDate = DateOnly.TryParse(comingIntoForceDate, out comingIntoForceValidDate);
            if (!isValidDate)
            {
                var error = new SemanticValidationError
                {
                    Name = $"Invalid '{Constants.ComingIntoForceDate}'",
                    Message = "The date that the TRO is coming into force",
                    Path = $"{Constants.Source} -> {Constants.ComingIntoForceDate}",
                    Rule = $"{Constants.ComingIntoForceDate} if present, must be of type {typeof(string)} and formatted as {typeof(DateOnly)}"
                };

                validationErrors.Add(error);
            }
        }

        if (comingIntoForceValidDate > DateOnly.FromDateTime(DateTime.UtcNow))
        {
            var error = new SemanticValidationError
            {
                Name = $"Invalid '{Constants.ComingIntoForceDate}'",
                Message = "The date that the TRO is coming into force",
                Path = $"{Constants.Source} -> {Constants.ComingIntoForceDate}",
                Rule = $"{Constants.ComingIntoForceDate} can not be in the future"
            };

            validationErrors.Add(error);
        }

        var users = _dtroUserDal.GetAllDtroUsersAsync().Result;
        var traCodes = users
            .Where(user => user != null)
            .Select(user => user.TraId)
            .ToList();

        var currentTraOwner = source.GetValueOrDefault<int>(Constants.CurrentTraOwner);
        var isOwnerWithinSwaCodes = traCodes.Any(traCode => traCode == currentTraOwner);
        if (!isOwnerWithinSwaCodes)
        {
            var error = new SemanticValidationError
            {
                Name = $"Invalid '{Constants.CurrentTraOwner}'",
                Message = "Current Traffic regulation authority maintaining this D-TRO (SWA-like code)",
                Path = $"{Constants.Source} -> {Constants.CurrentTraOwner}",
                Rule = $"{Constants.CurrentTraOwner} must be a valid SWA-like code and known to the D-TRO Service; " +
                       "TRA code must correspond to the appropriate APP-ID."
            };

            validationErrors.Add(error);
        }

        DateOnly validMadeDate = default;
        if (dtroSubmit.SchemaVersion >= new SchemaVersion("3.4.0") && source.HasField(Constants.MadeDate))
        {
            var madeDate = source.GetValueOrDefault<string>(Constants.MadeDate);
            var isValidMadeDate = DateOnly.TryParse(madeDate, out validMadeDate);
            if (!isValidMadeDate)
            {
                var error = new SemanticValidationError
                {
                    Name = $"Invalid '{Constants.MadeDate}'",
                    Message = "If appropriate, the date that the TRO is made",
                    Path = $"{Constants.Source} -> {Constants.MadeDate}",
                    Rule = $"{Constants.MadeDate} if present, must be of type {typeof(string)} and formatted as {typeof(DateOnly)}"
                };

                validationErrors.Add(error);
            }
        }

        if (validMadeDate > comingIntoForceValidDate)
        {
            var error = new SemanticValidationError
            {
                Name = $"Invalid '{validMadeDate}'",
                Message = $"{validMadeDate} cannot be later then {comingIntoForceValidDate}",
                Path = $"{Constants.Source} -> {Constants.MadeDate}",
                Rule = $"{Constants.MadeDate} if present, must earlier than {Constants.ComingIntoForceDate}"
            };

            validationErrors.Add(error);
        }

        var reference = source.GetValueOrDefault<string>(Constants.Reference);
        if (string.IsNullOrWhiteSpace(reference))
        {
            var error = new SemanticValidationError
            {
                Name = $"Invalid '{Constants.Reference}'",
                Message = "Indicates a reference to the relevant part of section of the TRO",
                Path = $"{Constants.Source} -> {Constants.Reference}",
                Rule = $"{Constants.Source} '{Constants.Reference}' must be of type {typeof(string)}"
            };

            validationErrors.Add(error);
        }

        var section = source.GetValueOrDefault<string>(Constants.Section);
        if (string.IsNullOrEmpty(section))
        {
            var error = new SemanticValidationError
            {
                Name = $"Invalid '{Constants.Section}'",
                Message = "Reference to the section of the D-TRO",
                Path = $"{Constants.Source} -> {Constants.Section}",
                Rule = $"{Constants.Source} '{Constants.Section}' must be of type '{typeof(string)}'"
            };

            validationErrors.Add(error);
        }

        if (dtroSubmit.SchemaVersion >= new SchemaVersion("3.4.0") && source.HasField(Constants.StatementDescription))
        {
            var statementDescription = source.GetValueOrDefault<string>(Constants.StatementDescription);
            if (string.IsNullOrEmpty(statementDescription))
            {
                var error = new SemanticValidationError
                {
                    Name = $"Invalid '{Constants.StatementDescription}'",
                    Message = "Statement of the overall nature of the prohibition, regulation or restriction imposed",
                    Path = $"{Constants.Source} -> {Constants.StatementDescription}",
                    Rule = $"{Constants.Source} '{Constants.StatementDescription}' if present, must be of type '{typeof(string)}'"
                };

                validationErrors.Add(error);
            }
        }

        var traAffectedCodes = source
            .GetValueOrDefault<IList<object>>(Constants.TraAffected)
            .Cast<long>()
            .ToList();
        if (!traAffectedCodes.Any())
        {
            var error = new SemanticValidationError
            {
                Name = $"Invalid or null '{Constants.TraAffected}'",
                Message = "Traffic regulation authorities who roads are affected by this D-TRO",
                Path = $"{Constants.Source} -> {Constants.TraAffected}",
                Rule = $"{Constants.Source} '{Constants.TraAffected}' can not be null and must be an array of integers"
            };
            validationErrors.Add(error);
        }

        var isTraAffectedWithinSwaCodes = traAffectedCodes
            .TrueForAll(traAffectedCode => traCodes.Contains((int)traAffectedCode));
        if (!isTraAffectedWithinSwaCodes)
        {
            validationErrors.Add(new SemanticValidationError
            {
                Name = $"Invalid '{Constants.TraAffected}'",
                Message = "Traffic regulation authorities who roads are affected by this D-TRO",
                Path = $"{Constants.Source} -> {Constants.TraAffected}",
                Rule = "Current TRA affected must be a valid SWA-like code and known to the D-TRO Service; " +
                       "TRA affected must correspond with the appropriate App-ID"
            });
        }

        var traCreator = source.GetValueOrDefault<int>(Constants.TraCreator);
        var isCreatorWithinSwaCodes = users.Any(response => response.TraId == traCreator);
        if (!isCreatorWithinSwaCodes)
        {
            var error = new SemanticValidationError
            {
                Name = $"Invalid '{Constants.TraCreator}'",
                Message = "Traffic regulation authority maintaining this D-TRO (SWA-like code)",
                Path = $"{Constants.Source} -> {Constants.TraCreator}",
                Rule = "TRA creator must be a valid SWA-like code and known to the D-TRO Service" +
                       "TRA affected must correspond with the appropriate App-ID"
            };

            validationErrors.Add(error);
        }

        if (submittedByTa == null)
        {
            var semanticValidationError = new SemanticValidationError
            {
                Name = $"Invalid '{Constants.TraCreator}' code",
                Message = "Traffic regulation authority code submitting D-TRO record (SWA-like code)",
                Rule = $"{Constants.Source} '{Constants.TraCreator}' code cannot be null"
            };

            validationErrors.Add(semanticValidationError);
        }

        if (traCreator != submittedByTa || currentTraOwner != submittedByTa)
        {
            var error = new SemanticValidationError
            {
                Name = "Traffic regulation authority code submitted is invalid",
                Message = $"TRA '{submittedByTa}' cannot add/update a TRO for another TRA. (This D-TRO creator ID is '{traCreator}', owner ID is '{currentTraOwner}' )",
                Path = $"{Constants.Source} -> {Constants.TraCreator} and {Constants.Source} -> {Constants.CurrentTraOwner}",
                Rule = $"'{submittedByTa}' must be '{traCreator}' or '{currentTraOwner}'",
            };

            validationErrors.Add(error);
        }

        var troName = source.GetValueOrDefault<string>(Constants.TroName);
        if (string.IsNullOrEmpty(troName))
        {
            var error = new SemanticValidationError
            {
                Name = $"Invalid '{Constants.TroName}'",
                Message = "Traffic regulation order published title",
                Rule = $"'{Constants.TroName}' need to be meaningful",
                Path = $"{Constants.Source} -> {Constants.TroName}"
            };

            validationErrors.Add(error);
        }

        return validationErrors;
    }
}