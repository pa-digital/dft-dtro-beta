using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DfT.DTRO.Models.DataBase;

[DataContract]
public class SchemaTemplate
{
    [Key]
    [SwaggerSchema(ReadOnly = true)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(TypeName = "uuid")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "schemaVersion field must be included")]
    [DataMember(Name = "schemaVersion")]
    [JsonConverter(typeof(SchemaVersionJsonConverter))]
    public SchemaVersion SchemaVersion { get; set; }

    [DataMember(Name = "created")]
    [SaveOnce]
    [SwaggerSchema(ReadOnly = true)]
    public DateTime? Created { get; set; }

    [DataMember(Name = "lastUpdated")]
    [SwaggerSchema(ReadOnly = true)]
    public DateTime? LastUpdated { get; set; }

    [DataMember(Name = "createdCorrelationId")]
    [SaveOnce]
    [SwaggerSchema(ReadOnly = true)]
    public string CreatedCorrelationId { get; set; }

    [DataMember(Name = "lastUpdatedCorrelationId")]
    [SwaggerSchema(ReadOnly = true)]
    public string LastUpdatedCorrelationId { get; set; }

    [DataMember(Name = "isActive")]
    [SwaggerSchema(ReadOnly = true)]
    public bool IsActive { get; set; } = false;

    [Required(ErrorMessage = "Template field must be included")]
    [DataMember(Name = "template")]
    [JsonConverter(typeof(ExpandoObjectConverter))]
    public ExpandoObject Template { get; set; }
}