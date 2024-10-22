namespace DfT.DTRO.Services;

public class SearchService : ISearchService
{
    private readonly IDtroService _dtroService;
    private readonly IDtroMappingService _dtroMappingService;

    public SearchService(IDtroService dtroService, IDtroMappingService dtroMappingService)
    {
        _dtroService = dtroService;
        _dtroMappingService = dtroMappingService;
    }

    public async Task<PaginatedResponse<DtroSearchResult>> SearchAsync(DtroSearch search)
    {
        PaginatedResult<Models.DataBase.DTRO> result = await _dtroService.FindDtrosAsync(search);
        IEnumerable<DtroSearchResult> mappedResult = _dtroMappingService.MapToSearchResult(result.Results);
        PaginatedResponse<DtroSearchResult> paginatedResult =
            new(mappedResult.ToList().AsReadOnly(),
                search.Page,
                result.TotalCount);
        return paginatedResult;
    }
}