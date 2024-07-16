namespace DfT.DTRO.RequestCorrelation;

public interface IRequestCorrelationProvider
{
    string CorrelationId { get; }
}
