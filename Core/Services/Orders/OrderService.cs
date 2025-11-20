using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities.Baskets;
using Domain.Entities.Orders;
using Domain.Entities.Products;
using Domain.Exceptions;
using Domain.Exceptions.BadRequests;
using Services.Abstractions.Orders;
using Services.Specification;
using Services.Specification.Orders;
using Shared.Dtos.Orders;
using Store.C44.G02.Services.Specifications;

namespace Services.Orders
{
    public class OrderService(IUnitOfWork _unitOfWork, IMapper _mapper, IBasketRepository _basketRepository) : IOrderService
    {
        public async Task<OrderResponse> CreateOrderAsync(OrderRequest request, string userEmail)
        {
            // 1. Get Order Address
            var orderAddress = _mapper.Map<OrderAddress>(request.ShipToAddress);


            // 2. Get DeliveryMethod By Id
            var deliveryMethod = await _unitOfWork.CreateRepository<DeliveryMethod, int>().GetByIdAsync(request.DeliveryMethodId);
            if (deliveryMethod is null)
                throw new DeliveryMethodNotFound(request.DeliveryMethodId);



            //3.1 Get Basket by Id
            var basket = await _basketRepository.GetBasketByIdAsync(request.BasketId);
            if (basket is null)
                throw new BasketNotFoundException(request.BasketId);

            // 3.2 Get OrderItems
            List<OrderItem> orderItems = await PopulateOrderItems(basket);


            // 4. Calculate SubTotal
            var subTotal = orderItems.Sum(oi => oi.Price * oi.Quantity);

            // Check for Order with the same paymentIntentId
            var spec = new OrderWithPaymentIdSpecification(basket.PaymentIntentId);
            var existedOrder = await _unitOfWork.CreateRepository<Order, Guid>().GetByIdAsync(spec);

            // delete the existed order with the same paymentIntentId
            if (existedOrder is not null)
                _unitOfWork.CreateRepository<Order, Guid>().Delete(existedOrder);

            // Create Order
            var order = new Order(userEmail, orderAddress, deliveryMethod, orderItems, subTotal, basket.PaymentIntentId);


            // Add Order in DB
            await _unitOfWork.CreateRepository<Order, Guid>().AddAsync(order);
            var count = await _unitOfWork.SaveChangesAsync();
            if (count <= 0)
                throw new OrderCreateBadRequestException();


            return _mapper.Map<OrderResponse>(order);
        }

        public async Task<IEnumerable<DeliveryMethodResponse>> GetAllDeliveryMethodsAsync()
        {
            var deliveryMethods = await _unitOfWork.CreateRepository<DeliveryMethod, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<DeliveryMethodResponse>>(deliveryMethods);
        }

        public async Task<OrderResponse?> GetOrderByIdForSpecificUserAsync(Guid id, string userEmail)
        {
            var spec = new OrderSepecification(id, userEmail);
            var order = await _unitOfWork.CreateRepository<Order, Guid>().GetByIdAsync(spec);

            return _mapper.Map<OrderResponse?>(order);
        }

        public async Task<IEnumerable<OrderResponse>> GetOrdersForSpecificUserAsync(string userEmail)
        {
            var spec = new OrderSepecification(userEmail);
            var orders = await _unitOfWork.CreateRepository<Order, Guid>().GetAllAsync(spec);

            return _mapper.Map<IEnumerable<OrderResponse>>(orders);
        }




        private async Task<List<OrderItem>> PopulateOrderItems(CustomerBasket basket)
        {
            // 3.2 Convert Every Basket Item to Order Item
            var orderItems = new List<OrderItem>();


            // Get all products with Ids == basketItems IDs (to Validate Prices)
            var basketItemsIDs = basket.Items.Select(i => i.Id);

            // add specification with this criteria
            var spec = new ProductsInIdListSpecification(basketItemsIDs);

            var products = await _unitOfWork.CreateRepository<Product, int>().GetAllAsync(spec);


            // I didn't GetProductById inside foreach to reduce DB requests
            foreach (var item in basket.Items)
            {
                // Check Price
                var product = products.FirstOrDefault(p => p.Id == item.Id);
                if (product is null)
                    throw new ProductNotFoundException(item.Id);

                if (product.Price != item.Price)
                    item.Price = product.Price;

                var productInOrderItem = new ProductInOrderItem(item.Id, item.ProductName, item.PictureUrl);
                var orderItem = new OrderItem(productInOrderItem, item.Price, item.Quantity);
                orderItems.Add(orderItem);
            }

            return orderItems;
        }
    }
}
