using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Orders;


namespace Services.Specification
{
    public class OrderWithPaymentIdSpecification : BaseSpecification<Order, Guid>
    {
        public OrderWithPaymentIdSpecification(string paymentIntentId) : base(o => o.PaymentIntentId == paymentIntentId)
        {

        }
    }
}
