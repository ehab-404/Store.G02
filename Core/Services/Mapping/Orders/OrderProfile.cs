using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities.Orders;
using Shared.Dtos.Orders;

namespace Services.Mapping.Orders
{
    public class OrderProfile :Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderAddress, OrderAddressDto>().ReverseMap();

            CreateMap<OrderItem, OrderItemDto>().
                ForMember(d=>d.ProductId, s=>s.MapFrom(p=>p.Id)).
                ForMember(d=>d.ProductName,s=>s.MapFrom(p=>p.product.ProductName)).
                ForMember(d=>d.PictureUrl,s=>s.MapFrom(p=>p.product.PictureUrl))
                ;

            CreateMap<Order, OrderResponse>()
                .ForMember(d=>d.DeliverMethod,d=>d.MapFrom(s=>s.deliveryMethod.ShortName))
                .ForMember(d => d.Total, d => d.MapFrom(s => s.GetTotal()))
                ;


            CreateMap<DeliveryMethod, DeliveryMethodResponse>();
        }

    }
}
