using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Services.Abstractions.Cashe;

namespace Services.Cashe
{
    public class CasheService(ICashRepository _cashRepository) : ICasheService
    {
        public async Task<string?> GetAsync(string key)
        {
            var result = await _cashRepository.GetAsync(key);


            return result;
        }

        public async Task SetAsync(string key, object value, TimeSpan duration)
        {
            await _cashRepository.SetAsync(key,value, duration);


        }
    }
}
