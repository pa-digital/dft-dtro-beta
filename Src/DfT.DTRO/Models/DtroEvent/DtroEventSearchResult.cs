namespace DfT.DTRO.Models.DtroEvent;

public class DtroEventSearchResult
{
    public List<DtroEvent> Events { get; set; } = new();

    public int Page { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }
}