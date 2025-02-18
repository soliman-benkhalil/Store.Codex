using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Codex.Core.Entities.Order;

namespace Store.Codex.Core.Services.Contract
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string buyerEmail, string baskeId, int deliveryMethod, Address shippingAddress);

        Task<Order?> GetOrderByIdForSpecificUserAsync(string buyerEmail, int orderId);
        Task<IEnumerable<Order?>> GetOrderForSpecificUserAsync(string buyerEmail);
        

    }
}
