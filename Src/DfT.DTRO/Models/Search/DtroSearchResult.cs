using DfT.DTRO.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DfT.DTRO.Models.Search;

/// <summary>
/// Response to DTRO search query.
/// </summary>
[DataContract]
public class DtroSearchResult
{
    /// <summary>
    /// The name of the DTRO.
    /// </summary>
    [DataMember(Name = "troName")]
    public string TroName { get; set; }

    /// <summary>
    /// The time when the DTRO was created.
    /// </summary>
    [DataMember(Name = "publicationTime")]
    public DateTime PublicationTime { get; set; }

    /// <summary>
    /// The identifier of the Traffic Authority.
    /// </summary>
    [DataMember(Name = "ta")]
    public long TrafficAuthorityId { get; set; }

    /// <summary>
    /// The types of all regulations in the DTRO.
    /// </summary>
    [DataMember(Name = "regulationType")]
    public IEnumerable<string> RegulationType { get; set; }

    /// <summary>
    /// The types of all vehicles specified in conditions section of the DTRO.
    /// </summary>
    [DataMember(Name = "vehicleType")]
    public IEnumerable<string> VehicleType { get; set; }

    /// <summary>
    /// The list of all order reporting points specified in the DTRO.
    /// </summary>
    [DataMember(Name = "orderReportingPoint")]
    public IEnumerable<string> OrderReportingPoint { get; set; }

    /// <summary>
    /// List of all regulation start dates specified in the DTRO.
    /// </summary>
    [DataMember(Name = "regulationStart")]
    public IEnumerable<DateTime> RegulationStart { get; set; }

    /// <summary>
    /// List of all regulation end dates specified in the DTRO.
    /// </summary>
    [DataMember(Name = "regulationEnd")]
    public IEnumerable<DateTime> RegulationEnd { get; set; }

    /// <summary>
    /// The links to related resources.
    /// </summary>
    [DataMember(Name = "_links")]
    public Links Links { get; set; }

    public static DtroSearchResult FromDtro(DataBase.DTRO dtro, string baseUrl, List<DateTime> regulationStartDates, List<DateTime> regulationEndDates)
    {
        return new DtroSearchResult
        {
            TroName = dtro.Data.GetValueOrDefault<string>("source.troName"),
            TrafficAuthorityId = dtro.Data.GetExpando("source").HasField("ta")
                ? dtro.Data.GetValueOrDefault<int>("source.ta")
                : dtro.Data.GetValueOrDefault<int>("source.ha"),
            PublicationTime = dtro.Created.Value,
            RegulationType = dtro.RegulationTypes,
            VehicleType = dtro.VehicleTypes,
            OrderReportingPoint = dtro.OrderReportingPoints,
            RegulationStart = regulationStartDates,
            RegulationEnd = regulationEndDates,
            Links = new Links
            {
                Self = $"{baseUrl}/v1/dtros/{dtro.Id}"
            }
        };
    }
}