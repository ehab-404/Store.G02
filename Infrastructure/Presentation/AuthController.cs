using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.Dtos.Auth;

namespace Presentation
{


    [ApiController]

    [Route("api/[controller]")]
    public class AuthController(IServiceManager _serviceManager) : ControllerBase
    {
        //login endpoint



        //register endpoint 



        [HttpPost("login")]

        public async Task<IActionResult> Login (LoginDto loginDto)
        {

            var result = await _serviceManager.AuthService.LoginAsync(loginDto);


            return Ok(result);



        }



        [HttpPost("register")]

        public async Task<IActionResult> Register(RegisterDto _registerDto)
        {

            var result = await _serviceManager.AuthService.RegisterAsync(_registerDto);


            return Ok(result);



        }

    }
}
