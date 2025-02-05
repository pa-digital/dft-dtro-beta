namespace DfT.DTRO.Models.Search;

[DataContract]
public class DtroSearchResult
{
    [DataMember(Name = "troName")]
    public string TroName { get; set; }

    [DataMember(Name = "publicationTime")]
    public DateTime PublicationTime { get; set; }

    [DataMember(Name = "trafficAuthorityCreatorId")]
    public long TrafficAuthorityCreatorId { get; set; }

    [DataMember(Name = "trafficAuthorityOwnerId")]
    public long TrafficAuthorityOwnerId { get; set; }

    [DataMember(Name = "regulationType")]
    public IEnumerable<string> RegulationType { get; set; }

    [DataMember(Name = "vehicleType")]
    public IEnumerable<string> VehicleType { get; set; }

    [DataMember(Name = "orderReportingPoint")]
    public IEnumerable<string> OrderReportingPoint { get; set; }

    [DataMember(Name = "regulatedPlaceTypes")]
    public IEnumerable<string> RegulatedPlaceType { get; set; }

    [DataMember(Name = "regulationStart")]
    public IEnumerable<DateTime> RegulationStart { get; set; }

    [DataMember(Name = "regulationEnd")]
    public IEnumerable<DateTime> RegulationEnd { get; set; }

    [DataMember(Name = "Id")]
    public Guid Id { get; set; }
}