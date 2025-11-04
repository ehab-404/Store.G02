using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface ICashRepository
    {

        Task<string?> GetAsync(string id);

        Task SetAsync(string id,object value,TimeSpan duration );

    }
}
