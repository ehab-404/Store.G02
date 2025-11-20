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
using Microsoft.Extensions.Configuration;
using Services.Abstractions.Payments;
using Shared.Dtos.Baskets;
using Store.C44.G02.Services.Specifications;
using Stripe;
using Product = Domain.Entities.Products.Product;

namespace Services.Payments
{

    public class PaymentService(IBasketRepository _basketRepository, IUnitOfWork _unitOfWork, IConfiguration _configuration, IMapper _mapper) : IPaymentService
    {
        public async Task<CustomerBasketDto> CreatePaymentIntentAsync(string basketId)
        {
            //  Get Basket
            var basket = await GetValidBasketAsync(basketId);

            // calculate the total price = subtotla + deliveryMethod cost
            var totalPrice = await CalculateTotalPrice(basket);

            // Set Secret Key (Api Key)
            var paymentIntent = await CreateOrUpdatePaymentIntentAsync(basket, totalPrice);

            // update the current basket with this new data
            basket.PaymentIntentId = paymentIntent.Id;
            basket.ClientSecret = paymentIntent.ClientSecret;

            // Update basket in redis
            basket = await _basketRepository.CreateOrUpdateBasketAsync(basket, TimeSpan.FromDays(1));

            return _mapper.Map<CustomerBasketDto>(basket);
        }

        private async Task<CustomerBasket?> GetValidBasketAsync(string basketId)
        {
            var basket = await _basketRepository.GetBasketByIdAsync(basketId);
            if (basket is null)
                throw new BasketNotFoundException(basketId);
            return basket;
        }

        private async Task<decimal?> CalculateTotalPrice(CustomerBasket basket)
        {
            // Ensure basket item prices are up to date with product prices
            await SyncBasketItemPricesWithProductsPricesAsync(basket);

            // calculate subtotal
            var subTotal = basket.Items.Sum(i => i.Price * i.Quantity);

            // Get DeliveryMethod
            if (basket.DeliveryMethodId is null)
                throw new DeliveryMethodNotFound(-1);

            var deliveryMethod = await _unitOfWork.CreateRepository<DeliveryMethod, int>().GetByIdAsync(basket.DeliveryMethodId.Value);
            if (deliveryMethod is null)
                throw new DeliveryMethodNotFound(basket.DeliveryMethodId.Value);

            basket.ShippingCost = deliveryMethod.Price;

            // calculate Total Price
            var totalPrice = subTotal + basket.ShippingCost;

            return totalPrice;
        }

        private async Task SyncBasketItemPricesWithProductsPricesAsync(CustomerBasket basket)
        {
            var basketItemsIDs = basket.Items.Select(i => i.Id);

            // Query all matching products in a single DB call
            var spec = new ProductsInIdListSpecification(basketItemsIDs);
            var products = await _unitOfWork.CreateRepository<Product, int>().GetAllAsync(spec);

            // Sync item prices to match latest product prices
            foreach (var item in basket.Items)
            {
                var product = products.FirstOrDefault(p => p.Id == item.Id);
                if (product is null)
                    throw new ProductNotFoundException(item.Id);

                if (product.Price != item.Price)
                    item.Price = product.Price;
            }
        }

        private async Task<PaymentIntent> CreateOrUpdatePaymentIntentAsync(CustomerBasket basket, decimal? totalPrice)
        {

            StripeConfiguration.ApiKey = _configuration["StripeOptions:SecretKey"];

            PaymentIntent paymentIntent;

            // Create Or Update PaymentService
            PaymentIntentService paymentService = new PaymentIntentService();

            if (basket.PaymentIntentId is null)
            {
                // Create Payment
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)totalPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }
                };

                paymentIntent = await paymentService.CreateAsync(options);
            }
            else
            {
                // Update Payment
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)totalPrice * 100,
                };

                paymentIntent = await paymentService.UpdateAsync(basket.PaymentIntentId, options);
            }

            return paymentIntent;
        }
    }
}
