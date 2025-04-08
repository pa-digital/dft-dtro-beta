namespace DfT.DTRO.Models.DataBase;

/// <summary>
/// Wrapper for Digital Service Provider
/// </summary>
[ExcludeFromCodeCoverage]
[DataContract]
public class DigitalServiceProvider
{
    /// <summary>
    /// Digital Service Provider unique identifier.
    /// </summary>
    [Key]
    [SwaggerSchema(ReadOnly = true)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(TypeName = "uuid")]
    public Guid Id { get; set; }

    /// <summary>
    /// Digital Service Provider name
    /// </summary>
    public string Name { get; set; }
}