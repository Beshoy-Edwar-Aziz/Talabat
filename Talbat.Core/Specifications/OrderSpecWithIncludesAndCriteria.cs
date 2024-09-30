using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Core.Specifications
{
    public class OrderSpecWithIncludesAndCriteria:BaseSpecifications<Order>
    {
        public OrderSpecWithIncludesAndCriteria(string BuyerEmail):base(O=>O.BuyerEmail == BuyerEmail)
        {
            Includes.Add(O=>O.DeliveryMethod);
            Includes.Add(O=>O.OrderItems);
        }
        public OrderSpecWithIncludesAndCriteria(string BuyerEmail, int OrderId):base(O=>O.BuyerEmail==BuyerEmail && O.Id == OrderId)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.OrderItems);
        }
    }
}
