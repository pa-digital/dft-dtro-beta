using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace DfT.DTRO.Caching;

/// <summary>
/// Implements <see cref="IRedisCache"/> in a way that
/// does not do anything when writing
/// and always simulates cache misses when reading.
/// </summary>
[ExcludeFromCodeCoverage /* this class is trivial */]
public class NoopCache : IRedisCache
{
    /// <inheritdoc/>
    public Task CacheDtro(Models.DataBase.DTRO dtro)
        => Task.CompletedTask;

    /// <inheritdoc/>
    public Task CacheDtroExists(Guid key, bool value)
        => Task.CompletedTask;

    /// <inheritdoc/>
    public Task<Models.DataBase.DTRO> GetDtro(Guid key)
        => Task.FromResult<Models.DataBase.DTRO>(null);

    /// <inheritdoc/>
    public Task<bool?> GetDtroExists(Guid key)
        => Task.FromResult<bool?>(null);

    /// <inheritdoc/>
    public Task RemoveDtro(Guid key)
        => Task.CompletedTask;

    /// <inheritdoc/>
    public Task RemoveDtroIfExists(Guid key)
        => Task.CompletedTask;
}
