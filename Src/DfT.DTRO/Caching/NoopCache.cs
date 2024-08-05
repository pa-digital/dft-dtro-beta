namespace DfT.DTRO.Caching;

/// <summary>
/// Class used for unit testing.
/// </summary>
[ExcludeFromCodeCoverage]
public class NoopCache : IRedisCache
{
    /// <inheritdoc cref="IRedisCache"/>
    public Task CacheDtro(Models.DataBase.DTRO dtro)
        => Task.CompletedTask;

    /// <inheritdoc cref="IRedisCache"/>
    public Task CacheDtroExists(Guid key, bool value)
        => Task.CompletedTask;

    /// <inheritdoc cref="IRedisCache"/>
    public Task<Models.DataBase.DTRO> GetDtro(Guid key)
        => Task.FromResult<Models.DataBase.DTRO>(null);

    /// <inheritdoc cref="IRedisCache"/>
    public Task<bool?> GetDtroExists(Guid key)
        => Task.FromResult<bool?>(null);

    /// <inheritdoc cref="IRedisCache"/>
    public Task RemoveDtro(Guid key)
        => Task.CompletedTask;

    /// <inheritdoc cref="IRedisCache"/>
    public Task RemoveDtroIfExists(Guid key)
        => Task.CompletedTask;
}
