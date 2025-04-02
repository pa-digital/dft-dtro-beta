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
            if (dateTimeValue.IsDaylightSavingTime())
            {
                if (dateTimeValue.AddHours(-1) < _clock.UtcNow.DateTime)
                {
                    continue;
                }
                else
                {
                    var semanticValidationError = new SemanticValidationError
                    {
                        Name = $"Invalid '{dateTimeValue}'", 
                        Message = "One or more date-time values passed are in the future", 
                        Path = "", 
                        Rule = ""
                    };
                    error.RequestComparedToRules.Add(semanticValidationError);
                }
            }
            else
            {
                if (dateTimeValue.AddHours(-1) < _clock.UtcNow.DateTime)
                {
                    continue;
                }
                else
                {
                    var semanticValidationError = new SemanticValidationError
                    {
                        Name = $"Invalid '{dateTimeValue}'", 
                        Message = "One or more date-time values passed are in the future", 
                        Path = "", 
                        Rule = ""
                    };
                    error.RequestComparedToRules.Add(semanticValidationError);
                }
            }
        }

        return error;
    }
}