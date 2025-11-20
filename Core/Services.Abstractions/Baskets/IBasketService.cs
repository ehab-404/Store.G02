using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Dtos.Baskets;
namespace Services.Abstractions.Baskets
{
    public interface IBasketService
    {
        Task<CustomerBasketDto?> GetBasketByIdAsync(string id);
        Task<CustomerBasketDto?> CreateOrUpdateBasketAsync(CustomerBasketDto basket, TimeSpan duration);
        Task<bool> DeleteBasketAsync(string id);
    }
}
