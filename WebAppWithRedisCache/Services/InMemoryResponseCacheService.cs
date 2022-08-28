using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebAppWithRedisCache.Services
{
    public class InMemoryResponseCacheService : IResponseCacheService
    {
        private readonly IMemoryCache _localCache;

        public InMemoryResponseCacheService(IMemoryCache localCache)
        {
            _localCache = localCache;
        }

        public Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeTimeLive)
        {
            if (response == null)
            {
                return Task.CompletedTask;
            }

            var serializedResponse = JsonConvert.SerializeObject(response);

            var cacheOptions = new MemoryCacheEntryOptions();
            cacheOptions.AbsoluteExpirationRelativeToNow = timeTimeLive;
            cacheOptions.SetSize(1);
            _localCache.Set(cacheKey, serializedResponse, cacheOptions);
            return Task.CompletedTask;
        }

        public Task<string> GetCachedResponseAsync(string cacheKey)
        {
            return Task.FromResult(_localCache.Get<string>(cacheKey));
        }
    }
}
