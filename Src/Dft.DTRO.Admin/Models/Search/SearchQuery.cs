namespace Dft.DTRO.Admin.Models.Search;

public class SearchQuery
{
    public Location Location { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? PublicationTime { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? ModificationTime { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? DeletionTime { get; set; }

    [DataMember(Name = "traCreator")]
    public int? TraCreator { get; set; }

    [DataMember(Name = "currentTraOwner")]
    public int? CurrentTraOwner { get; set; }

    [MinLength(0)]
    [MaxLength(500)]
    public string TroName { get; set; }

    [MinLength(0)]
    [MaxLength(60)]
    public string RegulationType { get; set; }

    [MinLength(0)]
    [MaxLength(40)]
    public string VehicleType { get; set; }

    [MinLength(0)]
    [MaxLength(50)]
    public string OrderReportingPoint { get; set; }

    public ValueCondition<DateTime> RegulationStart { get; set; }

    public ValueCondition<DateTime> RegulationEnd { get; set; }

}