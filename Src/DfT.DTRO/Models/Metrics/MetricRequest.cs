namespace DfT.DTRO.Models.Metrics;

[DataContract]
public class MetricRequest
{
    [DataMember(Name = "traId")]
    public int? TraId { get; set; }

    [DataMember(Name = "dateFrom")]
    public DateTime DateFrom { get; set; }

    [DataMember(Name = "dateTo")]
    public DateTime DateTo { get; set; }
}