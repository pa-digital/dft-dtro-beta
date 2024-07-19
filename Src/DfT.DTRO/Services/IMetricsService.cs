using DfT.DTRO.Models.Metrics;

namespace DfT.DTRO.Services;

public interface IMetricsService
{
    Task<bool> CheckDataBase();

    Task<MetricSummary> GetMetrics(MetricRequest metricRequest);

    Task<bool> IncrementMetric(MetricType type, int? traId);
}