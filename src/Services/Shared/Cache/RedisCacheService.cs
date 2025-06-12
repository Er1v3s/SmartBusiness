using Shared.Abstracts;
using StackExchange.Redis;

namespace Shared.Cache
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDatabase _cacheDatabase;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _cacheDatabase = redis.GetDatabase();
        }

        public async Task<string?> GetCachedValueAsync(string key)
        {
            Console.WriteLine($"Get cached values from key: {key}");
            return await _cacheDatabase.StringGetAsync(key);
        }

        public async Task SetCacheValueAsync(string key, string value, TimeSpan? expiration = null)
        {
            await _cacheDatabase.StringSetAsync(key, value, expiration ?? TimeSpan.FromMinutes(10));
        }

        public async Task RemoveAsync(string key)
        {
            Console.WriteLine($"Removing cache for key: {key}");
            await _cacheDatabase.KeyDeleteAsync(key);
        }
    }
}
