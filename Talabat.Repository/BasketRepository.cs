using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositiories;

namespace Talabat.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer Redis) 
        {
            _database = Redis.GetDatabase();
        }
        public async Task<bool> DeleteBasketAsync(string BasketId)
        =>  await _database.KeyDeleteAsync(BasketId);
        

        public async Task<CustomerBasket?> GetBasketAsync(string BasketId)
        {
            var Basket = await _database.StringGetAsync(BasketId);
            if (Basket.IsNull)
            {
                return null;
            }
            else
            {
                return JsonSerializer.Deserialize<CustomerBasket> (Basket);
            }
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket Basket)
        {
            var JsonSerialize = JsonSerializer.Serialize<CustomerBasket>(Basket);
            var basket = await _database.StringSetAsync(Basket.Id, JsonSerialize,TimeSpan.FromDays(2));
            if (!basket)
            {
                return null;
            }
            else
            {
                return await GetBasketAsync(Basket.Id);
            }
        }
    }
}
