namespace DfT.DTRO.Services.Validation.Implementation;

/// <inheritdoc cref="IDtroTimeZoneValidatorService"/>
public class DtroTimeZoneValidatorService : IDtroTimeZoneValidatorService
{
    private SystemClock _clock = new();

    /// <inheritdoc cref="IDtroTimeZoneValidatorService"/>
    public async Task<DtroValidationException> ValidateDtro(DtroSubmit dtroSubmit)
    {
        var expandoObject = dtroSubmit.Data.GetExpando(Constants.Source);

        var dateTimeValues = new List<DateTime>();
        await FindAllDateTimeValues(expandoObject, dateTimeValues);

        var error = new DtroValidationException();
        error.RequestComparedToRules = new List<SemanticValidationError>();
        foreach (var dateTimeValue in dateTimeValues)
        {
            if (dateTimeValue >= _clock.UtcNow.AddHours(-1).DateTime)
            {
                error.RequestComparedToRules.Add(
                    new SemanticValidationError()
                    {
                        Name = $"Invalid '{dateTimeValue}'", 
                        //TODO: To be updated
                        Message = "", 
                        //TODO: To be updated
                        Path = "", 
                        //TODO: To be updated
                        Rule = ""
                    });
            }
        }

        return error;
    }

    /// <summary>
    /// Recursive method to find all string values formatted as date-time
    /// </summary>
    /// <param name="expandoObject">Expando object to check against.</param>
    /// <param name="dateTimeValues">List of date-time values found.</param>
    public async Task FindAllDateTimeValues(ExpandoObject expandoObject, List<DateTime> dateTimeValues)
    {
        foreach (var kvp in expandoObject)
        {
            if (kvp.Value is DateTime value)
                dateTimeValues.Add(value);
            else if (kvp.Value is ExpandoObject nestedExpandoObject)
                await FindAllDateTimeValues(nestedExpandoObject, dateTimeValues);
            else if (kvp.Value is IEnumerable<object> enumerable)
                foreach (var item in enumerable)
                    if (item is ExpandoObject nestedItemExpandoObject)
                        await FindAllDateTimeValues(nestedItemExpandoObject, dateTimeValues);
                    else if (item is DateTime nestedItemValue) 
                        dateTimeValues.Add(nestedItemValue);
        }
    }
}