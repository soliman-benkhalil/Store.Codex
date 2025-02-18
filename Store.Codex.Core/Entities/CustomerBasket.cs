using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Codex.Core.Entities
{
    public class CustomerBasket
    {
        public string Id { get; set; }
        public List<BasketItem> Items { get; set; }
        public int? DevliveryMethodId { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClinetSecret { get; set; } // Token of the user
    }
}
