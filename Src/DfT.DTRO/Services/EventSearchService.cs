namespace DfT.DTRO.Services;

public class EventSearchService : IEventSearchService
{
    private readonly IDtroService _dtroService;
    private readonly IDtroMappingService _dtroMappingService;

    public EventSearchService(IDtroService dtroService, IDtroMappingService dtroMappingService)
    {
        _dtroService = dtroService;
        _dtroMappingService = dtroMappingService;
    }

    public async Task<DtroEventSearchResult> SearchAsync(DtroEventSearch search)
    {
        if (search.Since is not null && search.Since > DateTime.Now)
        {
            throw new InvalidOperationException("The datetime for the since field cannot be in the future.");
        }

        var searchRes = await _dtroService.FindDtrosAsync(search);

        var events = _dtroMappingService.MapToEvents(searchRes, search.Since);

        if (!events.Any())
        {
            throw new NotFoundException("No event found matching the criteria.");
        }

        var paginatedEvents = events
            .Skip((search.Page.Value - 1) * search.PageSize.Value)
            .Take(search.PageSize.Value)
            .ToList();

        var res = new DtroEventSearchResult
        {
            TotalCount = events.Count(),
            Events = paginatedEvents,
            Page = search.Page.Value,
            PageSize = Math.Min(search.PageSize.Value, paginatedEvents.Count)
        };

        return res;
    }
}