

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Services.Abstractions;
using Services.Mapping.Auth;
using Services.Mapping.Baskets;
using Services.Mapping.Orders;
using Services.Mapping.Products;
using Shared;

namespace Services
{

    public static class ApplicationServicesRegisteration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddAutoMapper(m => m.AddProfile(new ProductProfile(configuration)));
            services.AddAutoMapper(m => m.AddProfile(new BasketProfile()));
            services.AddAutoMapper(m => m.AddProfile(new OrderProfile()));
            services.AddAutoMapper(m => m.AddProfile(new AddressProfile()));

            return services;
        }
    }
}
