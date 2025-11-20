using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Identity;
using Shared.Dtos.Auth;

namespace Services.Abstractions
{
    public interface IAuthService
    {
        Task<UserResponse> LoginAsync(LoginRequestDto request);
        Task<UserResponse> RegisterAsync(RegisterRequestDto request);
        Task<bool> CheckEmailExistsAsync(string userEmail);
        Task<UserResponse> GetCurrentUserAsync(string userEmail, string currentUserToken);
        Task<AddressDto> GetUserAddressAsync(string UserEmail);
        Task<AddressDto> UpdateUserAddressAsync(AddressDto address, string userEmail);
    }
}
