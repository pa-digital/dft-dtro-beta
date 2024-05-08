using DfT.DTRO.Models.Filtering;
using DfT.DTRO.Models.Pagination;
using DfT.DTRO.Models.Search;
using System.Threading.Tasks;

namespace DfT.DTRO.Services;
public interface ISearchService
{
    /// <summary>
    /// Searches dtros.
    /// </summary>
    /// <param name="search">The values to be used in the query.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous search operation.</returns>
    Task<PaginatedResponse<DtroSearchResult>> SearchAsync(DtroSearch search);
}