using DfT.DTRO.DAL;
using DfT.DTRO.Enums;
using DfT.DTRO.Models.Metrics;
using System;
using System.Threading.Tasks;

namespace DfT.DTRO.Services;

/// <summary>
/// An implementation of <see cref="IMetricsService"/>
/// </summary>
public class MetricsService : IMetricsService
{
    private readonly IMetricDal _metricDal;

    /// <summary>
    /// The default constructor.
    /// </summary>
    /// <param name="metricDal">An <see cref="IMetricDal"/> instance.</param>
    /// 
    public MetricsService(IMetricDal metricDal)
    {
        _metricDal = metricDal;
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public async Task<MetricSummary> GetMetricsForTra(int traId, DateOnly fromDate, DateOnly toDate)
    {
        return await _metricDal.GetMetricsForTra(traId, fromDate, toDate);
    }

    /// <inheritdoc/>
    public async Task<bool> CheckDataBase()
    {
        return await _metricDal.HasValidConnectionAsync();
    }
}