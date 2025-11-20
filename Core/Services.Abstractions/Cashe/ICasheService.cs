using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions.Cashe
{
    public interface ICacheService
    {
        Task<string?> GetCachedValueAsync(string key);
        Task SetCachedValueAsync(string key, object value, TimeSpan duration);
    }
}
