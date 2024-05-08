namespace DfT.DTRO.RequestCorrelation;

/// <summary>
/// Contains configuration data related to request correlation.
/// </summary>
public class RequestCorrelationOptions
{
    /// <summary>
    /// The name of the HTTP header to be used as correlation ID.
    /// </summary>
    public string HeaderName { get; init; } = "X-Correlation-ID";

    /// <summary>
    /// The name to use for the property.
    /// </summary>
    public string LogPropertyName { get; init; } = "CorrelationId";
}
