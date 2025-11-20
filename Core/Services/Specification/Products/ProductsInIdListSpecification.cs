using Domain.Entities.Products;
using Services.Specification;
using Shared.Dtos.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.C44.G02.Services.Specifications
{
    public class ProductsInIdListSpecification : BaseSpecification<Product, int>
    {
        public ProductsInIdListSpecification(IEnumerable<int> IdList)
            : base(BuildCriteria(IdList))
        {

        }

        private static Expression<Func<Product, bool>> BuildCriteria(IEnumerable<int> IdList)
        {
            return p => IdList.Contains(p.Id);

        }
    }
}
