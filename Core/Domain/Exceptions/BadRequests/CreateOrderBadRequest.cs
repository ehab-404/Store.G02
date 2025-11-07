using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.BadRequests
{
    public class CreateOrderBadRequest():BadRequestException("Invalid Operation when create order")
    {
    }
}
