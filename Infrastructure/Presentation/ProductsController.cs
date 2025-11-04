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
    [Authorize]
    [Route("api/[controller]")]

    public class ProductsController(IServiceManager _serviceManager):ControllerBase
    {


        [HttpGet]  // baseurl/api/Products    verb get 
        [Cashe(60)]
        [ProducesResponseType(StatusCodes.Status200OK,Type =typeof(PaginationResponse<ProductResponse>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError,Type =typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest,Type =typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound,Type =typeof(ErrorDetails))]
        public async Task<ActionResult<PaginationResponse<ProductResponse>>> GetAllProductsAsync([FromQuery]ProductQueryParamters queryParamters)
        {


            var result = await _serviceManager.productService.GetAllProductsAsync(queryParamters);



            if(result is null)
            {

                return BadRequest();

            }

            return Ok(result);




        }





        [HttpGet("{id}")]  // baseurl/api/Products/1    verb get 

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]


        public async Task<ActionResult<ProductResponse>> GetProductByIdAsync(int? id)
        {
            if(id is null) { return BadRequest(); }



            var result = await _serviceManager.productService.GetProductByIdAsync(id.Value);



            if (result is null)
            {

                return NotFound();

            }

            return Ok(result);




        }



        [HttpGet("/brands")] // baseurl/api/Products/brands    verb get 

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BrandResponse>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]



        public async Task<ActionResult<IEnumerable<BrandResponse>>> GetAllProductBrandsAsync()
        {

            var result = await _serviceManager.productService.GetAllBrandsAsync();

            if (result is null)
            {

                return NotFound();

            }

            return Ok(result);



        }



        [HttpGet("/types")] // baseurl/api/Products/types    verb get

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TypeResponse>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]



        public async Task<ActionResult<IEnumerable<TypeResponse>>> GetAllProductTypesAsync()
        {

            var result = await _serviceManager.productService.GetAllTypesAsync();

            if (result is null)
            {

                return NotFound();

            }

            return Ok(result);



        }













    }
}
