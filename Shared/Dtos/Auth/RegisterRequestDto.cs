using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Auth
{
    public class RegisterRequestDto
    {
        [Required]
        public string DisplayName { get; set; }

        public string? UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        public string? PhoneNumber { get; set; }

    }
}


