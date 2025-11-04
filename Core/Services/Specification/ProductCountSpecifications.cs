using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Products;
using Shared.Dtos.Products;

namespace Services.Specification
{
    public class ProductCountSpecifications : BaseSpecifications<int,Product>
    {
        public ProductCountSpecifications(ProductQueryParamters queryParamters):base(

             p => (!queryParamters.brandId.HasValue || p.BrandId == queryParamters.brandId)
            &&

            (!queryParamters.typeId.HasValue || p.TypeId == queryParamters.typeId)
            &&
            (string.IsNullOrEmpty(queryParamters.search) || p.Name.ToLower().Contains(queryParamters.search.ToLower()))



            )
        {


            
        }


    }
}
