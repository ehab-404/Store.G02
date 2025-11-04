using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities.Products;
using Microsoft.Extensions.Configuration;
using Shared.Dtos.Products;

namespace Services.Mapping.Products
{
    public class ProductPictureUrlResolver(IConfiguration configuration) : IValueResolver<Product, ProductResponse, string>
    {
        public string Resolve(Product source, ProductResponse destination, string destMember, ResolutionContext context)
        {
            
            if(!string.IsNullOrEmpty(source.PictureUrl))
            {

                return $"{configuration["BaseUrl"]}/{source.PictureUrl}";



            }

            return string.Empty;
        }
    }
}
