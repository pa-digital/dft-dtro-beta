public class ErrorReportRequest
{
    public string? TroId { get; set; }
    public List<string>? Tras { get; set; }
    public List<string>? RegulationTypes { get; set; }
    public List<string>? TroTypes { get; set; }
    public string Type { get; set; }
    public string? OtherType { get; set; }
    public string MoreInformation { get; set; }
    public List<IFormFile>? Files { get; set; } = new();
}
