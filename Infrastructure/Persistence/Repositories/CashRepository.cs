using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Contracts;

using StackExchange.Redis;

namespace Persistence.Repositories
{
    public class CashRepository(IConnectionMultiplexer connectionMultiplexer) : ICashRepository
    {

        private readonly IDatabase _database= connectionMultiplexer.GetDatabase();

        public async Task<string?> GetAsync(string id)
        {
            var result =await _database.StringGetAsync(id);
            return result;

        }

        public async Task SetAsync(string id, object value, TimeSpan duration)
        {

            var jsonvalue= JsonSerializer.Serialize(value);


            await _database.StringSetAsync(id, jsonvalue, duration);

        }
    }
}
