using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities.Products;
using Domain.Exceptions;
using Services.Abstractions.Products;
using Services.Specification;
using Services.Specification.Products;
using Shared;
using Shared.Dtos.Products;

namespace Services.ProductServices
{
    public class ProductService(IUnitOfWork _unitOfWork, IMapper _mapper) : IProductService
    {

        public async Task<PaginationResponse<ProductResponse>> GetAllProductsAsync(ProductQueryParams productParams)
        {
            // create new specification for products with its brand and category
            var spec = new ProductWithBrandAndTypeSpecification(productParams);

            var products = await _unitOfWork.CreateRepository<Product, int>().GetAllAsync(spec);
            var result = _mapper.Map<IEnumerable<ProductResponse>>(products);

            // get count of filtered products
            var productSpecCount = new ProductCountSepecification(productParams);
            var totalCount = await _unitOfWork.CreateRepository<Product, int>().CountAsync(productSpecCount);

            return new PaginationResponse<ProductResponse>(productParams.PageIndex, productParams.PageSize, totalCount, result);
        }

        public async Task<ProductResponse> GetProductByIdAsync(int id)
        {
            var spec = new ProductWithBrandAndTypeSpecification(id);

            var product = await _unitOfWork.CreateRepository<Product, int>().GetByIdAsync(spec);
            if (product is null)
                throw new ProductNotFoundException(id);

            var result = _mapper.Map<ProductResponse>(product);
            return result;
        }

        public async Task<IEnumerable<TypeBrandResponse>> GetAllBrandsAsync()
        {
            var brands = await _unitOfWork.CreateRepository<ProductBrand, int>().GetAllAsync();
            var result = _mapper.Map<IEnumerable<TypeBrandResponse>>(brands);
            return result;
        }

        public async Task<IEnumerable<TypeBrandResponse>> GetAllTypesAsync()
        {
            var types = await _unitOfWork.CreateRepository<ProductType, int>().GetAllAsync();
            var result = _mapper.Map<IEnumerable<TypeBrandResponse>>(types);
            return result;
        }
    }
}
