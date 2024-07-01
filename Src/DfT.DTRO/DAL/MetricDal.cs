using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using DfT.DTRO.Enums;
using DfT.DTRO.Models.DataBase;
using DfT.DTRO.Models.Metrics;
using Microsoft.EntityFrameworkCore;

namespace DfT.DTRO.DAL;
/// <summary>
/// An implementation of <see cref="IMetricDal"/>
/// that uses an SQL database as its store.
/// </summary>
///

[ExcludeFromCodeCoverage]
public class MetricDal : IMetricDal
{
    private readonly DtroContext _dtroContext;

    /// <summary>
    /// The default constructor.
    /// </summary>
    /// <param name="dtroContext">
    /// An instance of <see cref="DtroContext"/>
    /// representing the current database session.
    /// </param>
    public MetricDal(DtroContext dtroContext)
    {
        _dtroContext = dtroContext;
    }

    /// <summary>
    /// Save metric to metric Table
    /// </summary>
    /// <param name="type"></param>
    /// <param name="traId"></param>
    /// <returns>
    /// A <see cref="Task"/> that resolved to <see langword="true"/>
    /// if the Metric was successfully saved
    /// or <see langword="false"/> if it was not.
    /// </returns>

    public async Task<bool> IncrementMetric(MetricType type, int traId)
    {
        DateOnly today = new DateOnly(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day);
        var metric = await _dtroContext.Metrics.FirstOrDefaultAsync(x => x.ForDate == today && x.TraId == traId);
        if (metric == null)
        {
            metric = new Metric
            {
                Id = Guid.NewGuid(),
                ForDate = today,
                TraId = traId
            };
            await _dtroContext.Metrics.AddAsync(metric);
        }

        switch (type)
        {
            case MetricType.SystemFailure:
                metric.SystemFailureCount++;
                break;
            case MetricType.SubmissionFailure:
                metric.SubmissionFailureCount++;
                break;
            case MetricType.Submission:
                metric.SubmissionCount++;
                break;
            case MetricType.Deletion:
                metric.DeletionCount++;
                break;
            case MetricType.Search:
                metric.SearchCount++;
                break;
            case MetricType.Event:
                metric.EventCount++;
                break;
        }
        await _dtroContext.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Get list of metrics from metric Table for a specific TRA.
    /// </summary>
    /// <returns>List of metrics</returns>
    public async Task<MetricSummary> GetMetricsForTra(int traId, DateOnly fromDate, DateOnly toDate)
    {
        var aggregatedMetrics = await _dtroContext.Metrics
               .Where(metric => metric.TraId == traId && metric.ForDate >= fromDate && metric.ForDate <= toDate)
               .GroupBy(metric => 1)
               .Select(group => new MetricSummary
               {
                   SystemFailureCount = group.Sum(metric => metric.SystemFailureCount),
                   SubmissionFailureCount = group.Sum(metric => metric.SubmissionFailureCount),
                   SubmissionCount = group.Sum(metric => metric.SubmissionCount),
                   DeletionCount = group.Sum(metric => metric.DeletionCount),
                   SearchCount = group.Sum(metric => metric.SearchCount),
                   EventCount = group.Sum(metric => metric.EventCount),
               })
               .FirstOrDefaultAsync();
        return aggregatedMetrics;
    }

    /// <summary>
    /// Checks if the database context has a valid connection.
    /// </summary>
    /// <returns>
    /// A <see cref="Task"/> that resolves to <see langword="true"/>
    /// if the connection is valid, or <see langword="false"/> if it is not.
    /// </returns>
    public async Task<bool> HasValidConnectionAsync()
    {
        try
        {
            return await _dtroContext.Database.CanConnectAsync();
        }
        catch (Exception)
        {
            return false;
        }
    }
}