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
        Task<PaginationResponse<ProductResponse>> GetAllProductsAsync(ProductQueryParams productParams);
        Task<ProductResponse> GetProductByIdAsync(int id);
        Task<IEnumerable<TypeBrandResponse>> GetAllBrandsAsync();
        Task<IEnumerable<TypeBrandResponse>> GetAllTypesAsync();

    }
}
