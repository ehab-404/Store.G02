using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

//using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Identity;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Services.Abstractions;
using Shared.Dtos.Auth;

using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Shared;

namespace Services.Auth
{
    public class AuthService(UserManager<AppUser> _userManager
       // , IConfiguration _configuration
       , IOptions<JwtOptions> options

        ) : IAuthService
    {
        public async Task<string> GenerateJwtTokenAsync(AppUser user)
        {
            //header



            //payload  : some data   like  claims 



            //signiture


            var jwtoptions = options.Value;


            var authClaims = new List<Claim>() {


                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email)

            };


            var roles = await _userManager.GetRolesAsync(user);

            foreach(var role in roles )
            {
                authClaims.Add(new Claim(ClaimTypes.Role ,role));




            }


            //var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtOptions:SecretKey"]));
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtoptions.SecretKey));


            var token = new JwtSecurityToken(

                //issuer: _configuration["JwtOptions:issuer"],
                issuer: jwtoptions.issuer,
               // audience: _configuration["JwtOptions:audience"],
                audience: jwtoptions.audience,
                claims: authClaims,
              //  expires: DateTime.UtcNow.AddDays(double.Parse(_configuration["JwtOptions:DurationInDays"])),
                expires: DateTime.UtcNow.AddDays(jwtoptions.DurationInDays),
                signingCredentials:new SigningCredentials(secretKey,SecurityAlgorithms.HmacSha256Signature)
                

                );




            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if ( user == null)
            {
                throw new UnAuthorizedException();
                

            }

            var checkPasswordFlag = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!checkPasswordFlag) { throw new UnAuthorizedException(); }





            return new UserResultDto()
            {

                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateJwtTokenAsync(user)


            };
        }

        public async Task<UserResultDto> RegisterAsync(RegisterDto registerDto)
        {

            var user = new AppUser()
            {

                DisplayName = registerDto.DisplayName,
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber



            };

                  var result = await  _userManager.CreateAsync(user, registerDto.Password);


            if(!result.Succeeded)
            {

                var errors = result.Errors.Select(error => error.Description);


                throw new ValidationException(errors);

            }


            return new UserResultDto()
            {

                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateJwtTokenAsync(user)


            };


            throw new NotImplementedException();
        }
    }
}
