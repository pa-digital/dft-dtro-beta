using DfT.DTRO.Attributes;
using DfT.DTRO.Converters;
using DfT.DTRO.Models.DtroJson;
using DfT.DTRO.Models.SchemaTemplate;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Runtime.Serialization;

namespace DfT.DTRO.Models.DataBase;

/// <summary>
/// Wrapper for a DTRO submission.
/// </summary>
[DataContract]
public class DTRO
{
    /// <summary>
    /// Id of the DTRO.
    /// </summary>
    [Key]
    [SwaggerSchema(ReadOnly = true)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(TypeName = "uuid")]
    public Guid Id { get; set; }

    /// <summary>
    /// The schema identifier of the DTRO data payload being submitted.
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
    /// The earliest of regulation start dates.
    /// </summary>
    public DateTime? RegulationStart { get; set; }

    /// <summary>
    /// The latest of regulation end dates.
    /// </summary>
    public DateTime? RegulationEnd { get; set; }

    /// <summary>
    /// The unique identifier of the traffic authority creating the DTRO.
    /// </summary>
    [DataMember(Name = "ta")]
    [Column("TA")]
    public int TrafficAuthorityId { get; set; }

    /// <summary>
    /// The descriptive name of the DTRO.
    /// </summary>
    public string TroName { get; set; }

    /// <summary>
    /// Correlation ID of the request with which this DTRO was created.
    /// </summary>
    [DataMember(Name = "createdCorrelationId")]
    [SaveOnce]
    [SwaggerSchema(ReadOnly = true)]
    public string CreatedCorrelationId { get; set; }

    /// <summary>
    /// Correlation ID of the request with which this DTRO was last updated.
    /// </summary>
    [DataMember(Name = "lastUpdatedCorrelationId")]
    [SwaggerSchema(ReadOnly = true)]
    public string LastUpdatedCorrelationId { get; set; }

    /// <summary>
    /// A flag representing whether the DTRO has been deleted.
    /// </summary>
    [DataMember(Name = "deleted")]
    [SwaggerSchema(ReadOnly = true)]
    public bool Deleted { get; set; } = false;

    /// <summary>
    /// Timestamp that represents when the DTRO was deleted.
    /// <br/><br/>
    /// <see langword="null"/> if <see cref="Deleted"/> is <see langword="false"/>.
    /// </summary>
    [DataMember(Name = "deletionTime")]
    [SwaggerSchema(ReadOnly = true)]
    public DateTime? DeletionTime { get; set; }

    /// <summary>
    /// The DTRO data model being submitted.
    /// </summary>
    [Required(ErrorMessage = "data field must be included")]
    [DataMember(Name = "data")]
    [JsonConverter(typeof(ExpandoObjectConverter))]
    public ExpandoObject Data { get; set; }

    /// <summary>
    /// Unique regulation types that this DTRO consists of.
    /// </summary>
    public List<string> RegulationTypes { get; set; }

    /// <summary>
    /// Unique vehicle types that this DTRO applies to.
    /// </summary>
    public List<string> VehicleTypes { get; set; }

    /// <summary>
    /// Unique order reporting points that this DTRO applies to.
    /// </summary>
    public List<string> OrderReportingPoints { get; set; }

    /// <summary>
    /// The bounding box containing all points from this DTRO's regulated places.
    /// </summary>
    public BoundingBox Location { get; set; }
}