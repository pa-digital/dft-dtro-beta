namespace DfT.DTRO.Caching;

/// <summary>
/// Service implementation.
/// </summary>
public class RedisCache : IRedisCache
{
    private readonly IDistributedCache _cache;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="cache">distributed cache.</param>
    public RedisCache(IDistributedCache cache) => _cache = cache;

    /// <inheritdoc cref="IRedisCache"/>
    public Task CacheDtro(Models.DataBase.DTRO dtro)
        => _cache.SetValueAsync(dtro.Id.ToString(), dtro);

    /// <inheritdoc cref="IRedisCache"/>
    public Task CacheDtroExists(Guid key, bool value)
        => _cache.SetBoolAsync(DtroExistsCacheKey(key), value);

    /// <inheritdoc cref="IRedisCache"/>
    public Task<Models.DataBase.DTRO> GetDtro(Guid key)
        => _cache.GetValueAsync<Models.DataBase.DTRO>(key.ToString());

    /// <inheritdoc cref="IRedisCache"/>
    public Task<bool?> GetDtroExists(Guid key)
        => _cache.GetBoolAsync(DtroExistsCacheKey(key));

    /// <inheritdoc cref="IRedisCache"/>
    public Task RemoveDtro(Guid key)
        => _cache.RemoveAsync(key.ToString());

    /// <inheritdoc cref="IRedisCache"/>
    public Task RemoveDtroIfExists(Guid key)
        => _cache.RemoveAsync(DtroExistsCacheKey(key));

    private static string DtroExistsCacheKey(Guid key)
        => $"exists_{key}";
}
