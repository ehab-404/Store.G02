using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.BadRequests
{
    public class DeliveryMethodNotFound(int id):NotFoundException($"delivery method with id {id} was not found ")
    {
    }
}
