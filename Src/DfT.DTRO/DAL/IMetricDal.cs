using System;
using System.Threading.Tasks;
using DfT.DTRO.Models.Metrics;

namespace DfT.DTRO.DAL;
public interface IMetricDal
{
    Task<MetricSummary> GetMetricsForTra(int? traId, DateOnly fromDate, DateOnly toDate);

    Task<bool> IncrementMetric(MetricType type, int traId);

    Task<bool> HasValidConnectionAsync();
}