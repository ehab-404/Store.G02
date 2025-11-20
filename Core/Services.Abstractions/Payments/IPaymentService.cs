using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Dtos.Baskets;

namespace Services.Abstractions.Payments
{
    public interface IPaymentService
    {
        Task<CustomerBasketDto> CreatePaymentIntentAsync(string basketId);
    }
}
