namespace Talabat.APIs.DTOs
{
    public class OrderDTO
    {
        public int DeliveryMethodId { get; set; }
        public AddressDTO ShippingAddress { get; set; }
        public string BasketId { get; set; }
    }
}
