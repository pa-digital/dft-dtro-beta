namespace DfT.DTRO.Services;

/// <inheritdoc cref="ISearchService"/>
public class SearchService : ISearchService
{
    private readonly IDtroService _dtroService;
    private readonly IDtroMappingService _dtroMappingService;
    private readonly ILogger<ISearchService> _logger;

    /// <inheritdoc cref="ISearchService"/>
    public SearchService(IDtroService dtroService, IDtroMappingService dtroMappingService, ILogger<ISearchService> logger)
    {
        _dtroService = dtroService;
        _dtroMappingService = dtroMappingService;
        _logger = logger;
    }

    /// <inheritdoc cref="ISearchService"/>
    public async Task<PaginatedResponse<DtroSearchResult>> SearchAsync(DtroSearch search)
    {
        _logger.LogInformation($"Entering method: {nameof(SearchAsync)}");

        PaginatedResult<Models.DataBase.DTRO> result = await _dtroService.FindDtrosAsync(search);
        _logger.LogInformation($"{string.Join(",", result.Results)}");

        IEnumerable<DtroSearchResult> mappedResult = _dtroMappingService.MapToSearchResult(result.Results);
        _logger.LogInformation($"{string.Join(",", mappedResult)}");

        PaginatedResponse<DtroSearchResult> paginatedResult =
            new(mappedResult.ToList().AsReadOnly(),
                search.Page,
                result.TotalCount);
        _logger.LogInformation($"{string.Join(",", paginatedResult)}");

        return paginatedResult;
    }
}