using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Products;
using Shared.Dtos.Products;

namespace Services.Specification
{
    public class ProductCountSepecification : BaseSpecification<Product, int>
    {
        public ProductCountSepecification(ProductQueryParams productParams)
            : base(BuildCriteria(productParams.BrandId, productParams.TypeId, productParams.Search))
        {

        }

        private static Expression<Func<Product, bool>> BuildCriteria(int? brandId, int? typeId, string? search)
        {
            return p =>
                (brandId == null || p.BrandId == brandId) &&
                (typeId == null || p.TypeId == typeId) &&
                (string.IsNullOrEmpty(search) || p.Name.ToLower().Contains(search.ToLower()));
        }
    }
}
