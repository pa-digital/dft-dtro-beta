namespace DfT.DTRO.Models.DtroEvent;

/// <summary>
/// Record representing a change event in the D-TRO service.
/// </summary>
[DataContract]
public class DtroEvent
{
    /// <summary>
    /// D-TRO ID
    /// </summary>
    [DataMember(Name = "id")]
    public Guid DtroId { get; set; }

    /// <summary>
    /// Date and time the D-TRO was published.
    /// </summary>
    [DataMember(Name = "publicationTime")]
    public DateTime PublicationTime { get; set; }

    /// <summary>
    /// Traffic Authority Regulation that created the D-TRO.
    /// </summary>
    [DataMember(Name = "traCreator")]
    public int TrafficAuthorityCreatorId { get; set; }

    /// <summary>
    /// Traffic Authority Regulation that owns the D-TRO.
    /// </summary>
    [DataMember(Name = "currentTraOwner")]
    public int TrafficAuthorityOwnerId { get; set; }

    /// <summary>
    /// Traffic Regulation Order name.
    /// </summary>
    [DataMember(Name = "troName")]
    public string TroName { get; set; }

    /// <summary>
    /// Regulation type of the traffic regulation order.
    /// </summary>
    [DataMember(Name = "regulationType")]
    public List<string> RegulationType { get; set; }

    /// <summary>
    /// Vehicle type.
    /// </summary>
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

    public static DtroEvent FromDeletion(DataBase.DigitalTrafficRegulationOrder digitalTrafficRegulationOrder, string baseUrl, List<DateTime> regulationStartTimes, List<DateTime> regulationEndTimes)
    {
        return new DtroEvent
        {
            EventType = DtroEventType.Delete,
            EventTime = digitalTrafficRegulationOrder.DeletionTime.Value.ToDateTimeTruncated(),
            PublicationTime = digitalTrafficRegulationOrder.LastUpdated.Value.ToDateTimeTruncated(),
            OrderReportingPoint = digitalTrafficRegulationOrder.OrderReportingPoints,
            VehicleType = digitalTrafficRegulationOrder.VehicleTypes,
            DtroId = digitalTrafficRegulationOrder.Id,
            TrafficAuthorityCreatorId = digitalTrafficRegulationOrder.TrafficAuthorityCreatorId,
            TrafficAuthorityOwnerId = digitalTrafficRegulationOrder.TrafficAuthorityOwnerId,
            RegulationType = digitalTrafficRegulationOrder.RegulationTypes,
            TroName = digitalTrafficRegulationOrder.TroName,
            RegulationStart = regulationStartTimes,
            RegulationEnd = regulationEndTimes,
            Links = new Links { Self = $"{baseUrl}/dtros/{digitalTrafficRegulationOrder.Id}" }
        };
    }

    public static DtroEvent FromCreation(DataBase.DigitalTrafficRegulationOrder digitalTrafficRegulationOrder, string baseUrl, List<DateTime> regulationStartTimes, List<DateTime> regulationEndTimes)
    {
        return new DtroEvent
        {
            EventType = DtroEventType.Create,
            EventTime = digitalTrafficRegulationOrder.Created.Value.ToDateTimeTruncated(),
            PublicationTime = digitalTrafficRegulationOrder.LastUpdated.Value.ToDateTimeTruncated(),
            OrderReportingPoint = digitalTrafficRegulationOrder.OrderReportingPoints,
            VehicleType = digitalTrafficRegulationOrder.VehicleTypes,
            DtroId = digitalTrafficRegulationOrder.Id,
            TrafficAuthorityCreatorId = digitalTrafficRegulationOrder.TrafficAuthorityCreatorId,
            TrafficAuthorityOwnerId = digitalTrafficRegulationOrder.TrafficAuthorityOwnerId,
            RegulationType = digitalTrafficRegulationOrder.RegulationTypes,
            TroName = digitalTrafficRegulationOrder.TroName,
            RegulationStart = regulationStartTimes,
            RegulationEnd = regulationEndTimes,
            Links = new Links { Self = $"{baseUrl}/dtros/{digitalTrafficRegulationOrder.Id}" }
        };
    }

    public static DtroEvent FromUpdate(DataBase.DigitalTrafficRegulationOrder digitalTrafficRegulationOrder, string baseUrl, List<DateTime> regulationStartTimes, List<DateTime> regulationEndTimes)
    {
        return new DtroEvent
        {
            EventType = DtroEventType.Update,
            EventTime = digitalTrafficRegulationOrder.LastUpdated.Value.ToDateTimeTruncated(),
            PublicationTime = digitalTrafficRegulationOrder.LastUpdated.Value.ToDateTimeTruncated(),
            OrderReportingPoint = digitalTrafficRegulationOrder.OrderReportingPoints,
            VehicleType = digitalTrafficRegulationOrder.VehicleTypes,
            DtroId = digitalTrafficRegulationOrder.Id,
            TrafficAuthorityCreatorId = digitalTrafficRegulationOrder.TrafficAuthorityCreatorId,
            TrafficAuthorityOwnerId = digitalTrafficRegulationOrder.TrafficAuthorityOwnerId,
            RegulationType = digitalTrafficRegulationOrder.RegulationTypes,
            TroName = digitalTrafficRegulationOrder.TroName,
            RegulationStart = regulationStartTimes,
            RegulationEnd = regulationEndTimes,
            Links = new Links { Self = $"{baseUrl}/dtros/{digitalTrafficRegulationOrder.Id}" }
        };
    }
}
