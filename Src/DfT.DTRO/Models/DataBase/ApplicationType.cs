namespace DfT.DTRO.Models.DataBase;

/// <summary>
/// Wrapper for Application Type
/// </summary>
public class ApplicationType
{
    /// <summary>
    /// System unique identifier of this document 
    /// </summary>
    [Key]
    [SwaggerSchema(ReadOnly = true)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(TypeName = "uuid")]
    public Guid Id { get; set; }

    /// <summary>
    /// Application type name
    /// </summary>
    public string Name { get; set; }
}
