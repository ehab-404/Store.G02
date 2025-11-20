using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.Dtos.Baskets;

namespace Presentation
{

    [ApiController]
    [Route("api/[controller]")]
    public class BasketsController(IServiceManager _serviceManager) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetBasketByIdAsync(string id)
        {
            var result = await _serviceManager.BasketService.GetBasketByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateBasketAsync(CustomerBasketDto basketDto)
        {
            var result = await _serviceManager.BasketService.CreateOrUpdateBasketAsync(basketDto, TimeSpan.FromDays(30));
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBasketAsync(string id)
        {
            await _serviceManager.BasketService.DeleteBasketAsync(id);
            return NoContent();     // 204
        }

    }
}
