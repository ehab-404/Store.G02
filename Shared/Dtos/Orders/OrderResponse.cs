using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Orders
{
    public class OrderResponse
    {

        public Guid Id { get; set; }
        public string UserEmail { get; set; }

        public OrderAddressDto ShippngAddress { get; set; }


        public string DeliverMethod { get; set; }


        public ICollection<OrderItemDto> items { get; set; }

        public decimal SubTotal {  get; set; }
        public decimal Total {  get; set; }




        public string OrderStatus { get; set; }



    }
}
