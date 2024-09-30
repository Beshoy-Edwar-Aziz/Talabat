using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Core.Services
{
    public interface IPaymentService
    {
        public Task<CustomerBasket?> CreateOrUpdatePaymentIntentAsync(string BasketId);
        public Task<Order> UpdatePaymentStatusAsync(string PaymentIntentId, bool Flag);
    }
}
