using System;
using System.Threading.Tasks;
using DfT.DTRO.Enums;
using DfT.DTRO.Models.Metrics;

namespace DfT.DTRO.Services;

/// <summary>
/// Service layer implementation for storage.
/// </summary>
public interface IMetricsService
{
    /// <summary>
    /// Checks the databse connection and returns a <see cref="bool"/>.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous check operation.</returns>
    Task<bool> CheckDataBase();

    /// <summary>
    /// return a metric for the dates and a tra.
    /// </summary>
    /// <param name="metricRequest"></param>
    Task<MetricSummary> GetMetrics(MetricRequest metricRequest);

    /// <summary>
    /// Incremens a metric of type <paramref name="type"/>.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="traId"></param>
    /// <returns>A <see cref="Task"/> representing the asynchronous increment operation.</returns>
    Task<bool> IncrementMetric(MetricType type, int? traId);
}