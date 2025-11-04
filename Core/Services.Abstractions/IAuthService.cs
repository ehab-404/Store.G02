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
      Task<UserResultDto>   LoginAsync(LoginDto loginDto);
      Task<UserResultDto>   RegisterAsync(RegisterDto registerDto);


      Task<string> GenerateJwtTokenAsync(AppUser user);

    }
}
