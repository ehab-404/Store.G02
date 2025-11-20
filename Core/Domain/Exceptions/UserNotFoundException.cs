using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Exceptions;

namespace Store.C44.G02.Domain.Exceptions
{
    public class UserNotFoundException(string userEmail) : NotFoundException($"User with email: {userEmail} was not found")
    {
    }
}
