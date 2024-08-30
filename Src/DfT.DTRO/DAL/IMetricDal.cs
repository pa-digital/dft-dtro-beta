using DfT.DTRO.Enums;

namespace DfT.DTRO.DAL;

/// <summary>
/// Service interface providing methods that support metrics capture.
/// </summary>
public interface IMetricDal
{
    /// <summary>
    /// Gets metrics for dtro User  having <paramref name="dtroUserId"/>.
    /// </summary>
    /// <param name="dtroUserId"> dtro User Id.</param>
    /// <param name="fromDate">Time stamp representing start date and time of metric being captured.</param>
    /// <param name="toDate">Time stamp representing end date and time of metric being captured.</param>
    /// <param name="userGroup">User Group.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous get operation.</returns>
    Task<MetricSummary> GetMetricsForDtroUser(Guid? dtroUserId, DateOnly fromDate, DateOnly toDate, UserGroup userGroup);

    /// <summary>
    /// Gets full metrics for dtro User  having <paramref name="dtroUserId"/>.
    /// </summary>
    /// <param name="dtroUserId"> dtro User Id.</param>
    /// <param name="fromDate">Time stamp representing start date and time of metric being captured.</param>
    /// <param name="toDate">Time stamp representing end date and time of metric being captured.</param>
    /// <param name="userGroup">User Group.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous get operation.</returns>
    Task<List<FullMetricSummary>> GetFullMetricsForDtroUser(Guid? dtroUserId, DateOnly fromDate, DateOnly toDate, UserGroup userGroup);

    /// <summary>
    /// Increment metric for dtro User having <paramref name="dtroUserId"/>.
    /// </summary>
    /// <param name="type">Type of the metric being incremented.</param>
    /// <param name="dtroUserId">dtro User ID.</param>
    /// <param name="userGroup">User Group.</param>
    /// <returns>
    /// A <see cref="Task"/> whose result is <see langword="true" />
    /// if a metric increment has been successful. Otherwise <see langword="false" />
    /// </returns>
    Task<bool> IncrementMetric(MetricType type, Guid dtroUserId, UserGroup userGroup);

    /// <summary>
    /// Check if there is a valid connection.
    /// </summary>
    /// <returns>
    /// A <see cref="Task"/> whose result is <see langword="true" />
    /// if a there is a valid connection. Otherwise <see langword="false" />
    /// </returns>
    Task<bool> HasValidConnectionAsync();

    /// <summary>
    /// Change the user group for Metrics having <paramref name="dtroUserId"/>.
    /// </summary>
    /// <param name="dtroUserId">dtro User ID.</param>
    /// <param name="userGroup">User Group.</param>
    Task UpdateUserGroupForMetricsAsync(Guid dtroUserId, UserGroup userGroup);
}