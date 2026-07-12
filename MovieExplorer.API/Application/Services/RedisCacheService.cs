using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using MovieExplorer.API.Core.Interfaces;

namespace MovieExplorer.API.Infrastructure.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var cachedValue = await _cache.GetStringAsync(key);

            if (string.IsNullOrEmpty(cachedValue))
            {
                Console.WriteLine($"Redis MISS : {key}");
                return default;
            }

            Console.WriteLine($"Redis HIT : {key}");

            return JsonSerializer.Deserialize<T>(cachedValue);
        }

        public async Task SetAsync<T>(
            string key,
            T value,
            TimeSpan expiration)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            };

            var json = JsonSerializer.Serialize(value);

            Console.WriteLine($"Redis SET : {key}");

            await _cache.SetStringAsync(
                key,
                json,
                options);
        }

        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}