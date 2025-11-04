using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Products;
using Shared;
using Shared.Dtos.Products;

namespace Services.Abstractions.Products
{
    public interface IProductService
    {

        Task</*IEnumerable<ProductResponse>*/ PaginationResponse<ProductResponse>> GetAllProductsAsync(ProductQueryParamters queryParamters);

        Task<ProductResponse> GetProductByIdAsync(int id);

        Task<IEnumerable<BrandResponse>> GetAllBrandsAsync();

        Task<IEnumerable<TypeResponse>> GetAllTypesAsync();


    }
}
