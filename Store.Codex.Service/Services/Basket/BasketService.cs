using AutoMapper;
using Store.Codex.Core.Dtos.Basket;
using Store.Codex.Core.Entities;
using Store.Codex.Core.Repositories.Contract;
using Store.Codex.Core.Services.Contract;
using Store.Codex.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Codex.Service.Services.Basket
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketService(IBasketRepository basket,IMapper mapper)
        {
            _basketRepository = basket;
            _mapper = mapper;
        }

        public async Task<CustomerBasketDto> GetBasketAsync(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);

            if (basket is null)
            {
                return _mapper.Map<CustomerBasketDto>(new CustomerBasket() { Id = id }  );
            }
            return _mapper.Map<CustomerBasketDto>(basket);
        }

        public async Task<CustomerBasketDto?> UpdateBasketAsync(CustomerBasketDto basketDto)
        {
            var basket = await _basketRepository.UpdateBasketAsync(_mapper.Map<CustomerBasket>(basketDto));

            if (basket is null) return null;

            return _mapper.Map<CustomerBasketDto>(basket);
        }

        public async Task<bool> DeleteBasketAsync(string id)
        {
            return await _basketRepository.DeleteBasketAsync(id);
        }
    }
}
