using System.ComponentModel.DataAnnotations.Schema;

namespace DfT.DTRO.Models.DataBase;

[DataContract]
public class SystemConfig
{
    [Key]
    [SwaggerSchema(ReadOnly = true)]
    [Column(TypeName = "uuid")]
    public Guid Id { get; set; }

   
    [Required(ErrorMessage = "System Name must be included")]
    [DataMember(Name = "systemName")]
    [StringLength(100)]
    public string SystemName { get; set; }

    [DataMember(Name = "isTest")]
    [SwaggerSchema(ReadOnly = true)]
    public bool IsTest { get; set; }

}