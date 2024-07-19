namespace DfT.DTRO.Models.Metrics;

[DataContract]
public class MetricSummary
{
    [DataMember(Name = "systemFailureCount")]
    public int SystemFailureCount { get; set; }

    [DataMember(Name = "submissionFailureCount")]
    public int SubmissionFailureCount { get; set; }

    [DataMember(Name = "submissionCount")]
    public int SubmissionCount { get; set; }

    [DataMember(Name = "deletionCount")]
    public int DeletionCount { get; set; }

    [DataMember(Name = "searchCount")]
    public int SearchCount { get; set; }

    [DataMember(Name = "eventCount")]
    public int EventCount { get; set; }
}