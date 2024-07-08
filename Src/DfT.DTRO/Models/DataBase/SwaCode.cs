using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace DfT.DTRO.Models.DataBase;

[DataContract]
public class SwaCode
{
    /// <summary>
    /// Gets or sets id of the SWA.
    /// </summary>
    [Key]
    [SwaggerSchema(ReadOnly = true)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(TypeName = "uuid")]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the SWA code of the request.
    /// </summary>
    /// <example>1585</example>
    [Required(ErrorMessage = "SWA code field must be included")]
    [DataMember(Name = "code")]
    public int Code { get; set; }

    /// <summary>
    /// Gets or sets the SWA name of the request.
    /// </summary>
    [Required(ErrorMessage = "SWA name field must be included")]
    [DataMember(Name = "name")]
    [StringLength(int.MaxValue)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the SWA prefix of the request.
    /// </summary>
    [Required(ErrorMessage = "SWA prefix field must be included")]
    [DataMember(Name = "prefix")]
    [StringLength(20)]
    public string Prefix { get; set; }
}