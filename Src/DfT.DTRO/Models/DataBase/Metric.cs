namespace DfT.DTRO.Models.DataBase;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using DfT.DTRO.Attributes;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

/// <summary>
/// Wrapper for a Metric.
/// </summary>
[DataContract]
public class Metric
{
    /// <summary>
    /// Gets or sets id of the Metric.
    /// </summary>
    [Key]
    [SwaggerSchema(ReadOnly = true)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(TypeName = "uuid")]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the Traffic Authority ID of the request.
    /// </summary>
    /// <example>1585.</example>
    [Required(ErrorMessage = "TraId field must be included")]
    [DataMember(Name = "traId")]
    public int TraId { get; set; }

    /// <summary>
    /// Gets or sets the date that metrics were generated.
    /// </summary>
    [Required(ErrorMessage = "ForDate field must be included")]
    [DataMember(Name = "forDate")]
    public DateOnly ForDate { get; set; }

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

    /// <summary>
    /// Gets or sets correlation ID of the request with which this DTRO was created.
    /// </summary>
    [DataMember(Name = "createdCorrelationId")]
    [SaveOnce]
    [SwaggerSchema(ReadOnly = true)]
    public string CreatedCorrelationId { get; set; }

    /// <summary>
    /// Gets or sets correlation ID of the request with which this DTRO was last updated.
    /// </summary>
    [DataMember(Name = "lastUpdatedCorrelationId")]
    [SwaggerSchema(ReadOnly = true)]
    public string LastUpdatedCorrelationId { get; set; }
}