namespace DfT.DTRO.Models.DataBase;

/// <summary>
/// Wrapper for User Status
/// </summary>
public class UserStatus
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
    /// Status of this document
    /// </summary>
    [DataMember(Name = "status")]
    [SwaggerSchema(ReadOnly = true)]
    public string Status { get; set; }
}