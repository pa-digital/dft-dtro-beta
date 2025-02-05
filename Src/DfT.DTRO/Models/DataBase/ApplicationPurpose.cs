namespace DfT.DTRO.Models.DataBase;

/// <summary>
/// Wrapper for Application Purpose
/// </summary>
public class ApplicationPurpose
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
    /// Application Purpose description
    /// </summary>
    public string Description { get; set; }
}