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
        Console.WriteLine("Search queries:");
        foreach (var query in search.Queries)
        {
            var properties = query.GetType().GetProperties();
            Console.WriteLine(string.Join(", ", properties.Select(p => $"{p.Name}: {p.GetValue(query)}")));
        }
        PaginatedResult<Models.DataBase.DTRO> result = await _dtroService.FindDtrosAsync(search);
        Console.WriteLine($"PaginatedResult: ");
        foreach (var item in result.Results)
        {
            var properties = item.GetType().GetProperties();
            Console.WriteLine(string.Join(", ", properties.Select(p => $"{p.Name}: {p.GetValue(item)}")));
        }
        IEnumerable<DtroSearchResult> mappedResult = _dtroMappingService.MapToSearchResult(result.Results);
        Console.WriteLine("Mapped results: ");
        foreach (var item in mappedResult)
        {
            var properties = item.GetType().GetProperties();
            Console.WriteLine(string.Join(", ", properties.Select(p => $"{p.Name}: {p.GetValue(item)}")));
        }
        PaginatedResponse<DtroSearchResult> paginatedResult =
            new(mappedResult.ToList().AsReadOnly(),
                search.Page,
                result.TotalCount);

        Console.WriteLine("Paginated result: ");
        foreach (var item in paginatedResult.Results)
        {
            var properties = item.GetType().GetProperties();
            Console.WriteLine(string.Join(", ", properties.Select(p => $"{p.Name}: {p.GetValue(item)}")));
        }
        return paginatedResult;
    }
}