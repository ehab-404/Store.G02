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
    public class ProductProfile : Profile
    {
        public ProductProfile(IConfiguration configuration)
        {
            CreateMap<Product, ProductResponse>()
                .ForMember(pr => pr.Brand, o => o.MapFrom(p => p.Brand.Name))
                .ForMember(pr => pr.Type, o => o.MapFrom(p => p.Type.Name))
                .ForMember(pr => pr.PictureUrl, o => o.MapFrom(new ProductPictureUrlResolver(configuration)));

            CreateMap<ProductBrand, TypeBrandResponse>();
            CreateMap<ProductType, TypeBrandResponse>();
        }
    }
}
