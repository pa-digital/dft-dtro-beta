namespace DfT.DTRO.Models.DataBase;

/// <summary>
/// Wrapper for ApplicationStatus
/// </summary>
[DataContract]
public class ApplicationStatus
{
    /// <summary>
    /// Application unique identifier.
    /// </summary>
    [Key]
    [SwaggerSchema(ReadOnly = true)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(TypeName = "uuid")]
    public Guid Id { get; set; }
    public string Status { get; set; }
}