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
    public class BasketService(IBasketRepository _basketRepository , IMapper _mapper) : IBasketService
    {
        public async Task<BasketDto> GetBasketAsync(string id)
        {

            var basket = await _basketRepository.GetBasketAsync(id);
             
            if(basket is null) { throw new BasketNotFoundException(id); }


            var result = _mapper.Map<BasketDto>(basket);

           

            return result;

        }

        public async Task<BasketDto> CreateBasketAsync(BasketDto basket, TimeSpan duration)
        {
            var customerbasket = _mapper.Map<CustomerBasket>(basket);

          var result = await _basketRepository.CreateBasketAsync(customerbasket, duration);


            if(result is null) { throw new CreateOrUpdateBasketBadRequestException(); }

            return _mapper.Map<BasketDto>(result);



        }

        public async Task<bool> DeleteBasketAsync(string id)
        {

            var flag = await _basketRepository.DeleteBasketAsync(id);

            if (!flag) { throw new DeleteBasketBadRequestException(); }

            return flag;

        }



        
    }
}
