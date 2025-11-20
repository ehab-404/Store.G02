using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Abstractions.Baskets;
using Services.Abstractions.Cashe;
using Services.Abstractions.Orders;
using Services.Abstractions.Payments;
using Services.Abstractions.Products;

namespace Services.Abstractions
{
    public interface IServiceManager
    {
        IProductService ProductService { get; }
        IBasketService BasketService { get; }
        ICacheService CacheService { get; }
        IAuthService AuthService { get; }
        IOrderService OrderService { get; }
        IPaymentService PaymentService { get; }
    }
}

