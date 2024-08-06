namespace DfT.DTRO.Models.DtroEvent;

/// <summary>
/// A structure of the search query request.
/// </summary>
[DataContract]
public class DtroEventSearch
{
    /// <summary>
    /// One-based amount the requested page.
    /// </summary>
    [Required]
    [Range(1, int.MaxValue)]
    [DataMember(Name = "page")]
    public int? Page { get; set; }

    /// <summary>
    /// Size of the requested page.
    /// </summary>
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
    /// Time D-TRO was deleted.
    /// </summary>
    public DateTime? DeletionTime { get; set; }

    /// <summary>
    /// Time D-TRO was last updated.
    /// </summary>
    public DateTime? ModificationTime { get; set; }

    /// <summary>
    /// Traffic regulation authority creator ID.
    /// </summary>
    [DataMember(Name = "traCreator")]
    public int? TraCreator { get; set; }

    /// <summary>
    /// Current traffic regulation authority owner ID.
    /// </summary>
    [DataMember(Name = "currentTraOwner")]
    public int? CurrentTraOwner { get; set; }

    /// <summary>
    /// Published title of the traffic regulation order
    /// </summary>
    [DataMember(Name = "troName")]
    public string TroName { get; set; }

    /// <summary>
    /// Regulation type.
    /// </summary>
    [DataMember(Name = "regulationType")]
    public string RegulationType { get; set; }

    /// <summary>
    /// Vehicle type.
    /// </summary>
    [DataMember(Name = "vehicleType")]
    public string VehicleType { get; set; }

    /// <summary>
    /// Reporting point of the D-TRO.
    /// </summary>
    [DataMember(Name = "orderReportingPoint")]
    public string OrderReportingPoint { get; set; }

    /// <summary>
    /// Limit results to a specified location,
    /// and a coordinate reference system name
    /// expressed by a bounding box.
    /// </summary>
    [DataMember(Name = "location")]
    public Location Location { get; set; }

    /// <summary>
    /// Date and time the regulation starts.
    /// </summary>
    [DataMember(Name = "regulationStart")]
    public ValueCondition<DateTime> RegulationStart { get; set; }

    /// <summary>
    /// Date and time the regulation ends.
    /// </summary>
    [DataMember(Name = "regulationEnd")]
    public ValueCondition<DateTime> RegulationEnd { get; set; }
}