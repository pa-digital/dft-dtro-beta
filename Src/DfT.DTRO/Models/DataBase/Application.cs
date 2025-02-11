namespace DfT.DTRO.Models.DataBase;

/// <summary>
/// Wrapper for Application
/// </summary>
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
    public DateTime Created { get; set; }

    /// <summary>
    /// Application last updated date
    /// </summary>
    public DateTime? LastUpdated { get; set; }

    /// <summary>
    /// Application nickname
    /// </summary>
    public string Nickname { get; set; }

    /// <summary>
    /// Application status
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// Application types
    /// </summary>
    public List<ApplicationType> ApplicationTypes { get; set; }

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
}