
namespace Dft.DTRO.Admin.Services;

public interface IMetricsService
{
    Task<bool> HealthDatabase();
    Task<bool> TraIdMatch();
    Task<bool> HealthApi();
    Task<MetricSummary> MetricsForTra(MetricRequest metricRequest);
}