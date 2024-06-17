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
    [DataMember(Name = "taCreator")]
    public long TrafficAuthorityCreatorId { get; set; }

    /// <summary>
    /// The identifier of the Traffic Authority.
    /// </summary>
    [DataMember(Name = "taOwner")]
    public long TrafficAuthorityOwnerId { get; set; }

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
    [DataMember(Name = "Id")]
    public Guid Id { get; set; }
}