using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain.Exceptions.BadRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.Dtos.Orders;

namespace Presentation
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController(IServiceManager _serviceManager) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderRequest request)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email);
            var response = await _serviceManager.OrderService.CreateOrderAsync(request, userEmail.Value);

            return Ok(response);
        }

        [HttpGet("deliverymethods")]
        public async Task<IActionResult> GetAllDeliveyMethods()
        {
            var response = await _serviceManager.OrderService.GetAllDeliveryMethodsAsync();
            return Ok(response);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderByIdForSpecificUser(Guid id)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email);
            var response = await _serviceManager.OrderService.GetOrderByIdForSpecificUserAsync(id, userEmail.Value);

            return Ok(response);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetOrdersForSpecificUser()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email);
            var response = await _serviceManager.OrderService.GetOrdersForSpecificUserAsync(userEmail.Value);

            return Ok(response);
        }
    }
}
