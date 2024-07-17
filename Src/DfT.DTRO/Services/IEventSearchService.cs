using DfT.DTRO.Models.DtroEvent;

namespace DfT.DTRO.Services;

public interface IEventSearchService
{
    Task<DtroEventSearchResult> SearchAsync(DtroEventSearch search);
}