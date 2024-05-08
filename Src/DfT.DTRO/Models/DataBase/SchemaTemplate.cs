using DfT.DTRO.Attributes;
using DfT.DTRO.Converters;
using DfT.DTRO.Models.SchemaTemplate;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Runtime.Serialization;

namespace DfT.DTRO.Models.DataBase;

/// <summary>
/// Wrapper for a Schema.
/// </summary>
[DataContract]
public class SchemaTemplate
{
    /// <summary>
    /// Id of the Schema.
    /// </summary>
    [Key]
    [SwaggerSchema(ReadOnly = true)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(TypeName = "uuid")]
    public Guid Id { get; set; }

    /// <summary>
    /// The schema identifier of the Schema data payload being submitted.
    /// </summary>
    /// <example>3.1.1.</example>
    [Required(ErrorMessage = "schemaVersion field must be included")]
    [DataMember(Name = "schemaVersion")]
    [JsonConverter(typeof(SchemaVersionJsonConverter))]
    public SchemaVersion SchemaVersion { get; set; }

    /// <summary>
    /// Timestamp that represents the creation time of this document.
    /// </summary>
    [DataMember(Name = "created")]
    [SaveOnce]
    [SwaggerSchema(ReadOnly = true)]
    public DateTime? Created { get; set; }

    /// <summary>
    /// Timestamp that represents the last time this document was updated.
    /// </summary>
    [DataMember(Name = "lastUpdated")]
    [SwaggerSchema(ReadOnly = true)]
    public DateTime? LastUpdated { get; set; }

    /// <summary>
    /// Correlation ID of the request with which this schema was created.
    /// </summary>
    [DataMember(Name = "createdCorrelationId")]
    [SaveOnce]
    [SwaggerSchema(ReadOnly = true)]
    public string CreatedCorrelationId { get; set; }

    /// <summary>
    /// Correlation ID of the request with which this schema was last updated.
    /// </summary>
    [DataMember(Name = "lastUpdatedCorrelationId")]
    [SwaggerSchema(ReadOnly = true)]
    public string LastUpdatedCorrelationId { get; set; }

    /// <summary>
    /// A flag representing whether the Schema is still avaiable.
    /// </summary>
    [DataMember(Name = "isActive")]
    [SwaggerSchema(ReadOnly = true)]
    public bool IsActive { get; set; } = false;

    /// <summary>
    /// The Schema data model being submitted.
    /// </summary>
    [Required(ErrorMessage = "Template field must be included")]
    [DataMember(Name = "template")]
    [JsonConverter(typeof(ExpandoObjectConverter))]
    public ExpandoObject Template { get; set; }
}