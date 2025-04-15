namespace DfT.DTRO.Models.DataBase;
/// <summary>
/// Wrapper for TwoFactorAuthentication
/// </summary>
[ExcludeFromCodeCoverage]
[DataContract]
public class TwoFactorAuthentication
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
    /// Code
    /// </summary>
    [Required]
    public string Code { get; set; }

    /// <summary>
    /// Expires at
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// Session token for identifying the user pre-JWT
    /// </summary>
    [Required]
    public Guid Token { get; set; }
}