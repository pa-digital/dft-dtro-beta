using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DfT.DTRO.Models.Filtering;
using DfT.DTRO.Models.Pagination;
using DfT.DTRO.Models.Search;
using DfT.DTRO.Services.Mapping;

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
        var result = await _dtroService.FindDtrosAsync(search);

        IEnumerable<DtroSearchResult> mappedResult = _dtroMappingService.MapToSearchResult(result.Results);

        PaginatedResponse<DtroSearchResult> paginatedResult =
            new(mappedResult.ToList().AsReadOnly(),
                search.Page,
                result.TotalCount);

        return paginatedResult;
    }
}