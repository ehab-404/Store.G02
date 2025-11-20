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
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Store.C44.G02.Domain.Exceptions;

namespace Services.Auth
{
    public class AuthService(UserManager<AppUser> _userManager,
        IOptions<JwtOptions> options,
        IMapper _mapper) : IAuthService
    {
        public async Task<UserResponse> LoginAsync(LoginRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                throw new NotAuthorizedException();

            var isValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isValidPassword)
                throw new NotAuthorizedException();

            return new UserResponse()
            {
                DisplayName = user.DiplayName,
                Email = user.Email,
                Token = await GenerateJwtTokenAsync(user)
            };
        }

        public async Task<UserResponse> RegisterAsync(RegisterRequestDto request)
        {
            var user = new AppUser()
            {
                DiplayName = request.DisplayName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                UserName = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                throw new ValidationException(result.Errors.Select(e => e.Description));

            return new UserResponse()
            {
                DisplayName = user.DiplayName,
                Email = user.Email,
                Token = await GenerateJwtTokenAsync(user)
            };
        }

        public async Task<bool> CheckEmailExistsAsync(string userEmail)
        {
            return await _userManager.FindByEmailAsync(userEmail) is not null;
        }

        public async Task<UserResponse> GetCurrentUserAsync(string userEmail, string currentUesrToken)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user is null)
                throw new UserNotFoundException(userEmail);

            return new UserResponse()
            {
                DisplayName = user.DiplayName,
                Email = userEmail,
                Token = currentUesrToken!
            };
        }

        public async Task<AddressDto> GetUserAddressAsync(string userEmail)
        {
            var user = await _userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email.ToLower() == userEmail.ToLower());
            if (user is null)
                throw new UserNotFoundException(userEmail);

            return _mapper.Map<AddressDto>(user.Address);
        }

        public async Task<AddressDto> UpdateUserAddressAsync(AddressDto address, string userEmail)
        {
            var user = await _userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email.ToLower() == userEmail.ToLower());
            if (user is null)
                throw new UserNotFoundException(userEmail);

            if (user.Address is null)
            {
                // Createing new Address
                user.Address = _mapper.Map<Address>(address);
            }
            else
            {
                // Updating existed Address
                user.Address.FirstName = address.FirstName;
                user.Address.LastName = address.LastName;
                user.Address.Country = address.Country;
                user.Address.City = address.City;
                user.Address.Street = address.Street;
            }


            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new UpdateUserAddressBadRequestException();

            return _mapper.Map<AddressDto>(user.Address);
        }


        private async Task<string> GenerateJwtTokenAsync(AppUser user)
        {
            var jwtOptions = options.Value;

            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var jwtSecretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey));

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: authClaims,
                expires: DateTime.UtcNow.AddDays(jwtOptions.ExpireInDays),
                signingCredentials: new SigningCredentials(jwtSecretKey, SecurityAlgorithms.HmacSha256Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
