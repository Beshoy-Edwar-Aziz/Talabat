using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.OrderAggregate
{
    public class OrderItem:BaseEntity
    {
        public OrderItem()
        {
            
        }
        public OrderItem(OrderedProductDetails orderedProductDetails, decimal price, int quantity)
        {
            OrderedProductDetails = orderedProductDetails;
            Price = price;
            Quantity = quantity;
        }

        public OrderedProductDetails OrderedProductDetails { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
