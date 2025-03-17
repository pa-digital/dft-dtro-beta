namespace DfT.DTRO.Services.Validation.Implementation;

/// <inheritdoc cref="IConsultationValidationService"/>
public class ConsultationValidationService : IConsultationValidationService
{
    private readonly SystemClock _systemClock = new();
    private readonly Regex _emailPattern = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

    /// <inheritdoc cref="IConsultationValidationService"/>
    public List<SemanticValidationError> Validate(DtroSubmit dtroSubmit)
    {
        List<SemanticValidationError> validationErrors = [];
        SemanticValidationError error;

        var consultation = dtroSubmit.Data.GetExpandoOrDefault(Constants.Consultation);
        if (consultation == null)
        {
            return validationErrors;
        }

        var consultationName = consultation.GetValueOrDefault<string>(Constants.ConsultationName);
        if (string.IsNullOrEmpty(consultationName))
        {
            error = new SemanticValidationError
            {
                Name = $"Invalid '{Constants.ConsultationName}'",
                Message = "Free text descriptive name for the consultation given by the Traffic Regulation Authority.",
                Rule = $"'{Constants.ConsultationName}' must be present and of type {typeof(string)}",
                Path = $"Consultation -> {Constants.ConsultationName}"
            };

            validationErrors.Add(error);
            return validationErrors;
        }

        var endOfConsultation = consultation.GetValueOrDefault<string>(Constants.EndOfConsultation);
        var isValidEndOfConsultationDateTime = DateTime.TryParse(endOfConsultation, out var endOfConsultationDateTime);
        if (!isValidEndOfConsultationDateTime)
        {
            error = new SemanticValidationError
            {
                Name = $"Invalid '{Constants.EndOfConsultation}'",
                Message = "Time and date of the end of the consultation period.",
                Rule = $"'{Constants.EndOfConsultation}' must be present and of type {typeof(DateTime)}",
                Path = $"Consultation -> {Constants.EndOfConsultation}"
            };

            validationErrors.Add(error);
            return validationErrors;
        }

        var isFutureEndOfConsultation = endOfConsultationDateTime > _systemClock.UtcNow;
        if (isFutureEndOfConsultation)
        {
            error = new SemanticValidationError
            {
                Name = $"Invalid '{Constants.EndOfConsultation}'",
                Message = "Time and date of the end of the consultation period.",
                Rule = $"'{Constants.EndOfConsultation}' cannot be in the future.",
                Path = $"Consultation -> {Constants.EndOfConsultation}"
            };

            validationErrors.Add(error);
            return validationErrors;
        }

        var howToComment = consultation.GetValueOrDefault<string>(Constants.HowToComment);
        if (consultation.HasField(Constants.HowToComment) && string.IsNullOrEmpty(howToComment))
        {
            error = new SemanticValidationError
            {
                Name = $"Invalid '{Constants.HowToComment}'",
                Message = "Free text description detailing how to comment on the proposal which is the subject of the consultation.",
                Rule = $"'{Constants.HowToComment}' if present, must be of type {typeof(string)}.",
                Path = $"Consultation -> {Constants.HowToComment}"
            };

            validationErrors.Add(error);
            return validationErrors;
        }

        var localReference = consultation.GetValueOrDefault<string>(Constants.LocalReference);
        if (consultation.HasField(Constants.LocalReference) && string.IsNullOrEmpty(localReference))
        {
            error = new SemanticValidationError
            {
                Name = $"Invalid '{Constants.LocalReference}'",
                Message = "Free text reference to an identifier for the consultation defined by the Traffic Regulation Authority.",
                Rule = $"'{Constants.LocalReference}' if present, must be of type {typeof(string)}.",
                Path = $"Consultation -> {Constants.LocalReference}"
            };

            validationErrors.Add(error);
            return validationErrors;
        }

        var pointOfContactAddress = consultation.GetValueOrDefault<string>(Constants.PointOfContactAddress);
        if (consultation.HasField(Constants.PointOfContactAddress) && string.IsNullOrEmpty(pointOfContactAddress))
        {
            error = new SemanticValidationError
            {
                Name = $"Invalid '{Constants.PointOfContactAddress}'",
                Message = "Postal or office address location to support receipt of comments / objections to consultation proposals.",
                Rule = $"'{Constants.PointOfContactAddress}' if present, must be of type {typeof(string)}.",
                Path = $"Consultation -> {Constants.PointOfContactAddress}"
            };

            validationErrors.Add(error);
            return validationErrors;
        }

        var pointOfContactEmail = consultation.GetValueOrDefault<string>(Constants.PointOfContactEmail);
        if (consultation.HasField(Constants.PointOfContactEmail))
        {
            var isEmail = _emailPattern.IsMatch(pointOfContactEmail);
            if (!isEmail)
            {
                error = new SemanticValidationError
                {
                    Name = $"Invalid '{Constants.PointOfContactEmail}'",
                    Message = "Email address location to support receipt of comments / objections to consultation proposals.",
                    Rule = $"'{Constants.PointOfContactEmail}' must be of type {typeof(string)} and formatted as email",
                    Path = $"Consultation -> {Constants.PointOfContactEmail}"
                };

                validationErrors.Add(error);
                return validationErrors;
            }
        }

        var startOfConsultation = consultation.GetValueOrDefault<string>(Constants.StartOfConsultation);
        if (consultation.HasField(Constants.StartOfConsultation))
        {
            var isStartOfConsultationValidDateTime = DateTime.TryParse(startOfConsultation, out var startOfConsultationDateTime);
            if (!isStartOfConsultationValidDateTime)
            {
                error = new SemanticValidationError
                {
                    Name = $"Invalid '{Constants.StartOfConsultation}'",
                    Message = "Time and date of the end of the consultation period.",
                    Rule = $"'{Constants.StartOfConsultation}' must be present and of type {typeof(DateTime)}",
                    Path = $"Consultation -> {Constants.StartOfConsultation}"
                };

                validationErrors.Add(error);
                return validationErrors;
            }

            var isFutureStartOfConsultation = startOfConsultationDateTime > _systemClock.UtcNow;
            if (isFutureStartOfConsultation)
            {
                error = new SemanticValidationError
                {
                    Name = $"Invalid '{Constants.StartOfConsultation}'",
                    Message = "Time and date of the end of the consultation period.",
                    Rule = $"'{Constants.StartOfConsultation}' cannot be in the future.",
                    Path = $"Consultation -> {Constants.StartOfConsultation}"
                };

                validationErrors.Add(error);
                return validationErrors;
            }

            var isBeforeEndEndOfConsultation = startOfConsultationDateTime >= endOfConsultationDateTime;
            if (isBeforeEndEndOfConsultation)
            {
                error = new SemanticValidationError
                {
                    Name = $"Invalid '{Constants.StartOfConsultation}'",
                    Message = "Time and date of the end of the consultation period.",
                    Rule = $"'{Constants.StartOfConsultation}' cannot be after '{Constants.EndOfConsultation}'.",
                    Path = $"Consultation -> {Constants.StartOfConsultation}"
                };

                validationErrors.Add(error);
                return validationErrors;
            }
        }

        var statementOfReason = consultation.GetValueOrDefault<string>(Constants.StatementOfReason);
        if (string.IsNullOrEmpty(statementOfReason))
        {
            error = new SemanticValidationError
            {
                Name = $"Invalid '{Constants.StatementOfReason}'",
                Message = "Statement of reason for the proposed traffic regulation measure.",
                Rule = $"'{Constants.StatementOfReason}' must be present and of type {typeof(string)}",
                Path = $"Consultation -> {Constants.StatementOfReason}"
            };

            validationErrors.Add(error);
            return validationErrors;
        }

        var urlAdditionalInformation = consultation.GetValueOrDefault<string>(Constants.UrlAdditionalInformation);
        if (!consultation.HasField(Constants.UrlAdditionalInformation))
        {
            return validationErrors;
        }

        var isValidUrlAdditionalInformation =
            Uri.TryCreate(urlAdditionalInformation, UriKind.Absolute, out Uri _);

        if (isValidUrlAdditionalInformation)
        {
            return validationErrors;
        }

        error = new SemanticValidationError
        {
            Name = $"Invalid '{Constants.UrlAdditionalInformation}'",
            Message = "Web address (URL) for where to find further information related to the consultation.",
            Rule = $"'{Constants.UrlAdditionalInformation}' if present, must be of type {typeof(string)} and {typeof(Uri)} formatted.",
            Path = $"Consultation -> {Constants.UrlAdditionalInformation}"
        };
        validationErrors.Add(error);
        return validationErrors;

    }
}