using DfT.DTRO.Enums;

namespace DfT.DTRO.Services;

public class MetricsService : IMetricsService
{
    private readonly IMetricDal _metricDal;

    public MetricsService(IMetricDal metricDal)
    {
        _metricDal = metricDal;
    }

    public async Task<bool> IncrementMetric(MetricType type, int? traId)
    {
        try
        {
            if (traId == null)
            {
                return false;
            }
            return await _metricDal.IncrementMetric(type, (int)traId);
        }
        catch (Exception)
        {
            return false;
        }

    }

    public async Task<MetricSummary> GetMetrics(MetricRequest metricRequest)
    {
        if (metricRequest.DateFrom > metricRequest.DateTo)
        {
            throw new ArgumentException("Start date must be before end date.");
        }

        return await _metricDal.GetMetricsForTra(metricRequest.TraId,
            new DateOnly(metricRequest.DateFrom.Year, metricRequest.DateFrom.Month, metricRequest.DateFrom.Day),
            new DateOnly(metricRequest.DateTo.Year, metricRequest.DateTo.Month, metricRequest.DateTo.Day));
    }

    public async Task<bool> CheckDataBase()
    {
        return await _metricDal.HasValidConnectionAsync();
    }
}