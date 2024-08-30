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
            var result = await _metricDal.IncrementMetric(type, dtroUser.Id,(UserGroup) dtroUser.UserGroup);
            return result;
    }

    public async Task<MetricSummary> GetMetrics(MetricRequest metricRequest)
    {
        if (metricRequest.DateFrom > metricRequest.DateTo)
        {
            throw new ArgumentException("Start date must be before end date.");
        }

        var dateFrom = new DateOnly(metricRequest.DateFrom.Year, metricRequest.DateFrom.Month, metricRequest.DateFrom.Day);
        var dateTo = new DateOnly(metricRequest.DateTo.Year, metricRequest.DateTo.Month, metricRequest.DateTo.Day);
        return await _metricDal.GetMetricsForDtroUser(metricRequest.DtroUserId,
            dateFrom,
            dateTo,
            metricRequest.UserGroup );
    }

    public async Task<List<FullMetricSummary>> GetFullMetrics(MetricRequest metricRequest)
    {
        if (metricRequest.DateFrom > metricRequest.DateTo)
        {
            throw new ArgumentException("Start date must be before end date.");
        }

        var dateFrom = new DateOnly(metricRequest.DateFrom.Year, metricRequest.DateFrom.Month, metricRequest.DateFrom.Day);
        var dateTo = new DateOnly(metricRequest.DateTo.Year, metricRequest.DateTo.Month, metricRequest.DateTo.Day);
        return await _metricDal.GetFullMetricsForDtroUser(metricRequest.DtroUserId,
            dateFrom,
            dateTo,
            metricRequest.UserGroup);
    }

    public async Task<bool> CheckDataBase()
    {
        return await _metricDal.HasValidConnectionAsync();
    }
}