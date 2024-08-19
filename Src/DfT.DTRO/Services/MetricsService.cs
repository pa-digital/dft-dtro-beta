using DfT.DTRO.Enums;

namespace DfT.DTRO.Services;

public class MetricsService : IMetricsService
{
    private readonly IMetricDal _metricDal;
    private readonly IDtroUserDal _dtroUserDal;

    public MetricsService(IMetricDal metricDal, IDtroUserDal dtroUserDal)
    {
        _metricDal = metricDal;
        _dtroUserDal = dtroUserDal;
    }

    public async Task<bool> IncrementMetric(MetricType type, Guid xAppId)
    {
            var dtroUser = await _dtroUserDal.GetDtroUserOnAppIdAsync(xAppId);
            var result = await _metricDal.IncrementMetric(type, dtroUser.Id);
            return result;
    }

    public async Task<MetricSummary> GetMetrics(MetricRequest metricRequest)
    {
        if (metricRequest.DateFrom > metricRequest.DateTo)
        {
            throw new ArgumentException("Start date must be before end date.");
        }


        return await _metricDal.GetMetricsForDtroUser(metricRequest.DtroUserId,
            new DateOnly(metricRequest.DateFrom.Year, metricRequest.DateFrom.Month, metricRequest.DateFrom.Day),
            new DateOnly(metricRequest.DateTo.Year, metricRequest.DateTo.Month, metricRequest.DateTo.Day));
    }

    public async Task<bool> CheckDataBase()
    {
        return await _metricDal.HasValidConnectionAsync();
    }
}