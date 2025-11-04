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
    public class ProductService(IUnitOfWork _unitOfWork , IMapper _mapper) : IProductService
    {

        /*IEnumerable<ProductResponse>*/
        public async Task< PaginationResponse<ProductResponse>> GetAllProductsAsync(ProductQueryParamters queryParamters)
        {

            //var specification = new BaseSpecifications<int, Product>(null);

            //specification.Includes.Add(p => p.Brand);
            //specification.Includes.Add(p => p.Type);

            var specification = new ProductWithBrandAndTypeSpecification(queryParamters);




            var products = await _unitOfWork.GetRepository<int, Product>().GetAllAsync(specification);
            var result = _mapper.Map<IEnumerable<ProductResponse>>(products);

            var totalSpec = new ProductCountSpecifications(queryParamters);

            var count = await _unitOfWork.GetRepository<int, Product>().GetCountAsync(totalSpec);



            return new PaginationResponse<ProductResponse>(queryParamters.pageSize, queryParamters.pageIndex, count, result);
        }

        

        public async Task<ProductResponse> GetProductByIdAsync(int id)
        {
            var specification = new ProductWithBrandAndTypeSpecification(id);


            var product = await _unitOfWork.GetRepository<int, Product>().GetByIdAsync(specification);
            if (product == null) { throw new ProductNotFoundException(id); }

            var result = _mapper.Map<ProductResponse>(product);
            return result;

        }


        public async Task<IEnumerable<BrandResponse>> GetAllBrandsAsync()
        {
            var brands = await _unitOfWork.GetRepository<int, ProductBrand>().GetAllAsync();
            var result = _mapper.Map<IEnumerable<BrandResponse>>(brands);
            return result;

        }


        public async Task<IEnumerable<TypeResponse>> GetAllTypesAsync()
        {
            var types = await _unitOfWork.GetRepository<int, ProductType>().GetAllAsync();
            var result = _mapper.Map<IEnumerable<TypeResponse>>(types);
            return result;

        }


    }
}
