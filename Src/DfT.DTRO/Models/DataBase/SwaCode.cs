using System.ComponentModel.DataAnnotations.Schema;

namespace DfT.DTRO.Models.DataBase;

[DataContract]
public class SwaCode
{
    [Key]
    [SwaggerSchema(ReadOnly = true)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(TypeName = "uuid")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "TRA id field must be included")]
    [DataMember(Name = "traId")]
    public int TraId { get; set; }

    [Required(ErrorMessage = "TRA name field must be included")]
    [DataMember(Name = "name")]
    [StringLength(int.MaxValue)]
    public string Name { get; set; }

    [Required(ErrorMessage = "TRA prefix field must be included")]
    [DataMember(Name = "prefix")]
    [StringLength(20)]
    public string Prefix { get; set; }

    [DataMember(Name = "IsAdmin")]
    public bool IsAdmin { get; set; }


    [DataMember(Name = "isActive")]
    public bool IsActive { get; set; } = false;
}