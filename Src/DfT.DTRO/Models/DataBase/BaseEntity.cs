namespace DfT.DTRO.Models.DataBase;

/// <summary>
/// Base entity
/// </summary>
public class BaseEntity
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
    /// Timestamp representing the date and time this document was created.
    /// </summary>
    [DataMember(Name = "created")]
    [SaveOnce]
    [SwaggerSchema(ReadOnly = true)]
    public DateTime? Created { get; set; }

    /// <summary>
    /// Timestamp representing the last date and time this document was updated.
    /// </summary>
    [DataMember(Name = "lastUpdated")]
    [SwaggerSchema(ReadOnly = true)]
    public DateTime? LastUpdated { get; set; }


    /// <summary>
    /// Name of this document
    /// </summary>
    [DataMember(Name = "name")]
    [SwaggerSchema(ReadOnly = true)]
    public string Name { get; set; }


    /// <summary>
    /// Status of this document
    /// </summary>
    [DataMember(Name = "status")]
    [SwaggerSchema(ReadOnly = true)]
    public bool Status { get; set; }
}