using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.APIs.DTOs
{
    public class OrderToReturnDTO
    {
        public int id {  get; set; }
        [Required]
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        [Required]
        public string Status { get; set; }
        public AddressDTO ShippingAddress { get; set; }
        [Required]
        public string DeliveryMethodName { get; set; }
        public decimal DeliveryAmount { get; set; }
        public ICollection<OrderItemDTO> OrderItems { get; set; } = new HashSet<OrderItemDTO>();
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        [Required]
        public string PaymentIntentId { get; set; } 
    }
}
