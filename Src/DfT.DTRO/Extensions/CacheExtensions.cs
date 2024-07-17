using System.Threading;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace DfT.DTRO.Extensions;

public static class CacheExtensions
{
    private static readonly TimeSpan _defaultSlidingExpiration = TimeSpan.FromHours(24);

    public static Task SetValueAsync<T>(this IDistributedCache cache, string key, T value, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default)
    {
        var serialized = JsonConvert.SerializeObject(value);

        return cache.SetStringAsync(
            key,
            serialized,
            new DistributedCacheEntryOptions
            {
                SlidingExpiration = slidingExpiration ?? _defaultSlidingExpiration,
            },
            cancellationToken);
    }

    public static Task SetBoolAsync(this IDistributedCache cache, string key, bool value, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default)
    {
        var serialized = value.ToString().ToLowerInvariant();

        return cache.SetStringAsync(
            key,
            serialized,
            new DistributedCacheEntryOptions
            {
                SlidingExpiration = slidingExpiration ?? _defaultSlidingExpiration,
            },
            cancellationToken);
    }

    public static async Task<T> GetValueAsync<T>(this IDistributedCache cache, string key, CancellationToken cancellationToken = default)
    {
        var res = await cache.GetStringAsync(key, cancellationToken);

        return res is null ? default : JsonConvert.DeserializeObject<T>(res);
    }

    public static async Task<bool?> GetBoolAsync(this IDistributedCache cache, string key, CancellationToken cancellationToken = default)
    {
        var res = await cache.GetStringAsync(key, cancellationToken);

        return res is null ? null : bool.Parse(res);
    }
}
