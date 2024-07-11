using System.Runtime.Serialization;

namespace DfT.DTRO.Models.Metrics;

/// <summary>
/// Response dto of the metric data.
/// </summary>

[DataContract]
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
    [DataMember(Name = "eventCount")]
    public int EventCount { get; set; }
}