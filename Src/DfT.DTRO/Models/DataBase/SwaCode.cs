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
    /// Gets or sets unique identifier of the TRA.
    /// </summary>
    [Key]
    [SwaggerSchema(ReadOnly = true)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(TypeName = "uuid")]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the TRA id of the request.
    /// </summary>
    /// <example>1585</example>
    [Required(ErrorMessage = "TRA id field must be included")]
    [DataMember(Name = "traId")]
    public int TraId { get; set; }

    /// <summary>
    /// Gets or sets the TRA name of the request.
    /// </summary>
    [Required(ErrorMessage = "TRA name field must be included")]
    [DataMember(Name = "name")]
    [StringLength(int.MaxValue)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the TRA prefix of the request.
    /// </summary>
    [Required(ErrorMessage = "TRA prefix field must be included")]
    [DataMember(Name = "prefix")]
    [StringLength(20)]
    public string Prefix { get; set; }

    /// <summary>
    /// Gets or sets the TRA as administrator.
    /// </summary>
    [DataMember(Name = "IsAdmin")]
    public bool IsAdmin { get; set; }
}