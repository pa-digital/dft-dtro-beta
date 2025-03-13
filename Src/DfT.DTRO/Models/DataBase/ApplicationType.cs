namespace DfT.DTRO.Models.DataBase;

/// <summary>
/// Wrapper for application type
/// </summary>
[ExcludeFromCodeCoverage]
[DataContract]
public class ApplicationType
{
    /// <summary>
    /// Application Type unique identifier.
    /// </summary>
    [Key]
    [SwaggerSchema(ReadOnly = true)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(TypeName = "uuid")]
    public Guid Id { get; set; }

    /// <summary>
    /// Application Type name
    /// </summary>
    public string Name { get; set; }
}