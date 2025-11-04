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
    public class ProductProfile :Profile
    {

        

        public ProductProfile(IConfiguration configuration)
        {


            var baseUrl = configuration["BaseUrl"];


            CreateMap<Product, ProductResponse>()
                .ForMember(d=>d.Brand, o => o.MapFrom(s => s.Brand.Name))
                .ForMember(p => p.Type, o => o.MapFrom(s => s.Type.Name))
                .ForMember(D=>D.PictureUrl,s=>s.MapFrom(new ProductPictureUrlResolver(configuration)));



            // .ForMember(D=>D.PictureUrl,s=>s.MapFrom(s => $"{baseUrl }/{s.PictureUrl}"));


            CreateMap<ProductBrand, BrandResponse>();
            CreateMap<ProductType, TypeResponse>();




        }

    }
}
