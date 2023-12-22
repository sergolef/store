using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {

        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork, IBasketRepository basketRepo)
        {
            _unitOfWork = unitOfWork;
            _basketRepo = basketRepo;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodsId, string basketId, Address shippingAddress)
        {
            //get basket from repo
            var basket = await _basketRepo.GetCustomerBasketAsync(basketId);
            //get items from product repo

            var items = new List<OrderItem>();
            foreach(var item in basket.Items){
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);

                var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
                items.Add(orderItem);
            }
            //get delivery method
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodsId);
            //calc subtotal
            var subtotal = items.Sum(item => item.Price * item.Quantity);

            //check if order with intent id exists
            var spec = new OrderByPaymentIntentIdSpecification(basket.PaymentIntentId);
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
            if(order != null)
            {
                
                order.ShipToAddress = shippingAddress;
                order.DeliveryMethod = deliveryMethod;
                order.Subtotal = subtotal;
                _unitOfWork.Repository<Order>().Update(order); 
            }
            else
            {
                //create order
                order = new Order(items, buyerEmail, shippingAddress, deliveryMethod, subtotal,
                    basket.PaymentIntentId);
                _unitOfWork.Repository<Order>().Add(order);
            }

            
            
            //save to db
            
            var result = await _unitOfWork.Complete();

            if(result <= 0){
                return null;
            }

            //delete basket
            //await _basketRepo.DeleteCustomerBasket(basketId);
            
            //return order
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
           return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec = new OrderWithItemsAndOrderingSpecification(id, buyerEmail); 
            
            return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrderWithItemsAndOrderingSpecification(buyerEmail); 
            
            return await _unitOfWork.Repository<Order>().ListAsync(spec);
        }
    }
}