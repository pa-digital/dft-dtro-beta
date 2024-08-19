using DfT.DTRO.Enums;

namespace DfT.DTRO.Services;

public interface IMetricsService
{
    Task<bool> CheckDataBase();

    Task<MetricSummary> GetMetrics(MetricRequest metricRequest);

    Task<bool> IncrementMetric(MetricType type, Guid xAppId);
}