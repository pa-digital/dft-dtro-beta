namespace DfT.DTRO.Models.DataBase;

/// <summary>
/// Wrapper for Street Work Manager codes.
/// </summary>
[DataContract]
public class DtroUser
{
    /// <summary>
    /// ID of the SWA document.
    /// </summary>
    [Key]
    [SwaggerSchema(ReadOnly = true)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(TypeName = "uuid")]
    public Guid Id { get; set; }


    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(TypeName = "uuid")]
    public Guid xAppId { get; set; }

    /// <summary>
    /// The ID of the traffic regulation authority
    /// </summary>
    [DataMember(Name = "traId")]
    public int? TraId { get; set; }

    /// <summary>
    /// The name of the traffic regulation authority
    /// </summary>
    [Required(ErrorMessage = "TRA name field must be included")]
    [DataMember(Name = "name")]
    [StringLength(int.MaxValue)]
    public string Name { get; set; }

    /// <summary>
    /// The prefix of the traffic regulation authority
    /// </summary>
    [DataMember(Name = "prefix")]
    [StringLength(20)]
    public string Prefix { get; set; }

    /// <summary>
    /// The user group
    /// </summary>
    [DataMember(Name = "userGroup")]
    public int UserGroup { get; set; }
}