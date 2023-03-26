using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace RedisDemo.App.Extensions;

public static class AppDistributedCacheExtensions
{
    public static async Task SetCacheRecordAsync<T>(
        this IDistributedCache cache,
        string keyId,
        T value,
        TimeSpan? absoluteExpireTime = null,
        TimeSpan? unusedExpireTime = null)
    {
        // control how long an entry stays in cache
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteExpireTime
            ?? TimeSpan.FromSeconds(60),
            SlidingExpiration = unusedExpireTime
        };

        string jsonValue = JsonConvert.SerializeObject(value);
        await cache.SetStringAsync(keyId, jsonValue, options);
    }

    public static async Task<T> GetCacheRecordAsync<T>(this IDistributedCache cache, string keyId)
    {
        if (String.IsNullOrEmpty(keyId))
            throw new ArgumentNullException(keyId);

        var value = await cache.GetStringAsync(keyId);

        return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
    }
}