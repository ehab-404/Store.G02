using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Orders;

namespace Services.Specification.Orders
{
    public class OrderSpecification :BaseSpecifications<Guid,Order>
    {
        public OrderSpecification(Guid Id ,string userEmail):base(o=>o.Id==Id && o.UserEmail.ToLower()==userEmail.ToLower())
        {

            Includes.Add(o => o.deliveryMethod);
            Includes.Add(o => o.items);


        }
        
        public OrderSpecification(string userEmail):base(o=> o.UserEmail.ToLower()==userEmail.ToLower())
        {

            Includes.Add(o => o.deliveryMethod);
            Includes.Add(o => o.items);
            AddOrderByDescinding(o => o.OrderDate);

        }


    }
}
