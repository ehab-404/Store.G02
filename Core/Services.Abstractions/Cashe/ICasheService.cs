using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions.Cashe
{
    public interface ICasheService
    {

        Task<string?> GetAsync(string key);
        Task SetAsync(string key, object value , TimeSpan duration);

    }
}
