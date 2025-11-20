using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Attributes;
using Services.Abstractions;
using Shared;
using Shared.Dtos.Products;
using Shared.ErrorModels;
namespace Presentation
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IServiceManager _serviceManager) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationResponse<ProductResponse>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetail))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetail))]
        [Cache(180)]
        public async Task<ActionResult<PaginationResponse<ProductResponse>>> GetAllProducts([FromQuery] ProductQueryParams productParams)
        {
            var result = await _serviceManager.ProductService.GetAllProductsAsync(productParams);

            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetail))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetail))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
        public async Task<ActionResult<ProductResponse>> GetProductById(int id)
        {
            var result = await _serviceManager.ProductService.GetProductByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("brands")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TypeBrandResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetail))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetail))]
        public async Task<ActionResult<TypeBrandResponse>> GetAllBrands()
        {
            var result = await _serviceManager.ProductService.GetAllBrandsAsync();

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("types")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TypeBrandResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetail))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetail))]
        public async Task<ActionResult<TypeBrandResponse>> GetAllTypes()
        {
            var result = await _serviceManager.ProductService.GetAllTypesAsync();

            if (result is null)
                return NotFound();

            return Ok(result);
        }
    }
}
