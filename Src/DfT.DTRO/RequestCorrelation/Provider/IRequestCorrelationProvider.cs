namespace DfT.DTRO.RequestCorrelation;

/// <summary>
/// Provides access to correlation information for current request.
/// </summary>
public interface IRequestCorrelationProvider
{
    /// <summary>
    /// The correlation ID associated with current HTTP request
    /// or <see langword="null"/> if there is none.
    /// </summary>
    string CorrelationId { get; }
}
