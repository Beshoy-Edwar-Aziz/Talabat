using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Services;

namespace Talabat.Services
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDatabase _database;
        public ResponseCacheService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task<string?> GetResponseCacheAsync(string CacheKey)
        {
            var Response =await _database.StringGetAsync(CacheKey);
            if (Response.IsNullOrEmpty) return null;
            return Response;
        }

        public async Task ResponseCacheAsync(string CacheKey, object Response, TimeSpan TimeToLive)
        {
            if (Response is null) return;
            var SerializedResponseOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var SerializedResponse = JsonSerializer.Serialize(Response,SerializedResponseOptions); ;
            await _database.StringSetAsync(CacheKey, SerializedResponse, TimeToLive);
        }
    }
}
