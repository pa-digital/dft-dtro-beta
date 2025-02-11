namespace DfT.DTRO.Models.DataBase;

/// <summary>
/// Wrapper for Digital Service Provider
/// </summary>
[DataContract]
public class DigitalServiceProvider
{
    /// <summary>
    /// Digital Service Provider unique identifier.
    /// </summary>
    [Key]
    [SwaggerSchema(ReadOnly = true)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(TypeName = "uuid")]
    public Guid Id { get; set; }

    /// <summary>
    /// Digital Service Provider created date
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    /// Digital Service Provider last updated date
    /// </summary>
    public DateTime? LastUpdated { get; set; }

    /// <summary>
    /// Digital Service Provider status
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// Digital Service Provider users
    /// </summary>
    public List<User> Users { get; set; }
}