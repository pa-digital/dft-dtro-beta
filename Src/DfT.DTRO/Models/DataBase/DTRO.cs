using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Runtime.Serialization;
using DfT.DTRO.Attributes;
using DfT.DTRO.Converters;
using DfT.DTRO.Models.DtroJson;
using DfT.DTRO.Models.SchemaTemplate;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.Annotations;

namespace DfT.DTRO.Models.DataBase;

[DataContract]
public class DTRO
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

    public DateTime? RegulationStart { get; set; }

    public DateTime? RegulationEnd { get; set; }

    [DataMember(Name = "ta")]
    [Column("TA")]
    public int TrafficAuthorityId { get; set; }

    public string TroName { get; set; }

    [DataMember(Name = "createdCorrelationId")]
    [SaveOnce]
    [SwaggerSchema(ReadOnly = true)]
    public string CreatedCorrelationId { get; set; }

    [DataMember(Name = "lastUpdatedCorrelationId")]
    [SwaggerSchema(ReadOnly = true)]
    public string LastUpdatedCorrelationId { get; set; }

    [DataMember(Name = "deleted")]
    [SwaggerSchema(ReadOnly = true)]
    public bool Deleted { get; set; } = false;

    [DataMember(Name = "deletionTime")]
    [SwaggerSchema(ReadOnly = true)]
    public DateTime? DeletionTime { get; set; }

    [Required(ErrorMessage = "data field must be included")]
    [DataMember(Name = "data")]
    [JsonConverter(typeof(ExpandoObjectConverter))]
    public ExpandoObject Data { get; set; }

    public List<string> RegulationTypes { get; set; }

    public List<string> VehicleTypes { get; set; }

    public List<string> OrderReportingPoints { get; set; }

    public BoundingBox Location { get; set; }

    [Required(ErrorMessage = "Source reference field must be included")]
    [DataMember(Name = "SourceReference")]
    [Column("SourceReference")]
    public string SourceReference { get; set; }

    [Required(ErrorMessage = "Source action type field must be included")]
    [DataMember(Name = "SourceActionType")]
    [Column("SourceActionType")]
    public string SourceActionType { get; set; }

    [Required(ErrorMessage = "Provision reference field must be included")]
    [DataMember(Name = "ProvisionReference")]
    [Column("ProvisionReference")]
    public string ProvisionReference { get; set; }

    [Required(ErrorMessage = "Provision action type field must be included")]
    [DataMember(Name = "ProvisionActionType")]
    [Column("ProvisionActionType")]
    public string ProvisionActionType { get; set; }
}