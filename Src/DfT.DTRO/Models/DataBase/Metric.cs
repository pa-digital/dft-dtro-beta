namespace DfT.DTRO.Models.DataBase;

/// <summary>
/// Wrapper for Metric
/// </summary>
[DataContract]
public class Metric
{
    /// <summary>
    /// ID of the metric
    /// </summary>
    [Key]
    [SwaggerSchema(ReadOnly = true)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(TypeName = "uuid")]
    public Guid Id { get; set; }

    /// <summary>
    /// Dtro User Id of the metric
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(TypeName = "uuid")]
    public Guid DtroUserId { get; set; }

    /// <summary>
    /// Timestamp representing the date of this metric.
    /// </summary>
    [Required(ErrorMessage = "ForDate field must be included")]
    [DataMember(Name = "forDate")]
    public DateOnly ForDate { get; set; }

    /// <summary>
    /// Count of system failure metric.
    /// </summary>
    [DataMember(Name = "systemFailureCount")]
    public int SystemFailureCount { get; set; }

    /// <summary>
    /// Count of submission failure metric.
    /// </summary>
    [DataMember(Name = "submissionFailureCount")]
    public int SubmissionFailureCount { get; set; }

    /// <summary>
    /// Count of submission metric.
    /// </summary>
    [DataMember(Name = "submissionCount")]
    public int SubmissionCount { get; set; }

    /// <summary>
    /// Count of deletion metric.
    /// </summary>
    [DataMember(Name = "deletionCount")]
    public int DeletionCount { get; set; }

    /// <summary>
    /// Count of search metric.
    /// </summary>
    [DataMember(Name = "searchCount")]
    public int SearchCount { get; set; }

    /// <summary>
    /// Count of event metric.
    /// </summary>
    [DataMember(Name = "eventCount ")]
    public int EventCount { get; set; }

    /// <summary>
    /// Correlation ID of the request with which this metric document was created.
    /// </summary>
    [DataMember(Name = "createdCorrelationId")]
    [SaveOnce]
    [SwaggerSchema(ReadOnly = true)]
    public string CreatedCorrelationId { get; set; }

    /// <summary>
    /// Correlation ID of the request witch which this metric document was updated.
    /// </summary>
    [DataMember(Name = "lastUpdatedCorrelationId")]
    [SwaggerSchema(ReadOnly = true)]
    public string LastUpdatedCorrelationId { get; set; }

    /// <summary>
    /// The user group
    /// </summary>
    [DataMember(Name = "userGroup")]
    public int UserGroup { get; set; }
}