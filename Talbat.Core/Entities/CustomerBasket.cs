using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
    public class CustomerBasket
    {
        public CustomerBasket()
        {

        }
        public CustomerBasket(string? BasketId)
        {
            Id = BasketId;
        }
        public string Id { get; set; }
        public List<BasketItems> BasketItems { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public int? DeliveryMethodId { get; set; }
    }
}
