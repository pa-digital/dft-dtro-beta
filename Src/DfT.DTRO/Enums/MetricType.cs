namespace DfT.DTRO.Enums;

/// <summary>
/// Metric statements.
/// </summary>
public enum MetricType
{
    /// <summary>
    /// System failure metric.
    /// </summary>
    [Display(Name = "systemFailure")]
    SystemFailure,

    /// <summary>
    /// Submission validation failure metric.
    /// </summary>
    [Display(Name = "submissionValidationFailure")]
    SubmissionValidationFailure,

    /// <summary>
    /// Submission metric.
    /// </summary>
    [Display(Name = "submission")]
    Submission,

    /// <summary>
    /// Deletion metric.
    /// </summary>
    [Display(Name = "deletion")]
    Deletion,

    /// <summary>
    /// Search metric.
    /// </summary>
    [Display(Name = "search")]
    Search,

    /// <summary>
    /// Event metric.
    /// </summary>
    [Display(Name = "event")]
    Event
}