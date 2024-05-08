using DfT.DTRO.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace DfT.DTRO.Caching;

/// <summary>
/// Provides methods that support caching DTRO extraction API response data using a distributed cache.
/// </summary>
public class RedisCache : IRedisCache
{
    private readonly IDistributedCache _cache;

    /// <summary>
    /// Initializes a new instance of the <see cref="RedisCache"/> class.
    /// </summary>
    /// <param name="cache">An <see cref="IDistributedCache"/> instance.</param>
    public RedisCache(IDistributedCache cache)
    {
        _cache = cache;
    }

    /// <inheritdoc/>
    public Task CacheDtro(Models.DataBase.DTRO dtro)
        => _cache.SetValueAsync(dtro.Id.ToString(), dtro);

    /// <inheritdoc/>
    public Task CacheDtroExists(Guid key, bool value)
        => _cache.SetBoolAsync(DtroExistsCacheKey(key), value);

    /// <inheritdoc/>
    public Task<Models.DataBase.DTRO> GetDtro(Guid key)
        => _cache.GetValueAsync<Models.DataBase.DTRO>(key.ToString());

    /// <inheritdoc/>
    public Task<bool?> GetDtroExists(Guid key)
        => _cache.GetBoolAsync(DtroExistsCacheKey(key));

    /// <inheritdoc/>
    public Task RemoveDtro(Guid key)
        => _cache.RemoveAsync(key.ToString());

    /// <inheritdoc/>
    public Task RemoveDtroIfExists(Guid key)
        => _cache.RemoveAsync(DtroExistsCacheKey(key));

    private static string DtroExistsCacheKey(Guid key)
        => $"exists_{key}";
}
