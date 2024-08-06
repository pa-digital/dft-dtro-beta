namespace DfT.DTRO.Models.Metrics;

/// <summary>
/// Metric summery data.
/// </summary>
[DataContract]
public class MetricSummary
{
    /// <summary>
    /// Count of how many system failure are recorded.
    /// </summary>
    [DataMember(Name = "systemFailureCount")]
    public int SystemFailureCount { get; set; }

    /// <summary>
    /// Count of how many submission failure are recorded.
    /// </summary>
    [DataMember(Name = "submissionFailureCount")]
    public int SubmissionFailureCount { get; set; }

    /// <summary>
    /// Count of how many submission are recorded.
    /// </summary>
    [DataMember(Name = "submissionCount")]
    public int SubmissionCount { get; set; }

    /// <summary>
    /// Count of how many deletion are recorded.
    /// </summary>
    [DataMember(Name = "deletionCount")]
    public int DeletionCount { get; set; }

    /// <summary>
    /// Count of how many searches are recorded.
    /// </summary>
    [DataMember(Name = "searchCount")]
    public int SearchCount { get; set; }

    /// <summary>
    /// Count of how many event are recorded.
    /// </summary>
    [DataMember(Name = "eventCount")]
    public int EventCount { get; set; }
}