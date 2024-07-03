using System;
using System.Threading.Tasks;
using DfT.DTRO.Enums;
using DfT.DTRO.Models.Metrics;

namespace DfT.DTRO.DAL;
/// <summary>
/// Service layer implementation for storage.
/// </summary>
/// 
public interface IMetricDal
{
    /// <summary>
    /// return a metric for the dates.
    /// </summary>
    /// <param name="traId"></param>
    /// <param name="fromDate"></param>
    /// <param name="toDate"></param>
    Task<MetricSummary> GetMetricsForTra(int traId, DateOnly fromDate, DateOnly toDate);

    /// <summary>
    /// Incremens a metric of type <paramref name="type"/>.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="traId"></param>
    /// <returns>A <see cref="Task"/> representing the asynchronous increment operation.</returns>
    Task<bool> IncrementMetric(MetricType type, int traId);

    /// <summary>
    /// Checks the databse connection and returns a <see cref="bool"/>.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous check operation.</returns>
    Task<bool> HasValidConnectionAsync();
}