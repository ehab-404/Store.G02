using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Orders
{

    //table
    public class Order:BaseEntity<Guid>
    {


        // must for ef core mapping 
        public Order()
        {
        }


        public Order(string userEmail, OrderAddress shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subTotal)
        {
            UserEmail = userEmail;
            ShippingAddress = shippingAddress;
            this.deliveryMethod = deliveryMethod;
            this.items = items;
            SubTotal = subTotal;
        }

        public string UserEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

        public OrderStatus status { get; set; } = OrderStatus.Pending;

        public OrderAddress ShippingAddress { get; set; }


        public int DeliveryMethodId { get; set; }
        public DeliveryMethod deliveryMethod { get; set; } //np 


        public ICollection<OrderItem> items { get; set; } //np


        public decimal SubTotal { get; set; }

        public decimal GetTotal() => SubTotal + deliveryMethod.Price;
        


    }
}
