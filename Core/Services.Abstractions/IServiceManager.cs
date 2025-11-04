using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Abstractions.Baskets;
using Services.Abstractions.Cashe;
using Services.Abstractions.Products;

namespace Services.Abstractions
{
    public interface IServiceManager
    {
        public IProductService productService { get; }
        public IBasketService basketService { get; }

        public ICasheService casheService { get; }
        public IAuthService AuthService { get; }




    }
}
