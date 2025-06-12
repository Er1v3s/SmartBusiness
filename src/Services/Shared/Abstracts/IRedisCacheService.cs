namespace Shared.Abstracts
{
    public interface IRedisCacheService
    {
        Task<string?> GetCachedValueAsync(string key);
        Task SetCacheValueAsync(string key, string value, TimeSpan? expiration = null);
        Task RemoveAsync(string key);
    }
}
