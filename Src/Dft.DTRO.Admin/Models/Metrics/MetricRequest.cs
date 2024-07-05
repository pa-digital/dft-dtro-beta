using System;
using System.Runtime.Serialization;

/// <summary>
/// Request for metric data.
/// </summary>

[DataContract]
public class MetricRequest
{
    /// <summary>
    /// Gets or sets system TraId.
    /// </summary>
    [DataMember(Name = "traId")]
    public int? TraId { get; set; }

    /// <summary>
    /// Gets or sets date from.
    /// </summary>
    [DataMember(Name = "dateFrom")]
    public DateTime DateFrom { get; set; }

    /// <summary>
    /// Gets or sets Date to.
    /// </summary>
    [DataMember(Name = "dateTo")]
    public DateTime DateTo { get; set; }
}