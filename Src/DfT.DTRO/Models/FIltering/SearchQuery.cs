namespace DfT.DTRO.Models.Filtering;

/// <summary>
/// D-TRO search criteria for a single query.
/// </summary>
public class SearchQuery
{
    /// <summary>
    /// Spatial search parameters.
    /// </summary>
    public Location Location { get; set; }

    /// <summary>
    /// Timestamp the D-TRO was published.
    /// </summary>
    [DataType(DataType.DateTime)]
    public DateTime? PublicationTime { get; set; }

    /// <summary>
    /// Timestamp the D-TRO was last updated.
    /// </summary>
    [DataType(DataType.DateTime)]
    public DateTime? ModificationTime { get; set; }

    /// <summary>
    /// Timestamp the D-TRO was deleted.
    /// </summary>
    [DataType(DataType.DateTime)]
    public DateTime? DeletionTime { get; set; }

    /// <summary>
    /// Traffic regulation authority that created D-TRO 
    /// </summary>
    [DataMember(Name = "traCreator")]
    public int? TraCreator { get; set; }

    /// <summary>
    /// Traffic regulation authority that owns D-TRO
    /// </summary>
    [DataMember(Name = "currentTraOwner")]
    public int? CurrentTraOwner { get; set; }

    /// <summary>
    /// Name of the traffic regulation order.
    /// </summary>
    [MinLength(0)]
    [MaxLength(500)]
    public string TroName { get; set; }

    /// <summary>
    /// Regulation type the restriction applies to.
    /// </summary>
    [MinLength(0)]
    [MaxLength(60)]
    public string RegulationType { get; set; }

    /// <summary>
    /// Vehicle type the restriction applies to.
    /// </summary>
    [MinLength(0)]
    [MaxLength(40)]
    public string VehicleType { get; set; }

    /// <summary>
    /// Reporting point of the D-TRO.
    /// </summary>
    [MinLength(0)]
    [MaxLength(50)]
    public string OrderReportingPoint { get; set; }

    /// <summary>
    /// Timestamp when the regulation starts.
    /// </summary>
    public ValueCondition<DateTime> RegulationStart { get; set; }

    /// <summary>
    /// Timestamp when the regulation ends.
    /// </summary>
    public ValueCondition<DateTime> RegulationEnd { get; set; }
}