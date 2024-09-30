using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.Extensions.Configuration;
using Stripe;
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
using Product = Talabat.Core.Entities.Product;

namespace Talabat.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public PaymentService(IBasketRepository basketRepository,
            IUnitOfWork unitOfWork,
            IConfiguration configuration)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntentAsync(string BasketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeKeys:SecretKey"];
            var Basket = await _basketRepository.GetBasketAsync(BasketId);
            if (Basket is null) return null;
            if (Basket.BasketItems.Count>0)
            {
                foreach (var item in Basket.BasketItems)
                {
                    var Product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    if (item.Price != Product.Price)
                    item.Price = Product.Price; 
                }
            }
            var SubTotal = Basket.BasketItems.Sum(O=>O.Price*O.Quantity);
            var ShippingCost = 0M;
            if (Basket.DeliveryMethodId.HasValue) {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(Basket.DeliveryMethodId.Value);
                ShippingCost = deliveryMethod.Cost;
            }
            var Service = new PaymentIntentService();
            PaymentIntent paymentIntent;
            if (string.IsNullOrEmpty(Basket.PaymentIntentId))
            {
                var Options = new PaymentIntentCreateOptions()
                {
                    Amount = (long) (SubTotal *100 + ShippingCost*100),
                    PaymentMethodTypes = new List<string>()
                    {
                        "card"
                    },
                    Currency= "usd"
                };
                paymentIntent= await Service.CreateAsync(Options);
                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var Options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)(SubTotal * 100)
                };
                paymentIntent = await Service.UpdateAsync(Basket.PaymentIntentId, Options);
                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;
            }
            return Basket;
        }
        public async Task<Order> UpdatePaymentStatusAsync(string PaymentIntentId, bool Flag)
        {
            var Spec = new OrderSpecForPaymentIntent(PaymentIntentId);
            var Order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(Spec);
            if (Flag)
            {
                Order.Status = OrderStatus.PaymentRecieved;
            }
            else
            {
                Order.Status = OrderStatus.PaymentFailed;
            }
            _unitOfWork.Repository<Order>().Update(Order);
            return Order;
        }
    }
}
