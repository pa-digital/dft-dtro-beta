using DfT.DTRO.Models.Filtering;
using DfT.DTRO.Models.Pagination;
using DfT.DTRO.Models.Search;

namespace DfT.DTRO.Services;
public interface ISearchService
{
    Task<PaginatedResponse<DtroSearchResult>> SearchAsync(DtroSearch search);
}