using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregate;
using Talabat.Core.Repositiories;
using Talabat.Core.Services;
using Talabat.Core.Specifications;

namespace Talabat.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IBasketRepository basketRepository, IUnitOfWork unitOfWork)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Order?>  CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethodId, Address ShippingAddress)
        {
            //Basket
            var Basket = await _basketRepository.GetBasketAsync(BasketId);
            //OrderItems
            var OrderItems = new List<OrderItem>();

            if (Basket?.BasketItems.Count>0)
            {
                foreach (var item in Basket.BasketItems)
                {
                    var Product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var OrderedProductDetails = new OrderedProductDetails(Product.Id,Product.Name,Product.PictureUrl);
                    var OrderItem = new OrderItem(OrderedProductDetails,Product.Price,item.Quantity);
                    OrderItems.Add(OrderItem);
                }
            }
            //Subtotal
            var Subtotal = OrderItems.Sum(O=>O.Price*O.Quantity);
            //DeliveryMethod
            var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethodId);
            //Create Order
            var Spec = new OrderSpecForPaymentIntent(Basket.PaymentIntentId);
            var ExOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(Spec);
            if (ExOrder is not null)
            {
                _unitOfWork.Repository<Order>().Delete(ExOrder);
                await _unitOfWork.CompleteAsync();
            }
            var Order = new Order(BuyerEmail,ShippingAddress,DeliveryMethod,OrderItems,Subtotal,Basket.PaymentIntentId);
            //Added Order Locally
             await _unitOfWork.Repository<Order>().AddAsync(Order);
            //Save Data To Database
            var Result = await _unitOfWork.CompleteAsync();
            if (Result<=0) return null;
            return Order;
        }

        public async Task<Order> GetOrderByIdForSpecificUser(string BuyerEmail, int OrderId)
        {
            var Spec = new OrderSpecWithIncludesAndCriteria(BuyerEmail, OrderId);
            var Order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(Spec);
            return Order;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForSpecificUser(string BuyerEmail)
        {
            var Spec = new OrderSpecWithIncludesAndCriteria(BuyerEmail);
            var Orders = await _unitOfWork.Repository<Order>().GetEntitiesWithSpecAsync(Spec);
            return Orders;
        }
    }
}
