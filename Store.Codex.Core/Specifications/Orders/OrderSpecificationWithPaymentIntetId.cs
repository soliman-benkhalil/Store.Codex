using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using Store.Codex.Core.Entities.Order;

namespace Store.Codex.Core.Specifications.Orders
{
    public class OrderSpecificationWithPaymentIntetId : BaseSpecifications<Entities.Order.Order,int>
    {
        public OrderSpecificationWithPaymentIntetId(String paymentIntentId) : base(O => O.PaymentIntentId == paymentIntentId)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
        }
    }
}
