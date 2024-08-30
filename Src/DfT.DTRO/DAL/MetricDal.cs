using DfT.DTRO.Enums;
using DfT.DTRO.Models.DataBase;

namespace DfT.DTRO.DAL;

/// <summary>
/// Implementation of <see cref="IMetricDal"/> service.
/// </summary>
[ExcludeFromCodeCoverage]
public class MetricDal : IMetricDal
{
    private readonly DtroContext _dtroContext;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="dtroContext"><see cref="DtroContext"/> database context.</param>
    public MetricDal(DtroContext dtroContext)
    {
        _dtroContext = dtroContext;
    }

    ///<inheritdoc cref="IMetricDal"/>
    public async Task<bool> IncrementMetric(MetricType type, Guid dtroUserId, UserGroup userGroup)
    {
        var today = new DateOnly(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day);
        var metric = await _dtroContext.Metrics.FirstOrDefaultAsync(x => x.ForDate == today && x.DtroUserId == dtroUserId);
        if (metric == null)
        {
            metric = new Metric
            {
                Id = Guid.NewGuid(),
                ForDate = today,
                DtroUserId = dtroUserId,
                UserGroup = (int) userGroup
            };
            await _dtroContext.Metrics.AddAsync(metric);
        }

        switch (type)
        {
            case MetricType.SystemFailure:
                metric.SystemFailureCount++;
                break;
            case MetricType.SubmissionValidationFailure:
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
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
        await _dtroContext.SaveChangesAsync();
        return true;
    }

    ///<inheritdoc cref="IMetricDal"/>
    public async Task UpdateUserGroupForMetricsAsync(Guid dtroUserId, UserGroup newUserGroup)
    {
        // Validate that the dtroUserId is not empty
        if (dtroUserId == Guid.Empty)
        {
            throw new ArgumentException("dtroUserId cannot be empty.", nameof(dtroUserId));
        }

        // Get all metrics with the specified dtroUserId
        var metricsToUpdate = await _dtroContext.Metrics
            .Where(metric => metric.DtroUserId == dtroUserId)
            .ToListAsync();

        foreach (var metric in metricsToUpdate)
        {
            metric.UserGroup = (int)newUserGroup;
        }

        await _dtroContext.SaveChangesAsync();
    }

    ///<inheritdoc cref="IMetricDal"/>
    public async Task<MetricSummary> GetMetricsForDtroUser(Guid? dtroUserId, DateOnly fromDate, DateOnly toDate, UserGroup userGroup)
    {
        var query = _dtroContext.Metrics.Where(metric => metric.ForDate >= fromDate && metric.ForDate <= toDate);

        if (dtroUserId.HasValue && dtroUserId != Guid.Empty)
        {
            query = query.Where(metric => metric.DtroUserId == dtroUserId.Value);
        }

        if (userGroup != UserGroup.All)
        {
            query = query.Where(metric => metric.UserGroup == (int)userGroup);
        }

        var aggregatedMetrics = await query
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

    ///<inheritdoc cref="IMetricDal"/>
    public async Task<List<FullMetricSummary>> GetFullMetricsForDtroUser(Guid? dtroUserId, DateOnly fromDate, DateOnly toDate, UserGroup userGroup)
    {

        // Create the query with initial filter
        var query = from metric in _dtroContext.Metrics
                    join user in _dtroContext.DtroUsers
                    on metric.DtroUserId equals user.Id
                    where metric.ForDate >= fromDate && metric.ForDate <= toDate
                    select new
                    {
                        metric.ForDate,
                        metric.SystemFailureCount,
                        metric.SubmissionFailureCount,
                        metric.SubmissionCount,
                        metric.DeletionCount,
                        metric.SearchCount,
                        metric.EventCount,
                        user.Name,
                        metric.DtroUserId,
                        metric.UserGroup
                    };

        // Apply optional filters
        if (dtroUserId.HasValue && dtroUserId != Guid.Empty)
        {
            query = query.Where(x => x.DtroUserId == dtroUserId.Value);
        }

        if (userGroup != UserGroup.All)
        {
            query = query.Where(x => x.UserGroup == (int)userGroup);
        }

        // Execute the query and project results into FullMetricSummary
        var fullMetrics = await query
            .Select(x => new FullMetricSummary
            {
                ForDate = x.ForDate.ToShortDateString(),
                SystemFailureCount = x.SystemFailureCount,
                SubmissionFailureCount = x.SubmissionFailureCount,
                SubmissionCount = x.SubmissionCount,
                DeletionCount = x.DeletionCount,
                SearchCount = x.SearchCount,
                EventCount = x.EventCount,
                UserName = x.Name
            })
            .ToListAsync();

        return fullMetrics;
    }


    ///<inheritdoc cref="IMetricDal"/>
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