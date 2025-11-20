using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.BadRequests
{
    public class BasketDeleteBadRequestException():
        Exception("Invalid operation when delete the basket !!")

    {
    }
}
