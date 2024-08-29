namespace Dft.DTRO.Admin.Models.Metrics;

public class FullMetricSummary
{
    [DataMember(Name = "forDate")]
    public string ForDate { get; set; }

    [DataMember(Name = "userName")]
    public string UserName { get; set; } = string.Empty;

    [DataMember(Name = "systemFailureCount")]
    public int SystemFailureCount { get; set; } = 0;

    [DataMember(Name = "submissionFailureCount")]
    public int SubmissionFailureCount { get; set; } = 0;

    [DataMember(Name = "submissionCount")]
    public int SubmissionCount { get; set; } = 0;

    [DataMember(Name = "deletionCount")]
    public int DeletionCount { get; set; } = 0;

    [DataMember(Name = "searchCount")]
    public int SearchCount { get; set; } = 0;

    [DataMember(Name = "eventCount")]
    public int EventCount { get; set; } = 0;
}



