namespace DfT.DTRO.RequestCorrelation;

public class NoOpRequestCorrelationProvider : IRequestCorrelationProvider
{
    public string CorrelationId => null;

    public static readonly NoOpRequestCorrelationProvider Instance = new();
}
