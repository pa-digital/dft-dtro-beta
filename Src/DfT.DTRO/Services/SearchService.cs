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
        Console.WriteLine("Search queries:\t" + string.Join("-", search.Queries));
        PaginatedResult<Models.DataBase.DTRO> result = await _dtroService.FindDtrosAsync(search);
        Console.WriteLine("Result:\t" + string.Join("-", result));

        IEnumerable<DtroSearchResult> mappedResult = _dtroMappingService.MapToSearchResult(result.Results);
        Console.WriteLine("Mapped result:\t" + string.Join("-", mappedResult));

        PaginatedResponse<DtroSearchResult> paginatedResult =
            new(mappedResult.ToList().AsReadOnly(),
                search.Page,
                result.TotalCount);

        Console.WriteLine("Paginated result:\t" + string.Join("-", paginatedResult));
        return paginatedResult;
    }
}