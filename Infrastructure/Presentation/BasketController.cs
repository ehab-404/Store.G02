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


    public class BasketController(IServiceManager _serviceManager):ControllerBase
    {


        [HttpGet]
       public async Task<ActionResult> GetBasketById(string id)
        {


            var basket = await _serviceManager.basketService.GetBasketAsync(id);

            return Ok(basket);


        }


        [HttpPost]
       public async Task<ActionResult> CreateOrUpdateBasket(BasketDto basketDto )
        {

        var result=   await  _serviceManager.basketService.CreateBasketAsync(basketDto , TimeSpan.FromDays(1));

            return Ok(result);
        
        }



        [HttpDelete]
      public  async Task<ActionResult> DeleteBasketById(string id)
        {


            var flag = await _serviceManager.basketService.DeleteBasketAsync(id);

            return NoContent();


        }


    }
}
