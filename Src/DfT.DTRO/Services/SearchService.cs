using DfT.DTRO.Models.Filtering;
using DfT.DTRO.Models.Pagination;
using DfT.DTRO.Models.Search;
using DfT.DTRO.Services.Mapping;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DfT.DTRO.Services;

/// <summary>
/// An implementation of <see cref="ISearchService"/>
/// </summary>
public class SearchService : ISearchService
{
    private readonly IDtroService _dtroService;
    private readonly IDtroMappingService _dtroMappingService;

    /// <summary>
    /// The default constructor.
    /// </summary>
    /// <param name="dtroService">An <see cref="IDtroService"/> instance.</param>
    /// <param name="dtroMappingService">An <see cref="IDtroMappingService"/> instance.</param>
    public SearchService(IDtroService dtroService, IDtroMappingService dtroMappingService)
    {
        _dtroService = dtroService;
        _dtroMappingService = dtroMappingService;
    }

    /// <inheritdoc/>
    public async Task<PaginatedResponse<DtroSearchResult>> SearchAsync(DtroSearch search)
    {
        var result = await _dtroService.FindDtrosAsync(search);

        IEnumerable<DtroSearchResult> mappedResult = _dtroMappingService.MapToSearchResult(result.Results);

        PaginatedResponse<DtroSearchResult> paginatedResult =
            new (mappedResult.ToList().AsReadOnly(),
                search.Page,
                result.TotalCount);

        return paginatedResult;
    }
}