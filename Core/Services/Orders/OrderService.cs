using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities.Orders;
using Domain.Entities.Products;
using Domain.Exceptions;
using Domain.Exceptions.BadRequests;
using Services.Abstractions.Orders;
using Services.Specification.Orders;
using Shared.Dtos.Orders;

namespace Services.Orders
{
    public class OrderService(IUnitOfWork _unitOfWork , IMapper _mapper , IBasketRepository _basketRepository) : IOrderService
    {
        public async Task<OrderResponse?> CreateOrderAsync(OrderRequest request, string userEmail)
        {

            //get order address

            var orderAddress = _mapper.Map<OrderAddress>(request.ShipToAddress);


            // get delivery method by id 

            var deliveryMethodRepo = _unitOfWork.GetRepository<int, DeliveryMethod>();

           var deliveryMethod= await deliveryMethodRepo.GetByIdAsync(request.DeliverMethodId);

            if(deliveryMethod is null) { throw new DeliveryMethodNotFoundException(request.DeliverMethodId); }


            // 3. get order items 
            // 3.1  get basket by id 

            var basket= await _basketRepository.GetBasketAsync(request.BasketId);

            if (basket is null) { throw new BasketNotFoundException(request.BasketId); }


            //3.2 convert every basket item to order item
            var orderItems = new List<OrderItem>();

            foreach(var item in basket.Items)
            {
                //check price of product in db 


                //get the product

                var product = await _unitOfWork.GetRepository<int, Product>().GetByIdAsync(item.Id);
                if(product is null ) { throw new ProductNotFoundException(item.Id); }

                //check and re assign

                if(item.Price != product.Price) { item.Price = product.Price; }




                var productInOrderItem = new ProductInOrderItem(item.Id, item.ProductName, item.PictureUrl);

                var OrderItem = new OrderItem(productInOrderItem, item.Price, item.Quantity);

                orderItems.Add(OrderItem);

            }



            //calculate Sub total


         var subtotal=   orderItems.Sum(x => x.Price*x.Quantity);

          var total = subtotal+deliveryMethod.Price;


            //create order 

            var order = new Order(userEmail, orderAddress,deliveryMethod,orderItems,subtotal);


            //add order in db 

            await _unitOfWork.GetRepository<Guid, Order>().AddAsync(order);

            var count = await _unitOfWork.SaveChangesAsync();

            if (count <= 0) { throw new CreateOrderBadRequest(); }




            return _mapper.Map<OrderResponse>(order);   


        }







        public async Task<IEnumerable<DeliveryMethodResponse>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await _unitOfWork.GetRepository<int, DeliveryMethod>().GetAllAsync();
            return _mapper.Map<IEnumerable<DeliveryMethodResponse>>(deliveryMethods);
        }

        public async Task<OrderResponse> GetOrderByIdForSpecifiedUser(Guid Id, string userEmail)
        {
            var specification = new OrderSpecification(Id,userEmail);


            var order =await _unitOfWork.GetRepository<Guid, Order>().GetAllAsync(specification);

            return _mapper.Map<OrderResponse>(order);

        }

        public async Task<IEnumerable<OrderResponse>> GetOrdersForSpecifiedUser(string userEmail)
        {
            var specification = new OrderSpecification(userEmail);


            var order = await _unitOfWork.GetRepository<Guid, Order>().GetAllAsync(specification);

            return _mapper.Map<IEnumerable<OrderResponse>>(order);
        }
    }
}
