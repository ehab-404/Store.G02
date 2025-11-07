using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain.Exceptions.BadRequests;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.Dtos.Orders;

namespace Presentation
{

    [ApiController]

    [Route("api/[controller]")]
    public class OrdersController(IServiceManager _serviceManager) :ControllerBase
    {

        [HttpPost]
        [Route("create")]
        //baseurl/api/orders
        public async Task<IActionResult> CreateOrder(OrderRequest request)
        {
            var UserEmailClaim = User.FindFirst(ClaimTypes.Email);

            if (UserEmailClaim == null) {throw new CreateOrderBadRequest(); }

            var result = await _serviceManager.orderService.CreateOrderAsync(request, UserEmailClaim.Value);

            return Ok(result);




        }




        [HttpGet]
        [Route("deliverymethods")]
        public async Task<IActionResult> GetAllDeliveryMethods()
        {

         var deliveryMethods= await  _serviceManager.orderService.GetDeliveryMethodsAsync();

        if(deliveryMethods == null) { return NotFound("delivery methods not found"); }

        return Ok(deliveryMethods);

        }


        [HttpGet]
        [Route("userorder{id}")]
        public async Task<IActionResult> GetUserOrderById(Guid id , string userEmail)
        {

            if (userEmail == null ) { return BadRequest("user Email is invalid"); }


      var order=   await   _serviceManager.orderService.GetOrderByIdForSpecifiedUser(id, userEmail);

            if(order == null) { return NotFound(); }

            return Ok(order);



        }


        [HttpGet]
        [Route("userorders")]
        public async Task<IActionResult> GetUserOrders( string userEmail)
        {

            if (userEmail == null) { return BadRequest("user Email is invalid"); }


            var orders = await _serviceManager.orderService.GetOrdersForSpecifiedUser( userEmail);

            if (orders == null) { return NotFound(); }

            return Ok(orders);



        }




    }
}
