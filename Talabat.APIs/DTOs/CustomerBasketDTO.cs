using Talabat.Core.Entities;

namespace Talabat.APIs.DTOs
{
    public class CustomerBasketDTO
    {
        public string Id { get; set; }
        public List<BasketItemsDTO> BasketItems { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public int? DeliveryMethodId { get; set; }
    }
}
