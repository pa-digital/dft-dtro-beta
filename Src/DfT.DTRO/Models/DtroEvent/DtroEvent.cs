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

    public static DtroEvent FromDeletion(DataBase.DTRO dtro, string baseUrl, List<DateTime> regulationStartTimes, List<DateTime> regulationEndTimes)
    {
        return new DtroEvent
        {
            EventType = DtroEventType.Delete,
            EventTime = dtro.DeletionTime.Value.ToDateTimeTruncated(),
            PublicationTime = dtro.LastUpdated.Value.ToDateTimeTruncated(),
            OrderReportingPoint = dtro.OrderReportingPoints,
            VehicleType = dtro.VehicleTypes,
            DtroId = dtro.Id,
            TrafficAuthorityCreatorId = dtro.TrafficAuthorityCreatorId,
            TrafficAuthorityOwnerId = dtro.TrafficAuthorityOwnerId,
            RegulationType = dtro.RegulationTypes,
            TroName = dtro.TroName,
            RegulationStart = regulationStartTimes,
            RegulationEnd = regulationEndTimes,
            Links = new Links { Self = $"{baseUrl}/dtros/{dtro.Id}" }
        };
    }

    public static DtroEvent FromCreation(DataBase.DTRO dtro, string baseUrl, List<DateTime> regulationStartTimes, List<DateTime> regulationEndTimes)
    {
        return new DtroEvent
        {
            EventType = DtroEventType.Create,
            EventTime = dtro.Created.Value.ToDateTimeTruncated(),
            PublicationTime = dtro.LastUpdated.Value.ToDateTimeTruncated(),
            OrderReportingPoint = dtro.OrderReportingPoints,
            VehicleType = dtro.VehicleTypes,
            DtroId = dtro.Id,
            TrafficAuthorityCreatorId = dtro.TrafficAuthorityCreatorId,
            TrafficAuthorityOwnerId = dtro.TrafficAuthorityOwnerId,
            RegulationType = dtro.RegulationTypes,
            TroName = dtro.TroName,
            RegulationStart = regulationStartTimes,
            RegulationEnd = regulationEndTimes,
            Links = new Links { Self = $"{baseUrl}/dtros/{dtro.Id}" }
        };
    }

    public static DtroEvent FromUpdate(DataBase.DTRO dtro, string baseUrl, List<DateTime> regulationStartTimes, List<DateTime> regulationEndTimes)
    {
        return new DtroEvent
        {
            EventType = DtroEventType.Update,
            EventTime = dtro.LastUpdated.Value.ToDateTimeTruncated(),
            PublicationTime = dtro.LastUpdated.Value.ToDateTimeTruncated(),
            OrderReportingPoint = dtro.OrderReportingPoints,
            VehicleType = dtro.VehicleTypes,
            DtroId = dtro.Id,
            TrafficAuthorityCreatorId = dtro.TrafficAuthorityCreatorId,
            TrafficAuthorityOwnerId = dtro.TrafficAuthorityOwnerId,
            RegulationType = dtro.RegulationTypes,
            TroName = dtro.TroName,
            RegulationStart = regulationStartTimes,
            RegulationEnd = regulationEndTimes,
            Links = new Links { Self = $"{baseUrl}/dtros/{dtro.Id}" }
        };
    }
}
