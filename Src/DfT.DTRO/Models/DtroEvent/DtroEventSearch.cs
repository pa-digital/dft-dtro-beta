using DfT.DTRO.Models.DtroJson;
using DfT.DTRO.Models.Filtering;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace DfT.DTRO.Models.DtroEvent;

/// <summary>
/// A structure of the search query request.
/// </summary>
[DataContract]
public class DtroEventSearch
{
    /// <summary>
    /// One-based number of the requested page.
    /// </summary>
    /// <example>1.</example>
    [Required]
    [Range(1, int.MaxValue)]
    [DataMember(Name = "page")]
    public int? Page { get; set; }

    /// <summary>
    /// Size of the requested page.
    /// </summary>
    /// <example>10.</example>
    [Required]
    [Range(1, 50)]
    [DataMember(Name = "pageSize")]
    public int? PageSize { get; set; }

    /// <summary>
    /// Timestamp from which to search events.
    /// </summary>
    [Required]
    [DataMember(Name = "since")]
    public DateTime? Since { get; set; }

    /// <summary>
    /// The time DTRO was deleted.
    /// </summary>
    public DateTime? DeletionTime { get; set; }

    /// <summary>
    /// The time DTRO was last updated.
    /// </summary>
    public DateTime? ModificationTime { get; set; }

    /// <summary>
    /// The unique identifier of the traffic authority that created the TRO.
    /// </summary>
    [DataMember(Name = "taCreator")]
    public int? TaCreator { get; set; }

    [DataMember(Name = "taOwner")]
    public int? TaOwner { get; set; }

    /// <summary>
    /// Published title of the Traffic Regulation Order.
    /// </summary>
    [DataMember(Name = "troName")]
    public string TroName { get; set; }

    /// <summary>
    /// Regulation type.
    /// </summary>
    [DataMember(Name = "regulationType")]
    public string RegulationType { get; set; }

    /// <summary>
    /// Type of vehicle the restriction applies to.
    /// </summary>
    [DataMember(Name = "vehicleType")]
    public string VehicleType { get; set; }

    /// <summary>
    /// Reporting point of the D-TRO.
    /// </summary>
    [DataMember(Name = "orderReportingPoint")]
    public string OrderReportingPoint { get; set; }

    /// <summary>
    /// Limits results to a specified location
    /// and a coordinate reference system name
    /// expressed by a bounding box.
    /// </summary>
    [DataMember(Name = "location")]
    public Location Location { get; set; }

    /// <summary>
    /// Date and time when the regulation starts.
    /// </summary>
    [DataMember(Name = "regulationStart")]
    public ValueCondition<DateTime> RegulationStart { get; set; }

    /// <summary>
    /// Date and time when the regulation ends.
    /// </summary>
    [DataMember(Name = "regulationEnd")]
    public ValueCondition<DateTime> RegulationEnd { get; set; }
}