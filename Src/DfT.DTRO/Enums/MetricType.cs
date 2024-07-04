/// <summary>
/// Represents the type of metric.
/// </summary>
public enum MetricType
{
    /// <summary>
    /// Indicates a system failure metric.
    /// </summary>
    SystemFailure,

    /// <summary>
    /// Indicates a submission failure metric.
    /// </summary>
    SubmissionValidationFailure,

    /// <summary>
    /// Indicates a submission metric.
    /// </summary>
    Submission,

    /// <summary>
    /// Indicates a deletion metric.
    /// </summary>
    Deletion,

    /// <summary>
    /// Indicates a search metric.
    /// </summary>
    Search,

    /// <summary>
    /// Indicates an event metric.
    /// </summary>
    Event
}