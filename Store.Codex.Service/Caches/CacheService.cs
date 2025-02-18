using StackExchange.Redis;
using Store.Codex.Core.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Codex.Service.Caches
{
    public class CachedService : ICacheService
    {
        private readonly IDatabase _dataBase;

        public CachedService(IConnectionMultiplexer redis)
        {
            _dataBase = redis.GetDatabase();
        }

        public async Task<string> GetCacheKeyAsync(string key)
        {
            var cacheResponse = await _dataBase.StringGetAsync(key);

            if (cacheResponse.IsNullOrEmpty) return null;

            return cacheResponse.ToString();
        }

        public async Task SetCacheKeyAsync(string key, object response, TimeSpan expireTime)
        {
            if (response is null) return;

            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var jsonResponse = JsonSerializer.Serialize(response, options);

            await _dataBase.StringSetAsync(key, jsonResponse, expireTime);
        }
    }
}
