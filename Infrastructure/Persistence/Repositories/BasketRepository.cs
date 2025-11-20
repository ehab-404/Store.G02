using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Entities.Baskets;
using StackExchange.Redis;

namespace Persistence.Repositories
{
    public class BasketRepository(IConnectionMultiplexer connection) : IBasketRepository
    {
        private readonly IDatabase _database = connection.GetDatabase();

        public async Task<CustomerBasket?> GetBasketByIdAsync(string id)
        {
            var redisValue = await _database.StringGetAsync(id);
            if (redisValue.IsNullOrEmpty)
                return null;

            var basket = JsonSerializer.Deserialize<CustomerBasket>(redisValue);
            if (basket is null)
                return null;

            return basket;
        }

        public async Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan duration)
        {
            var redisValue = JsonSerializer.Serialize(basket);

            var flag = await _database.StringSetAsync(basket.Id, redisValue, duration);

            return flag ? await GetBasketByIdAsync(basket.Id) : null;
        }

        public async Task<bool> DeleteBasketAsync(string id)
        {
            return await _database.KeyDeleteAsync(id);
        }
    }
}

