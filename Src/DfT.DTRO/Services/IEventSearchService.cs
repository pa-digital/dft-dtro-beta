using DfT.DTRO.Models.DtroEvent;
using System.Threading.Tasks;

namespace DfT.DTRO.Services;

/// <summary>
/// An implementation of <see cref="IEventSearchService"/>.
/// </summary>
public interface IEventSearchService
{
    /// <summary>
    /// Searches for events.
    /// </summary>
    /// <param name="search">The values to be used in the query.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous search operation.</returns>
    Task<DtroEventSearchResult> SearchAsync(DtroEventSearch search);
}