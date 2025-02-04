namespace DfT.DTRO.Models.DataBase;

//TODO: This will become obsolete and should be removed once the new structure is on-line.
/// <summary>
/// Wrapper for Street Work Manager codes.
/// </summary>
[DataContract]
public class DtroUser : BaseEntity
{
    /// <summary>
    /// The unique identifier of the application used
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(TypeName = "uuid")]
    public Guid xAppId { get; set; }

    /// <summary>
    /// The ID of the traffic regulation authority
    /// </summary>
    [DataMember(Name = "traId")]
    public int? TraId { get; set; }

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