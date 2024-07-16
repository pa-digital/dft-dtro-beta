using System;
using System.Threading.Tasks;
using DfT.DTRO.Extensions;
using Microsoft.Extensions.Caching.Distributed;

namespace DfT.DTRO.Caching;

public class RedisCache : IRedisCache
{
    private readonly IDistributedCache _cache;

    public RedisCache(IDistributedCache cache)
    {
        _cache = cache;
    }

    public Task CacheDtro(Models.DataBase.DTRO dtro)
        => _cache.SetValueAsync(dtro.Id.ToString(), dtro);

    public Task CacheDtroExists(Guid key, bool value)
        => _cache.SetBoolAsync(DtroExistsCacheKey(key), value);

    public Task<Models.DataBase.DTRO> GetDtro(Guid key)
        => _cache.GetValueAsync<Models.DataBase.DTRO>(key.ToString());

    public Task<bool?> GetDtroExists(Guid key)
        => _cache.GetBoolAsync(DtroExistsCacheKey(key));

    public Task RemoveDtro(Guid key)
        => _cache.RemoveAsync(key.ToString());

    public Task RemoveDtroIfExists(Guid key)
        => _cache.RemoveAsync(DtroExistsCacheKey(key));

    private static string DtroExistsCacheKey(Guid key)
        => $"exists_{key}";
}
