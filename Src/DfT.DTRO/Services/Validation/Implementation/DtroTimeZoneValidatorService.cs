namespace DfT.DTRO.Services.Validation.Implementation;

/// <inheritdoc cref="IDtroTimeZoneValidatorService"/>
public class DtroTimeZoneValidatorService : IDtroTimeZoneValidatorService
{
    private SystemClock _clock = new();

    /// <inheritdoc cref="IDtroTimeZoneValidatorService"/>
    public DtroValidationException Validate(DtroSubmit dtroSubmit)
    {
        var expandoObject = dtroSubmit.Data.GetExpando(Constants.Source);

        var dateTimeValues = new List<DateTime>();
        expandoObject.FindDateTimeValues(dateTimeValues);

        var error = new DtroValidationException();
        error.RequestComparedToRules = new List<SemanticValidationError>();
        foreach (var dateTimeValue in dateTimeValues)
        {
            TimeZoneInfo ukTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
            DateTime utcTime = TimeZoneInfo.ConvertTimeToUtc(dateTimeValue, ukTimeZone);
            if (utcTime >= _clock.UtcNow)
            {
                var semanticValidationError = new SemanticValidationError
                {
                    Name = $"Invalid '{dateTimeValue}'", 
                    Message = $"Date-time values passed in the record should not be equal or greater than GMT Standard Time", 
                    Path = "Check all date-time values passed in the D-TRO payload", 
                    Rule = $"Passed in date-time '{dateTimeValue}' cannot be equal or greater than GMT Standard Time: '{_clock.UtcNow.DateTime}'"
                };
                error.RequestComparedToRules.Add(semanticValidationError);
            }
        }

        return error;
    }
}