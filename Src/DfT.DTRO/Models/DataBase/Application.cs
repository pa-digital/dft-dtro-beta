namespace DfT.DTRO.Models.DataBase;
/// <summary>
/// Wrapper for Application
/// </summary>
[ExcludeFromCodeCoverage]
[DataContract]
public class Application
{
    /// <summary>
    /// Application unique identifier.
    /// </summary>
    [Key]
    [SwaggerSchema(ReadOnly = true)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(TypeName = "uuid")]
    public Guid Id { get; set; }

    /// <summary>
    /// Application created date
    /// </summary>
    public DateTime Created { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Application last updated date
    /// </summary>
    public DateTime? LastUpdated { get; set; }

    /// <summary>
    /// Application nickname
    /// </summary>
    public string Nickname { get; set; }

    [ForeignKey("Status")]
    public Guid StatusId { get; set; }

    /// <summary>
    /// Application status
    /// </summary>
    public ApplicationStatus Status { get; set; }

    /// <summary>
    /// Application rule templates
    /// </summary>
    public List<RuleTemplate> RuleTemplates { get; set; }


    /// <summary>
    /// Application schema templates
    /// </summary>
    public List<SchemaTemplate> SchemaTemplates { get; set; }

    /// <summary>
    /// Application D-TROs
    /// </summary>
    public List<DTRO> Dtros { get; set; }

    public string? Purpose { get; set; }
    public string? AdditionalInformation { get; set; }
    public string? Activity { get; set; }
    public string? Regions { get; set; }
    public string? DataType { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    [Column("TrafficRegulationAuthorityId")]
    [ForeignKey("TrafficRegulationAuthority")]
    public Guid TrafficRegulationAuthorityId { get; set; }
    public TrafficRegulationAuthority TrafficRegulationAuthority { get; set; }

    public Guid ApplicationTypeId { get; set; }
    public ApplicationType ApplicationType { get; set; }
}