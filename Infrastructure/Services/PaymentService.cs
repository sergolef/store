using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.Extensions.Configuration;
using Stripe;
using Product = Core.Entities.Product;

namespace Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IBasketRepository basketRepository, IUnitOfWork unitOfWork,
        IConfiguration config)
        {
            _basketRepository = basketRepository;
            _config = config;
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];
            var basket = await _basketRepository.GetCustomerBasketAsync(basketId);
            if(basket == null) return null;

            var shippingPrice = 0m;

            if(basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync((int)basket.DeliveryMethodId);
                shippingPrice = deliveryMethod.Price;
            }

            foreach( var item in basket.Items)
            {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                if(item.Price != productItem.Price)
                {
                    item.Price = productItem.Price;
                }
            }

            var service = new PaymentIntentService();
            PaymentIntent intent;
            if(string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long) basket.Items.Sum(i => i.Quantity * (i.Price * 100)) +
                    (long) shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> {"card"}
                };

                intent = await service.CreateAsync(options);
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                     Amount = (long) basket.Items.Sum(i => i.Quantity * (i.Price * 100)) +
                    (long) shippingPrice * 100
                };
                await service.UpdateAsync(basket.PaymentIntentId, options);
            }

            await _basketRepository.UpdateCustomerBusket(basket);

            return basket;
        }

        public async Task<Order> UpdateOrderPaymentFailed(string paymentIntentId)
        {
            var spec = new OrderByPaymentIntentIdSpecification(paymentIntentId);
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

            if(order == null) return null;
            order.Status = OrderStatus.PaymentFailed;

            await _unitOfWork.Complete();

            return order;
        }

        public async Task<Order> UpdateOrderPaymentSuccseed(string paymentIntentId)
        {
            var spec = new OrderByPaymentIntentIdSpecification(paymentIntentId);
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

            if(order == null) return null;
            order.Status = OrderStatus.PaymentReceived;

            await _unitOfWork.Complete();

            return order;
        }
    }
}