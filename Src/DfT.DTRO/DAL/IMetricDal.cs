namespace DfT.DTRO.DAL;

/// <summary>
/// Service interface providing methods that support metrics capture.
/// </summary>
public interface IMetricDal
{
    /// <summary>
    /// Gets metrics for traffic regulation authority having <paramref name="traId"/>.
    /// </summary>
    /// <param name="traId">Traffic regulation authority ID.</param>
    /// <param name="fromDate">Time stamp representing start date and time of metric being captured.</param>
    /// <param name="toDate">Time stamp representing end date and time of metric being captured.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous get operation.</returns>
    Task<MetricSummary> GetMetricsForTra(int? traId, DateOnly fromDate, DateOnly toDate);

    /// <summary>
    /// Increment metric for traffic regulation authority having <paramref name="traId"/>.
    /// </summary>
    /// <param name="type">Type of the metric being incremented.</param>
    /// <param name="traId">Traffic regulation authority ID.</param>
    /// <returns>
    /// A <see cref="Task"/> whose result is <see langword="true" />
    /// if a metric increment has been successful. Otherwise <see langword="false" />
    /// </returns>
    Task<bool> IncrementMetric(MetricType type, int traId);

    /// <summary>
    /// Check if there is a valid connection.
    /// </summary>
    /// <returns>
    /// A <see cref="Task"/> whose result is <see langword="true" />
    /// if a there is a valid connection. Otherwise <see langword="false" />
    /// </returns>
    Task<bool> HasValidConnectionAsync();
}