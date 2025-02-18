using Store.Codex.Core.Dtos.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Codex.Core.Services.Contract
{
    public interface IBasketService
    {
        public Task<CustomerBasketDto?> GetBasketAsync(string id);

        public Task<CustomerBasketDto?> UpdateBasketAsync(CustomerBasketDto basketDto);

        Task<bool> DeleteBasketAsync (string id);
    }
}
