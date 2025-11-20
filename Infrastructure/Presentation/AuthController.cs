using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.Dtos.Auth;

namespace Presentation
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IServiceManager _serviceManager) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var result = await _serviceManager.AuthService.LoginAsync(request);
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            var result = await _serviceManager.AuthService.RegisterAsync(request);
            return Ok(result);
        }

        [HttpGet("EmailExists")]
        public async Task<IActionResult> CheckEmailExists(string email)
        {
            var result = await _serviceManager.AuthService.CheckEmailExistsAsync(email);
            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            var existedUserClaim = User.FindFirst(ClaimTypes.Email);
            if (existedUserClaim is null)
                return Unauthorized();

            var authHeader = HttpContext?.Request.Headers["Authorization"].ToString();
            var currentToken = authHeader?.Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase).Trim();

            var result = await _serviceManager.AuthService.GetCurrentUserAsync(existedUserClaim.Value, currentToken);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("Address")]
        public async Task<IActionResult> GetCurrentUserAddress()
        {
            var existedEmail = User.FindFirst(ClaimTypes.Email);
            if (existedEmail is null)
                return Unauthorized();

            var result = await _serviceManager.AuthService.GetUserAddressAsync(existedEmail.Value);
            return Ok(result);
        }

        [Authorize]
        [HttpPut("Address")]
        public async Task<IActionResult> UpdateCurrentUserAddress(AddressDto addressDto)
        {
            var existedEmail = User.FindFirst(ClaimTypes.Email);
            if (existedEmail is null)
                return Unauthorized();

            var result = await _serviceManager.AuthService.UpdateUserAddressAsync(addressDto, existedEmail.Value);
            return Ok(result);
        }
    }
}
