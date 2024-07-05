using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dft.DTRO.Admin.Pages;
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public MetricSummary Metrics { get; set; }
    public bool HealthApi { get; set; } = true;

    public bool TraIdMatch { get; set; } = false;

    public bool HealthDatabase { get; set; } = true;

    public void OnGet()
    {
        Metrics = new MetricSummary
        {
            SystemFailureCount = 0,
            SubmissionFailureCount = 0,
            SubmissionCount = 0,
            DeletionCount = 0,
            SearchCount = 0,
            EventCount = 0
        };
    }
}

public class MetricSummary
{
    /// <summary>
    /// Gets or sets system Failure Count Metric.
    /// </summary>
    [DataMember(Name = "systemFailureCount")]
    public int SystemFailureCount { get; set; }

    /// <summary>
    /// Gets or sets Submission Failure Count Metric.
    /// </summary>
    [DataMember(Name = "submissionFailureCount")]
    public int SubmissionFailureCount { get; set; }

    /// <summary>
    /// Gets or sets Submission Count Metric.
    /// </summary>
    [DataMember(Name = "submissionCount")]
    public int SubmissionCount { get; set; }

    /// <summary>
    /// Gets or sets Deletion Count Metric.
    /// </summary>
    [DataMember(Name = "deletionCount")]
    public int DeletionCount { get; set; }

    /// <summary>
    /// Gets or sets Search Count Metic.
    /// </summary>
    [DataMember(Name = "searchCount")]
    public int SearchCount { get; set; }

    /// <summary>
    /// Gets or sets Event Count Metic.
    /// </summary>
    [DataMember(Name = "eventCount ")]
    public int EventCount { get; set; }
}