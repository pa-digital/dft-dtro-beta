namespace DfT.DTRO.Models.Metrics;

/// <summary>
/// Parameters of a request for metric data.
/// </summary>
[DataContract]
public class MetricRequest
{
    /// <summary>
    /// Traffic regulation authority ID.
    /// </summary>
    [DataMember(Name = "traId")]
    public int? TraId { get; set; }

    /// <summary>
    /// Timestamp when the metric starts.
    /// </summary>
    [DataMember(Name = "dateFrom")]
    public DateTime DateFrom { get; set; }

    /// <summary>
    /// Timestamp when the metric ends.
    /// </summary>
    [DataMember(Name = "dateTo")]
    public DateTime DateTo { get; set; }
}