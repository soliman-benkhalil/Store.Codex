using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Codex.Core.Dtos.Basket;
using Store.Codex.Core.Entities;
using Store.Codex.Core.Entities.Order;
using Stripe;

namespace Store.Codex.Core.Services.Contract
{
    public interface IPaymentService
    {
        Task<CustomerBasketDto> CreateOrUpdatePaymentIntentIdAsync(string basketId);
        Task<Order> UpdatePaymentIntentForSucceedOrFailed(string paymentIntentId, bool flag);
    }
}
