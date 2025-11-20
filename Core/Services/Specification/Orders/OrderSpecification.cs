using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Orders;

namespace Services.Specification.Orders
{
    public class OrderSepecification : BaseSpecification<Order, Guid>
    {
        // a specific order for specific user
        public OrderSepecification(Guid id, string userEmail)
            : base(o => o.Id == id && o.UserEmail.ToLower() == userEmail.ToLower())
        {
            AddIncludeExp(o => o.Items);
            AddIncludeExp(o => o.DeliveryMethod);
        }

        // all orders for specific user
        public OrderSepecification(string userEmail)
            : base(o => o.UserEmail.ToLower() == userEmail.ToLower())
        {
            AddIncludeExp(o => o.Items);
            AddIncludeExp(o => o.DeliveryMethod);
            AddOrderByDesc(o => o.OrderDate);
        }
    }
}
