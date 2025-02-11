namespace DfT.DTRO.Models.DataBase;

/// <summary>
/// Wrapper for Application Purpose
/// </summary>
[DataContract]
public class ApplicationPurpose
{
    /// <summary>
    /// Application Purpose unique identifier.
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

    /// <summary>
    /// Application 
    /// </summary>
    public Application Application { get; set; }
}