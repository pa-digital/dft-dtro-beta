using DfT.DTRO.Models.Search;

namespace DfT.DTRO.Models.DtroEvent;

[DataContract]
public class DtroEvent
{
    [DataMember(Name = "publicationTime")]
    public DateTime PublicationTime { get; set; }

    [DataMember(Name = "traCreator")]
    public int TrafficAuthorityCreatorId { get; set; }

    [DataMember(Name = "currentTraOwner")]
    public int TrafficAuthorityOwnerId { get; set; }

    [DataMember(Name = "troName")]
    public string TroName { get; set; }

    [DataMember(Name = "regulationType")]
    public List<string> RegulationType { get; set; }

    [DataMember(Name = "vehicleType")]
    public List<string> VehicleType { get; set; }

    [DataMember(Name = "orderReportingPoint")]
    public List<string> OrderReportingPoint { get; set; }

    [DataMember(Name = "regulationStart")]
    public List<DateTime> RegulationStart { get; set; }

    [DataMember(Name = "regulationEnd")]
    public List<DateTime> RegulationEnd { get; set; }

    [DataMember(Name = "eventType")]
    public DtroEventType EventType { get; set; }

    [DataMember(Name = "eventTime")]
    public DateTime EventTime { get; set; }

    [DataMember(Name = "_links")]
    public Links Links { get; set; }

    public static DtroEvent FromDeletion(DataBase.DTRO dtro, string baseUrl, List<DateTime> regulationStartTimes, List<DateTime> regulationEndTimes)
    {
        return new DtroEvent
        {
            EventType = DtroEventType.Delete,
            EventTime = dtro.DeletionTime.Value,
            PublicationTime = dtro.Created.Value,
            OrderReportingPoint = dtro.OrderReportingPoints,
            VehicleType = dtro.VehicleTypes,
            TrafficAuthorityCreatorId = dtro.TrafficAuthorityCreatorId,
            TrafficAuthorityOwnerId = dtro.TrafficAuthorityOwnerId,
            RegulationType = dtro.RegulationTypes,
            TroName = dtro.TroName,
            RegulationStart = regulationStartTimes,
            RegulationEnd = regulationEndTimes,
            Links = new Links { Self = $"{baseUrl}/v1/dtros/{dtro.Id}" }
        };
    }

    public static DtroEvent FromCreation(DataBase.DTRO dtro, string baseUrl, List<DateTime> regulationStartTimes, List<DateTime> regulationEndTimes)
    {
        return new DtroEvent
        {
            EventType = DtroEventType.Create,
            EventTime = dtro.Created.Value,
            PublicationTime = dtro.Created.Value,
            OrderReportingPoint = dtro.OrderReportingPoints,
            VehicleType = dtro.VehicleTypes,
            TrafficAuthorityCreatorId = dtro.TrafficAuthorityCreatorId,
            TrafficAuthorityOwnerId = dtro.TrafficAuthorityOwnerId,
            RegulationType = dtro.RegulationTypes,
            TroName = dtro.TroName,
            RegulationStart = regulationStartTimes,
            RegulationEnd = regulationEndTimes,
            Links = new Links { Self = $"{baseUrl}/v1/dtros/{dtro.Id}" }
        };
    }

    public static DtroEvent FromUpdate(DataBase.DTRO dtro, string baseUrl, List<DateTime> regulationStartTimes, List<DateTime> regulationEndTimes)
    {
        return new DtroEvent
        {
            EventType = DtroEventType.Update,
            EventTime = dtro.LastUpdated.Value,
            PublicationTime = dtro.Created.Value,
            OrderReportingPoint = dtro.OrderReportingPoints,
            VehicleType = dtro.VehicleTypes,
            TrafficAuthorityCreatorId = dtro.TrafficAuthorityCreatorId,
            TrafficAuthorityOwnerId = dtro.TrafficAuthorityOwnerId,
            RegulationType = dtro.RegulationTypes,
            TroName = dtro.TroName,
            RegulationStart = regulationStartTimes,
            RegulationEnd = regulationEndTimes,
            Links = new Links { Self = $"{baseUrl}/v1/dtros/{dtro.Id}" }
        };
    }
}
