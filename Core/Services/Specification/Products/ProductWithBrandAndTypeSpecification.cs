using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Products;
using Shared.Dtos.Products;

namespace Services.Specification.Products
{
    public class ProductWithBrandAndTypeSpecification : BaseSpecification<Product, int>
    {
        public ProductWithBrandAndTypeSpecification(ProductQueryParams productParams)
        : base(BuildCriteria(productParams.BrandId, productParams.TypeId, productParams.Search))
        {
            ApplyIncludes();
            ApplySort(productParams.Sort);
            AddPagination(productParams.PageIndex, productParams.PageSize);
        }

        public ProductWithBrandAndTypeSpecification(int id) : base(p => p.Id == id)
        {
            ApplyIncludes();
        }

        private void ApplyIncludes()
        {
            AddIncludeExp(p => p.Brand);
            AddIncludeExp(p => p.Type);
        }

        private static Expression<Func<Product, bool>> BuildCriteria(int? brandId, int? typeId, string? search)
        {
            return p =>
                (brandId == null || p.BrandId == brandId) &&
                (typeId == null || p.TypeId == typeId) &&
                (string.IsNullOrEmpty(search) || p.Name.ToLower().Contains(search.ToLower()));
        }

        private void ApplySort(string? sort)
        {
            if (string.IsNullOrEmpty(sort))
                AddOrderBy(p => p.Name);
            else
            {
                switch (sort.ToLower())
                {
                    case "priceasc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "pricedesc":
                        AddOrderByDesc(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }
        }

    }
}
