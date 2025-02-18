using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using Store.Codex.Core;
using Store.Codex.Core.Entities;
using Store.Codex.Core.Entities.Order;
using Store.Codex.Core.Repositories.Contract;
using Store.Codex.Core.Services.Contract;
using Store.Codex.Core.Specifications.Orders;
using Store.Codex.Repository.Repositories;
using Store.Codex.Service.Payments;
using Order = Store.Codex.Core.Entities.Order.Order;

namespace Store.Codex.Service.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketRepository _basketRepository;
        private readonly IPaymentService _paymentService;

        public OrderService(IUnitOfWork unitOfWork,IBasketRepository basketRepository, IPaymentService paymentService) // to generate a repository of UnitOfWork
        {
            _unitOfWork = unitOfWork;
            _basketRepository = basketRepository;
            _paymentService = paymentService;
        }
        public async Task<Core.Entities.Order.Order> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId);
            if (basket is null) return null;

            var orderItmes = new List<OrderItem>();
            if(basket.Items.Count() >0)
            {
                foreach(var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product, int>().GetAsync(item.Id);
                    var productOrderedItem = new ProductItemOrder(product.Id, product.Name, product.PictureUrl);
                    var orderItem = new OrderItem(productOrderedItem, product.Price, item.Quantity);

                    orderItmes.Add(orderItem);
                }
            }

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetAsync(deliveryMethodId);

            var subtotal = orderItmes.Sum(I => I.Price * I.Quantity);

            // TODO

            if(!string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var spec = new OrderSpecificationWithPaymentIntetId(basket.PaymentIntentId);
                var ExOrder = await _unitOfWork.Repository<Order, int>().GetWithSpecAsync(spec);
                _unitOfWork.Repository<Order, int>().Delete(ExOrder); 
            }

                var basketDto = await _paymentService.CreateOrUpdatePaymentIntentIdAsync(basketId);



            var order = new Order(buyerEmail,shippingAddress, deliveryMethod, orderItmes, subtotal, basketDto.PaymentIntentId);

            await _unitOfWork.Repository<Order,int>().AddAsync(order);

            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0) return null;

            return order;

        }

        public async Task<Order?> GetOrderByIdForSpecificUserAsync(string buyerEmail, int orderId)
        {
            var spec = new OrderSpecifications(buyerEmail, orderId);
            var order = await _unitOfWork.Repository<Core.Entities.Order.Order, int>().GetWithSpecAsync(spec);
            if (order is null) return null;
            return order;
        }

        public async Task<IEnumerable<Order>?> GetOrderForSpecificUserAsync(string buyerEmail)
        {
            var spec = new OrderSpecifications(buyerEmail);
            var orders = await _unitOfWork.Repository<Order, int>().GetAllWithSpecAsync(spec);

            if (orders is null) return null;

            return orders;
        }


    }
}
