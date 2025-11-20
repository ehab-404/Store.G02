using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Services.Abstractions;
using Services.Abstractions.Baskets;
using Services.Abstractions.Cashe;
using Services.Abstractions.Orders;
using Services.Abstractions.Payments;
using Services.Abstractions.Products;
using Services.Auth;
using Services.Baskets;
using Services.Cashe;
using Services.Orders;
using Services.Payments;
using Services.ProductServices;
using Shared;

namespace Services
{
    public class ServiceManager(IUnitOfWork unitOfWork,
          IBasketRepository basketRepository,
          ICacheRepository cacheRepository,
          UserManager<AppUser> userManager,
          IOptions<JwtOptions> options,
          IMapper mapper,
          IConfiguration configuration) : IServiceManager
    {
        public IProductService ProductService { get; } = new ProductService(unitOfWork, mapper);
        public IBasketService BasketService { get; } = new BasketService(basketRepository, mapper);
        public ICacheService CacheService { get; } = new CacheService(cacheRepository);
        public IAuthService AuthService { get; } = new AuthService(userManager, options, mapper);

        public IOrderService OrderService { get; } = new OrderService(unitOfWork, mapper, basketRepository);
        public IPaymentService PaymentService { get; } = new PaymentService(basketRepository, unitOfWork, configuration, mapper);
    }
}
