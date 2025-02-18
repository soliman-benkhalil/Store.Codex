using Store.Codex.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Codex.Core.Repositories.Contract
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> GetBasketAsync(string basketId);
        Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket); // update and create are the same command
        Task<bool> DeleteBasketAsync(string basketId); 


    }
}
