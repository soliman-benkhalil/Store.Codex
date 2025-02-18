using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Codex.Core.Entities.Order;

namespace Store.Codex.Core.Specifications.Orders
{
    public class OrderSpecifications : BaseSpecifications<Order,int>
    {
        public OrderSpecifications(string buyerEmail, int orderId) : base(O => O.BuyerEmail == buyerEmail && O.Id == orderId)
        {
            AddInclude();
        }

        public OrderSpecifications(string buyerEmail) : base(O=>O.BuyerEmail ==  buyerEmail )
        {
            AddInclude();
        }


        private void AddInclude()
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
        }


    }
}
