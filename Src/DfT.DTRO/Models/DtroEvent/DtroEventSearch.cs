namespace DfT.DTRO.Models.DtroEvent;

[DataContract]
public class DtroEventSearch
{
    [Required]
    [Range(1, int.MaxValue)]
    [DataMember(Name = "page")]
    public int? Page { get; set; }

    [Required]
    [Range(1, 50)]
    [DataMember(Name = "pageSize")]
    public int? PageSize { get; set; }

    [Required]
    [DataMember(Name = "since")]
    public DateTime? Since { get; set; }

    public DateTime? DeletionTime { get; set; }

    public DateTime? ModificationTime { get; set; }

    [DataMember(Name = "traCreator")]
    public int? TraCreator { get; set; }

    [DataMember(Name = "currentTraOwner")]
    public int? CurrentTraOwner { get; set; }

    [DataMember(Name = "troName")]
    public string TroName { get; set; }

    [DataMember(Name = "regulationType")]
    public string RegulationType { get; set; }

    [DataMember(Name = "regulatedPlaceType")]
    public string RegulatedPlaceType { get; set; }

    [DataMember(Name = "vehicleType")]
    public string VehicleType { get; set; }

    [DataMember(Name = "orderReportingPoint")]
    public string OrderReportingPoint { get; set; }

    //[DataMember(Name = "location")]
    //public Location Location { get; set; }

    [DataMember(Name = "regulationStart")]
    public ValueCondition<DateTime> RegulationStart { get; set; }

    [DataMember(Name = "regulationEnd")]
    public ValueCondition<DateTime> RegulationEnd { get; set; }
}