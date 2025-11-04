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
    public class ProductWithBrandAndTypeSpecification:BaseSpecifications<int,Product>
    {
        public ProductWithBrandAndTypeSpecification(/*Expression<Func<Product,bool>> expression*/
            ):base(/*expression*/ null)
        {

            AppliedIncludes();




        }


        public ProductWithBrandAndTypeSpecification(ProductQueryParamters queryParamters) : base(

            p => (!queryParamters.brandId.HasValue || p.BrandId==queryParamters.brandId )
            &&
            
            (!queryParamters.typeId.HasValue ||  p.TypeId== queryParamters.typeId)
            &&
            ( string.IsNullOrEmpty(queryParamters.search)  ||  p.Name.ToLower().Contains(queryParamters.search.ToLower()) )


            )
        {


            ApplyPagination(queryParamters.pageIndex,queryParamters.pageSize);

            ApplySorting(queryParamters.sort);

                AppliedIncludes();




        }


       

        public ProductWithBrandAndTypeSpecification(int id ) : base(p=>p.Id==id)
        {


            AppliedIncludes();



        }

        private void AppliedIncludes()
        {

            Includes.Add(p => p.Brand);
            Includes.Add(p => p.Type);


        }

        private void ApplySorting(string? sort)
        {


            if (!string.IsNullOrEmpty(sort))
            {

                sort = sort.ToLower();

                if (sort == "priceasc") { AddOrderBy(p => p.Price); }
                else if (sort == "pricedesc") { AddOrderByDescinding(p => p.Price); }
                else { AddOrderBy(p => p.Name); }

            }

            else
            {

                // OrderBy = p => p.Name;
                AddOrderBy(p => p.Name);

            }
        }

    }
}
