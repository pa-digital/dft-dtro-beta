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
    public async Task<bool> IncrementMetric(MetricType type, int traId)
    {
        var today = new DateOnly(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day);
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
    public async Task<MetricSummary> GetMetricsForTra(int? traId, DateOnly fromDate, DateOnly toDate)
    {
        if (traId == null)
        {
            var aggregatedMetrics = await _dtroContext.Metrics
                          .Where(metric => metric.ForDate >= fromDate && metric.ForDate <= toDate)
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
        else
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