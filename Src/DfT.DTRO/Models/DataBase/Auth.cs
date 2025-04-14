namespace DfT.DTRO.Models.DataBase;
/// <summary>
/// Wrapper for Auth
/// </summary>
[ExcludeFromCodeCoverage]
[DataContract]
public class Auth
{
    /// <summary>
    /// Unique identifier.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(TypeName = "uuid")]
    public Guid Id { get; set; }

    /// <summary>
    /// User ID
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; }

    /// <summary>
    /// Password
    /// </summary>
    [Required]
    public string Password { get; set; }
}