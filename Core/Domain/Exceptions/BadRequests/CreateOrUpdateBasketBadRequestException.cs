using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.BadRequests
{
    public  class CreateOrUpdateBasketBadRequestException(string message="Invalid Operation when create or update basket !!") 
        : Exception(message)
    {



    }
}
