namespace DfT.DTRO.RequestCorrelation;

/// <summary>
/// A mock implementation of <see cref="IRequestCorrelationProvider"/>.
/// Provides empty correlation information.
/// </summary>
public class NoOpRequestCorrelationProvider : IRequestCorrelationProvider
{
    /// <summary>
    /// Always <see langword="null"/>.
    /// </summary>
    public string CorrelationId => null;

    /// <summary>
    /// A static instance of this class.
    /// </summary>
    public static readonly NoOpRequestCorrelationProvider Instance = new ();
}
