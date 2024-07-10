using System.Runtime.Serialization;

public class MetricSummary
{
    /// <summary>
    /// Gets or sets system Failure Count Metric.
    /// </summary>
    [DataMember(Name = "systemFailureCount")]
    public int SystemFailureCount { get; set; } = 0;

    /// <summary>
    /// Gets or sets Submission Failure Count Metric.
    /// </summary>
    [DataMember(Name = "submissionFailureCount")]
    public int SubmissionFailureCount { get; set; } = 0;

    /// <summary>
    /// Gets or sets Submission Count Metric.
    /// </summary>
    [DataMember(Name = "submissionCount")]
    public int SubmissionCount { get; set; } = 0;

    /// <summary>
    /// Gets or sets Deletion Count Metric.
    /// </summary>
    [DataMember(Name = "deletionCount")]
    public int DeletionCount { get; set; } = 0;

    /// <summary>
    /// Gets or sets Search Count Metic.
    /// </summary>
    [DataMember(Name = "searchCount")]
    public int SearchCount { get; set; } = 0;

    /// <summary>
    /// Gets or sets Event Count Metic.
    /// </summary>
    [DataMember(Name = "eventCount")]
    public int EventCount { get; set; } = 0;
}



