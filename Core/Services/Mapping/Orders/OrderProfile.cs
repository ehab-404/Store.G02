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
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderResponse>()
                .ForMember(or => or.DeliveryMethod, o => o.MapFrom(o => o.DeliveryMethod.ShortName))
                .ForMember(or => or.Total, o => o.MapFrom(o => o.GetTotal()));

            CreateMap<OrderAddress, OrderAddressDto>().ReverseMap();

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(oid => oid.ProductId, o => o.MapFrom(oi => oi.Product.ProductId))
                .ForMember(oid => oid.ProductName, o => o.MapFrom(oi => oi.Product.ProductName))
                .ForMember(oid => oid.PictureUrl, o => o.MapFrom(oi => oi.Product.PictureUrl));

            CreateMap<DeliveryMethod, DeliveryMethodResponse>().ReverseMap();
        }
    }
}
