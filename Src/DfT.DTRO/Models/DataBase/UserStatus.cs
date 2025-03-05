namespace DfT.DTRO.Models.DataBase;

/// <summary>
/// Wrapper for User Status table
/// </summary>
[ExcludeFromCodeCoverage]
[DataContract]
public class UserStatus
{

    /// <summary>
    /// User Status unique identifier.
    /// </summary>
    [Key]
    [SwaggerSchema(ReadOnly = true)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(TypeName = "uuid")]
    public Guid Id { get; set; }

    /// <summary>
    /// User status
    /// </summary>
    public string Status { get; set; }
}