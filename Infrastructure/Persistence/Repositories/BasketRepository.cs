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
        private readonly IDatabase database = connection.GetDatabase();




        public async Task<CustomerBasket?> CreateBasketAsync(CustomerBasket basket, TimeSpan duration)
        {
            var redisValue = JsonSerializer.Serialize(basket);

            var flag = await database.StringSetAsync(basket.Id, redisValue, duration);

            if (!flag) { return null; }

            return await GetBasketAsync(basket.Id);


        }

        public async Task<bool> DeleteBasketAsync(string id)
        {

            var flag = await database.KeyDeleteAsync(id);
            return flag;

        }

        public async Task<CustomerBasket?> GetBasketAsync(string id)
        {
            var redisValue = await database.StringGetAsync(id);

            if(redisValue.IsNullOrEmpty) { return null; }


           var basket = JsonSerializer.Deserialize<CustomerBasket>(redisValue);

            if(basket is null) { return null; }

            return basket;



        }
    }
}
