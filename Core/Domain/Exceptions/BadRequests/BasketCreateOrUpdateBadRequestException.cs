using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.BadRequests
{
    public  class BasketCreateOrUpdateBadRequestException(string message="Invalid Operation when create or update basket !!") 
        : Exception(message)
    {



    }
}
