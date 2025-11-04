using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstractions;
using Services.Mapping.Baskets;
using Services.Mapping.Products;
using Shared;

namespace Services
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration configuration)
        {


            services.AddAutoMapper(m => m.AddProfile(new ProductProfile(configuration)));
           
            
            services.AddAutoMapper(m => m.AddProfile(new BasketProfile()));
           
            
            //services.AddAutoMapper(typeof(AssemblyReference).Assembly);

            services.AddScoped<IServiceManager, ServiceManager>();


            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));

            return services;

        }

    }
}
