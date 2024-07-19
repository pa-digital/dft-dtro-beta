using System.ComponentModel.DataAnnotations.Schema;

namespace DfT.DTRO.Models.DataBase;

[DataContract]
public class Metric
{
    [Key]
    [SwaggerSchema(ReadOnly = true)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(TypeName = "uuid")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "TraId field must be included")]
    [DataMember(Name = "traId")]
    public int TraId { get; set; }

    [Required(ErrorMessage = "ForDate field must be included")]
    [DataMember(Name = "forDate")]
    public DateOnly ForDate { get; set; }

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

    [DataMember(Name = "eventCount ")]
    public int EventCount { get; set; }

    [DataMember(Name = "createdCorrelationId")]
    [SaveOnce]
    [SwaggerSchema(ReadOnly = true)]
    public string CreatedCorrelationId { get; set; }

    [DataMember(Name = "lastUpdatedCorrelationId")]
    [SwaggerSchema(ReadOnly = true)]
    public string LastUpdatedCorrelationId { get; set; }
}