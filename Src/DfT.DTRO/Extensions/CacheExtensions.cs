using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DfT.DTRO.Extensions;

/// <summary>
/// Provides extensions for <see cref="IDistributedCache"/>.
/// </summary>
public static class CacheExtensions
{
    private static readonly TimeSpan _defaultSlidingExpiration = TimeSpan.FromHours(24);

    /// <summary>
    /// Asynchronously sets a value of type <typeparamref name="T"/> in the specified cache with the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the data being stored.</typeparam>
    /// <param name="cache">The cache in which to store the data.</param>
    /// <param name="key">The key to store the data in.</param>
    /// <param name="value">The data to store in the cache.</param>
    /// <param name="slidingExpiration">Specifies how long an entry needs to be inactive before it is evicted.</param>
    /// <param name="cancellationToken">Optional. A <see cref="CancellationToken" /> to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous set operation.</returns>
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

    /// <summary>
    /// Asynchronously sets a <see cref="bool"/> value in the specified cache with the specified key.
    /// </summary>
    /// <param name="cache">The cache in which to store the data.</param>
    /// <param name="key">The key to store the data in.</param>
    /// <param name="value">The data to store in the cache.</param>
    /// <param name="slidingExpiration">Specifies how long an entry needs to be inactive before it is evicted.</param>
    /// <param name="cancellationToken">Optional. A <see cref="CancellationToken" /> to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous set operation.</returns>
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

    /// <summary>
    /// Asynchronously gets a value of type <typeparamref name="T"/> from the specified cache with the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the stored data.</typeparam>
    /// <param name="cache">The cache in which to store the data.</param>
    /// <param name="key">The key to store the data in.</param>
    /// <param name="cancellationToken">Optional. A <see cref="CancellationToken" /> to cancel the operation.</param>
    /// <returns>A task that resolves to the cached value or the default value in case of a cache miss.</returns>
    public static async Task<T> GetValueAsync<T>(this IDistributedCache cache, string key, CancellationToken cancellationToken = default)
    {
        var res = await cache.GetStringAsync(key, cancellationToken);

        return res is null ? default : JsonConvert.DeserializeObject<T>(res);
    }

    /// <summary>
    /// Asynchronously gets a <see cref="bool"/> from the specified cache with the specified key.
    /// </summary>
    /// <param name="cache">The cache in which to store the data.</param>
    /// <param name="key">The key to store the data in.</param>
    /// <param name="cancellationToken">Optional. A <see cref="CancellationToken" /> to cancel the operation.</param>
    /// <returns>A task that resolves to the cached value or <see langword="null"/> in case of a cache miss.</returns>
    public static async Task<bool?> GetBoolAsync(this IDistributedCache cache, string key, CancellationToken cancellationToken = default)
    {
        var res = await cache.GetStringAsync(key, cancellationToken);

        return res is null ? null : bool.Parse(res);
    }
}
