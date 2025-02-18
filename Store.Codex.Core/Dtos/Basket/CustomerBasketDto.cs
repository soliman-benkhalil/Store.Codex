using Store.Codex.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Store.Codex.Core.Dtos.Basket
{
    public class CustomerBasketDto
    {
        public string Id { get; set; }
        public List<BasketItem> Items { get; set; }
        [JsonPropertyName("deliveryMethodId")]
        public int? DevliveryMethodId { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClinetSecret { get; set; }

    }
}
