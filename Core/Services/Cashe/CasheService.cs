using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Services.Abstractions.Cashe;

namespace Services.Cashe
{
    public class CacheService(ICacheRepository cacheRepository) : ICacheService
    {
        public async Task<string?> GetCachedValueAsync(string key)
        {
            var result = await cacheRepository.GetAsync(key);
            return result;

        }

        public async Task SetCachedValueAsync(string key, object value, TimeSpan duration)
        {
            await cacheRepository.SetAsync(key, value, duration);
        }
    }
}
