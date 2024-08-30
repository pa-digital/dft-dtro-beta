
namespace Dft.DTRO.Admin.Services;
public interface IMetricsService
{
    Task<bool> HealthDatabase();
    Task<bool> HealthApi();
    Task<MetricSummary> MetricsForDtroUser(MetricRequest metricRequest);
    Task<List<FullMetricSummary>> FullMetricsForDtroUser(MetricRequest metricRequest);
}