using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities.Baskets;
using Domain.Exceptions;
using Domain.Exceptions.BadRequests;
using Services.Abstractions.Baskets;
using Shared.Dtos.Baskets;

namespace Services.Baskets
{
    public class BasketService(IBasketRepository _basketRepository, IMapper mapper) : IBasketService
    {
        public async Task<CustomerBasketDto?> GetBasketByIdAsync(string id)
        {
            var basket = await _basketRepository.GetBasketByIdAsync(id);
            if (basket is null)
                throw new BasketNotFoundException(id);

            var result = mapper.Map<CustomerBasketDto>(basket);

            return result;
        }

        public async Task<CustomerBasketDto?> CreateOrUpdateBasketAsync(CustomerBasketDto basketDto, TimeSpan duration)
        {
            var basket = mapper.Map<CustomerBasket>(basketDto);

            basket = await _basketRepository.CreateOrUpdateBasketAsync(basket, duration);
            if (basket is null)
                throw new BasketCreateOrUpdateBadRequestException();

            var result = mapper.Map<CustomerBasketDto>(basket);

            return result;
        }

        public async Task<bool> DeleteBasketAsync(string id)
        {
            return await _basketRepository.DeleteBasketAsync(id) ? true : throw new BasketDeleteBadRequestException();
        }
    }
}
