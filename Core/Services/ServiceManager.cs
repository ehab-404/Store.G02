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
using Services.Abstractions.Products;
using Services.Auth;
using Services.Baskets;
using Services.Cashe;
using Services.Orders;
using Services.ProductServices;
using Shared;

namespace Services
{
    public class ServiceManager(IUnitOfWork _unitOfWork , IMapper _mapper,
        IBasketRepository _basketRepository , ICashRepository _cashRepository
        ,UserManager<AppUser> _userManager
       // , IConfiguration _configuration
       ,IOptions<JwtOptions> options


        ) :
        IServiceManager
    {
        public IProductService productService { get; } = new ProductService(_unitOfWork, _mapper);

        public IBasketService basketService { get; } = new BasketService( _basketRepository,_mapper);

        public ICasheService casheService { get; } = new CasheService(_cashRepository);
        public IAuthService AuthService { get; } = new AuthService(_userManager   /*_configuration*/ , options );

        public IOrderService orderService { get; } = new OrderService(_unitOfWork, _mapper, _basketRepository);
    }
}
