using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Core.Specifications
{
    public class OrderSpecForPaymentIntent:BaseSpecifications<Order>
    {
        public OrderSpecForPaymentIntent(string PaymentIntentId):base(O=>O.PaymentIntentId==PaymentIntentId)
        {
            
        }
    }
}
