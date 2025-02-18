using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Store.Codex.Core;
using Store.Codex.Core.Dtos.Basket;
using Store.Codex.Core.Entities;
using Store.Codex.Core.Entities.Order;
using Store.Codex.Core.Repositories.Contract;
using Store.Codex.Core.Services.Contract;
using Store.Codex.Core.Specifications.Orders;
using Stripe;

using Product = Store.Codex.Core.Entities.Product;
namespace Store.Codex.Service.Payments
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketService _basketService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public PaymentService(IBasketService basketService, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _basketService = basketService;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<CustomerBasketDto> CreateOrUpdatePaymentIntentIdAsync(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["Stripe:Secret key"];

            // Get Basket 
            var basket = await _basketService.GetBasketAsync(basketId);
            if (basket is null) return null;

            var shippingPrice = 0m;

            if (basket.DevliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetAsync(basket.DevliveryMethodId.Value);
                shippingPrice = deliveryMethod.Cost;
            }

            if(basket.Items.Count() > 0)
            {
                foreach(var item in basket.Items)
                {
                    var product = _unitOfWork.Repository<Product, int>().GetAsync(item.Id);
                    if(item.Price !=  product.Result.Price)
                    {
                        item.Price = product.Result.Price;
                    }
                }
                
            }

            var subTotoal = basket.Items.Sum(I => I.Price * I.Quantity);

            var service = new PaymentIntentService();


            PaymentIntent paymentIntent;

            if(string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                // Create
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)(subTotoal * 100 + shippingPrice * 100),
                    PaymentMethodTypes = new List<string>() { "card" },
                    Currency = "usd"
                };
                paymentIntent = await service.CreateAsync(options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClinetSecret = paymentIntent.ClientSecret;

            }
            else
            {
                // Update 
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)(subTotoal * 100 + shippingPrice * 100),
                };
                paymentIntent = await service.UpdateAsync(basket.PaymentIntentId,options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClinetSecret = paymentIntent.ClientSecret;
            }

            basket = await _basketService.UpdateBasketAsync(basket);
            if (basket is null) return null;

            return basket;

        }

        public async Task<Order> UpdatePaymentIntentForSucceedOrFailed(string paymentIntentId, bool flag)
        {
            var spec = new OrderSpecificationWithPaymentIntetId(paymentIntentId);
            var order = await _unitOfWork.Repository<Order, int>().GetWithSpecAsync(spec);
            //if (order == null)
            //{
            //    _logger.LogError("Order with PaymentIntentId {paymentIntentId} not found", paymentIntentId);
            //    return null; // Or handle it gracefully
            //}
            if (flag)
            {
                order.Status = OrderStatus.PaymentReceived;
            }
            else
            {
                order.Status = OrderStatus.PaymentFailed;
            }

            _unitOfWork.Repository<Order, int>().Update(order);

            await _unitOfWork.CompleteAsync();

            return order;
        }
    }
}
