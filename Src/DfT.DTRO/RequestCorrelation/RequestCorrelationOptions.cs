namespace DfT.DTRO.RequestCorrelation;

public class RequestCorrelationOptions
{
    public string HeaderName { get; init; } = "X-Correlation-ID";

    public string LogPropertyName { get; init; } = "CorrelationId";
}
